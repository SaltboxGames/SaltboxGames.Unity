/*
 * Copyright (c) 2024 SaltboxGames, Jonathan Gardner
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */


using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using SaltboxGames.Core.Terminals;
using UnityEngine;
using LogType = SaltboxGames.Core.Terminals.LogType;

namespace SaltboxGames.Unity
{
    public class UnityLog : ITerminal
    {
        [HideInCallstack]
        public void LogFormat(LogType logType, LogCategory category, string format, params object[] args)
        {
            Debug.LogFormat(logType.ToUnity(), LogOption.None, null, format, args);
        }

        [HideInCallstack]
        public void LogException(Exception exception, LogCategory category)
        {
            Debug.LogException(exception);
        }

        public Task StartTerminal(ICommandRunner commandRunner, CancellationToken cancellationToken)
        {
            // Mildly criminal activity; but GameTerm assumes that if the task ends than the terminal is done.
            return UniTask.WaitUntil(() => cancellationToken.IsCancellationRequested).AsTask();
        }
    }
}
