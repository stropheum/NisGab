using NisGab;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTester : MonoBehaviour
{
    private void Start()
    {
        InputEvent.Instance.EnablePlayerInput();
        InputEvent.Player.Attack += PlayerOnAttack;
    }
    
    private void OnDestroy()
    {
        InputEvent.Instance.DisablePlayerInput();
    }
    
    private void PlayerOnAttack(InputAction.CallbackContext ctx)
    {
        Debug.Log("Player Shooting");
    }
}