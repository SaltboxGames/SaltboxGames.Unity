using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SaltboxGames.Unity.Utilities
{
    [CreateAssetMenu(menuName = "Input/Action Group")]
    public class ActionGroup : ScriptableObject
    {
        [SerializeField]
        private bool _startDisabled;
        
        [SerializeField]
        private List<InputActionReference> actions = new List<InputActionReference>();
        
        private bool isEnabled;
        public bool IsIsEnabled => isEnabled;

        private void Awake()
        {
            SetEnabled(!_startDisabled);
        }

        public void SetEnabled(bool enabled)
        {
            isEnabled = enabled;
            if (enabled)
            {
                foreach (InputActionReference inputAction in actions)
                {
                    inputAction.action.Enable();
                }
            }
            else
            {
                foreach (InputActionReference inputAction in actions)
                {
                    inputAction.action.Disable();
                }
            }
        }
    }
}
