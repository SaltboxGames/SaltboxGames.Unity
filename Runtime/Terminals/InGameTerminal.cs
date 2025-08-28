/*
 * Copyright (c) 2024 SaltboxGames, Jonathan Gardner
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */


using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using SaltboxGames.Core.Terminals;
using SaltboxGames.Core.Utilities;
using SaltboxGames.Unity.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using LogType = SaltboxGames.Core.Terminals.LogType;

namespace SaltboxGames.Unity
{
    public class InGameTerminal : MonoBehaviour, ITerminal
    {
        private const int max_lines = 300;
        private const int max_history_entries = 100;

        [ThreadStatic]
        private static StringBuilder _message_builder;

        [SerializeField, Header("UI References")]
        private GameObject _terminalRoot;
        private bool IsOpen => _terminalRoot.activeSelf;
        
        [SerializeField]
        private TMP_Text _logText;
        [SerializeField]
        private ScrollRect _scrollRect;
        [SerializeField]
        private TMP_InputField _inputField;
        
        [SerializeField]
        [Header("Controls")]
        private InputActionReference _toggleAction;
        [SerializeField]
        private InputActionReference _autoCompleteAction;
        [SerializeField]
        private InputActionReference _historyUpAction;
        [SerializeField]
        private InputActionReference _historyDownAction;

        [SerializeField]
        public UnityEvent OnOpen;
        [SerializeField]
        public UnityEvent OnClose;

        private readonly Queue<string> logQueue = new Queue<string>();
        private readonly ConcurrentQueue<string> pendingLogLines = new ConcurrentQueue<string>();
        private readonly ConcurrentQueue<string> inputQueue = new ConcurrentQueue<string>();
        private readonly SemaphoreSlim signal = new SemaphoreSlim(0, 1);

        private readonly List<string> history = new List<string>();
        private int historyIndex = -1; // -1 = not browsing

        private ICommandRunner commandRunner;
        
        private void Awake()
        {
            _inputField.onSubmit.AddListener(OnInputSubmitted);
            
            _toggleAction.action.performed += _ => SetActive(!IsOpen);
            _historyUpAction.action.performed += _ => BrowseHistory(-1);
            _historyDownAction.action.performed += _ => BrowseHistory(1);
            _autoCompleteAction.action.performed += context =>
            {
                _ = HandleAutocomplete();
            };

            _terminalRoot.SetActive(false);
            _autoCompleteAction.action.Disable();
            _historyUpAction.action.Disable();
            _historyDownAction.action.Disable();
        }

        private void Update()
        {
            // Drain pending logs from any thread
            bool logChanges = false;
            while (pendingLogLines.TryDequeue(out string log))
            {
                logQueue.Enqueue(log);
                while (logQueue.Count > max_lines)
                {
                    logQueue.Dequeue();
                }
                logChanges = true;
            }

            if (!IsOpen)
            {
                return;
            }

            if (logChanges)
            {
                RefreshDisplay();
            }
        }

        public void SetActive(bool state)
        {
            _terminalRoot.SetActive(state);
            if (state)
            {
                _autoCompleteAction.action.Enable();
                _historyUpAction.action.Enable();
                _historyDownAction.action.Enable();

                OnOpen?.Invoke();

                UniTask.NextFrame()
                    .ContinueWith(_inputField.ActivateInputField);
                RefreshDisplay();
            }
            else
            {
                _autoCompleteAction.action.Disable();
                _historyUpAction.action.Disable();
                _historyDownAction.action.Disable();

                OnClose?.Invoke();
            }
        }

        private static string GetColor(LogType type)
        {
            return type switch
            {
                LogType.Warning => "yellow",
                LogType.Error => "orange",
                LogType.Assert => "magenta",
                LogType.Exception => "red",
                _ => "white",
            };
        }

        public void LogFormat(LogType logType, LogCategory category, string format, params object[] args)
        {
            _message_builder ??= new StringBuilder();
            _message_builder.Clear();

            if (category != LogCategory.TerminalResponse)
            {
                _message_builder.Append($"[{category}] <color={GetColor(logType)}>[{logType}]</color> ");
            }

            _message_builder.Append(string.Format(format, args));
            AppendLine(_message_builder.ToString());
        }

        public void LogException(Exception exception, LogCategory category)
        {
            _message_builder ??= new StringBuilder();
            _message_builder.Clear();

            if (category != LogCategory.TerminalResponse)
            {
                _message_builder.Append($"[{category}] <color={GetColor(LogType.Exception)}>[{LogType.Exception}]</color>" );
            }

            _message_builder.Append(exception.Message);
            AppendLine(_message_builder.ToString());
        }

        public async Task StartTerminal(ICommandRunner commandRunner, CancellationToken cancellationToken)
        {
            this.commandRunner = commandRunner;

            while (!cancellationToken.IsCancellationRequested)
            {
                await signal.WaitAsync(cancellationToken);
                if (!inputQueue.TryDequeue(out string line))
                {
                    continue;
                }

                CommandResult result = await commandRunner.Execute(line);
                this.LogCommandResult(result);
            }
        }

        private void OnInputSubmitted(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return;
            }

            if (history.Count == 0 || history[^1] != input)
            {
                history.Add(input);
                if (history.Count > max_history_entries)
                {
                    history.RemoveAt(0);
                }
            }

            historyIndex = -1;

            _inputField.text = "";

            pendingLogLines.Enqueue(input);
            inputQueue.Enqueue(input);
            signal.Release();
            
            _inputField.ActivateInputField();
        }

        private void AppendLine(string message)
        {
            // Just enqueue for thread safety
            pendingLogLines.Enqueue(message);
        }

        private void RefreshDisplay()
        {
            _logText.text = string.Join("\n", logQueue);
            Canvas.ForceUpdateCanvases();
            _scrollRect.verticalNormalizedPosition = 0;
        }

        private async Task HandleAutocomplete()
        {
            if (commandRunner == null)
            {
                return;
            }

            string current = _inputField.text;
            if (string.IsNullOrWhiteSpace(current))
            {
                return;
            }

            string[] parts = current.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0)
            {
                return;
            }

            string verbPart = parts[0];
            string rest = current.Substring(verbPart.Length);

            List<string> matches = await commandRunner.TryAutoComplete(verbPart);

            if (matches == null || matches.Count == 0)
            {
                return;
            }

            string common = StringUtilities.LongestCommonPrefix(matches);
            if (!string.IsNullOrWhiteSpace(common) && common.Length > verbPart.Length)
            {
                _inputField.text = common + rest;
                _inputField.caretPosition = _inputField.text.Length;
            }
            else if (matches.Count > 1)
            {
                AppendLine($"<color=grey>Matches:</color> {string.Join(", ", matches)}");
            }
        }

        private void BrowseHistory(int direction)
        {
            Debug.Log("Testing");
            
            if (history.Count == 0)
            {
                return;
            }

            if (historyIndex == -1)
            {
                historyIndex = history.Count;
            }

            historyIndex += direction;

            if (historyIndex < 0)
            {
                historyIndex = 0;
            }
            else if (historyIndex >= history.Count)
            {
                historyIndex = -1;
                _inputField.text = "";
                return;
            }

            _inputField.text = history[historyIndex];
            _inputField.caretPosition = _inputField.text.Length;
        }
    }
}
