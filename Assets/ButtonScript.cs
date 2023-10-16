using UnityEngine;
//using UnityEngine.UIElements;

public class ButtonScript : MonoBehaviour
{
    public Texture2D customCursor;

    public void MouseEnter() 
    {
        Cursor.SetCursor(customCursor, Vector2.zero, CursorMode.Auto);
    }

    public void MouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}