using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;

    public Vector3 mouseScreenPosition;
    public float scrollDelta;
    public bool leftMousePressed;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        mouseScreenPosition = Input.mousePosition;
        scrollDelta = Input.mouseScrollDelta.y;
        leftMousePressed = Input.GetMouseButtonDown(0);
    }
}