using UnityEngine;

public class MouseBehaviour : MonoBehaviour
{
    [SerializeField] private Texture2D cursorArrow;
    private Vector2 _cursorHotspot;
    private void Start()
    {
        _cursorHotspot = new Vector2(cursorArrow.width / 2, cursorArrow.height / 2);
        Cursor.SetCursor(cursorArrow, _cursorHotspot, CursorMode.Auto);
    }
}
