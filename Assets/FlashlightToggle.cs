using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightToggle : MonoBehaviour
{
    public Light flashlight; // Assign in inspector
    public InputActionReference toggleAction; // Assign action from Input System

    private void OnEnable()
    {
        if (toggleAction != null)
            Debug.Log("Action detected!");
            toggleAction.action.performed += OnToggle;
    }

    private void OnDisable()
    {
        if (toggleAction != null)
            Debug.Log("Disable detected!");
            toggleAction.action.performed -= OnToggle;
    }

    private void OnToggle(InputAction.CallbackContext context)
    {
        if (flashlight != null)
            flashlight.enabled = !flashlight.enabled;
    }
}
