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
