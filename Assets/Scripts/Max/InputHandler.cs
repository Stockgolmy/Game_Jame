using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;

    public Vector2 mouseScreenPosition;
    public float scrollDelta;
    public bool leftMousePressed;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Mouse.current == null) return;

        mouseScreenPosition = Mouse.current.position.ReadValue();
        scrollDelta = Mouse.current.scroll.ReadValue().y;
        leftMousePressed = Mouse.current.leftButton.wasPressedThisFrame;
    }
}