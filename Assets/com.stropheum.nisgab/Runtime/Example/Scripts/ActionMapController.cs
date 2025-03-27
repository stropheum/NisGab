using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace NisGab.Example
{
    public class ActionMapController : MonoBehaviour
    {
        [SerializeField] private Color _activeColor = Color.green;
        [SerializeField] private Color _inactiveColor = Color.red;
        [SerializeField] private Toggle _togglePlayerInputButton;
        [SerializeField] private Toggle _toggleUIInputButton;

        private void Awake()
        {
            Debug.Assert(_togglePlayerInputButton != null, "_togglePlayerInputButton != null");
            Debug.Assert(_toggleUIInputButton != null, "_toggleUiInputButton != null");
            InputEvent.Instance.EnablePlayerInput();
            InputEvent.Instance.EnableUIInput();      
            InputEvent.Player.Interact += PlayerOnInteract;
        }

        private void Start()
        {
            _togglePlayerInputButton.onValueChanged.AddListener(OnTogglePlayerInput);
            _toggleUIInputButton.onValueChanged.AddListener(OnToggleUiInput);
            OnTogglePlayerInput(_togglePlayerInputButton.isOn);
            OnToggleUiInput(_toggleUIInputButton.isOn);
        }

        private void OnTogglePlayerInput(bool isActive)
        {
            _togglePlayerInputButton.targetGraphic.color = isActive ? _activeColor : _inactiveColor;
            if (isActive)
            {
                InputEvent.Instance.EnablePlayerInput();
            }
            else
            {
                InputEvent.Instance.DisablePlayerInput();
            }
        }

        private void OnToggleUiInput(bool isActive)
        {
            _toggleUIInputButton.targetGraphic.color = isActive ? _activeColor : _inactiveColor;
            if (isActive)
            {
                InputEvent.Instance.EnableUIInput();
            }
            else
            {
                InputEvent.Instance.DisableUIInput();
            }
        }
        
        private void PlayerOnInteract(InputAction.CallbackContext context)
        {
            InputEvent.Instance.EnableUIInput();
        }
    }
}
