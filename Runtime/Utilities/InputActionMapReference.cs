using UnityEngine;
using UnityEngine.InputSystem;

namespace SaltboxGames.Unity.Utilities
{
    
    [CreateAssetMenu(menuName = "Input/Action Map Reference")]
    public class InputActionMapReference : ScriptableObject
    {
        [SerializeField] private InputActionAsset asset;
        [SerializeField] private string mapId; // could be GUID instead of name

        private InputActionMap cachedMap;

        public InputActionMap ActionMap
        {
            get
            {
                if (cachedMap == null && asset != null)
                {
                    cachedMap = asset.FindActionMap(mapId, throwIfNotFound: true);
                }
                return cachedMap;
            }
        }
    }
}
