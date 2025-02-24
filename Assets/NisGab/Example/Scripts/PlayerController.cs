using UnityEngine;
using UnityEngine.InputSystem;

namespace NisGab.Example
{
    public class PlayerController : MonoBehaviour
    {
        private void Awake()
        {
            InputEvent.Player.Interact += PlayerOnInteract;
            InputEvent.UI.Click += UIOnClick;
        }

        private void OnDestroy()
        {
            InputEvent.Player.Interact -= PlayerOnInteract;
            InputEvent.UI.Click -= UIOnClick;
        }

        private void PlayerOnInteract(InputAction.CallbackContext context)
        {
            Debug.Log("PlayerOnInteract");
        }
        
        private void UIOnClick(InputAction.CallbackContext context)
        {
            Debug.Log("UIOnClick");
        }
    }
}
