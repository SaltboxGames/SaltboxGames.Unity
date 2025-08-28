using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SaltboxGames.Unity.Utilities
{
    [CreateAssetMenu(menuName = "Input/Action Group")]
    public class ActionGroup : ScriptableObject
    {
        [SerializeField]
        private List<InputActionReference> actions = new List<InputActionReference>();

        public void SetEnabled(bool isEnabled)
        {
            if (isEnabled)
            {
                Enable();
            }
            else
            {
                Disable();
            }
        }

        public void Enable()
        {
            foreach (InputActionReference inputAction in actions)
            {
                inputAction.action.Enable();
            }
        }

        public void Disable()
        {
            foreach (InputActionReference inputAction in actions)
            {
                inputAction.action.Disable();
            }
        }
    }
}
