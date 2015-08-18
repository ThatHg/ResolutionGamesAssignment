using UnityEngine;

public class ButtonController : MonoBehaviour {
    public UnityEngine.UI.InputField widthField;
    public UnityEngine.UI.InputField heightField;
    public GameObject game;

    public void ButtonPressed() {
        var board = game.GetComponent<Board>();
        Debug.Assert(board != null, "Error, ButtonController.cs - Could not find board component on gameobject");

        int width;
        int height;
        if(int.TryParse(widthField.text, out width)) {
            board.SetWidth(width);
        }
        if(int.TryParse(heightField.text, out height)) {
            board.SetHeight(height);
        }
        board.ReBuildBoard();
    }
}
