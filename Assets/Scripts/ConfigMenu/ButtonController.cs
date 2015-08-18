using UnityEngine;

public class ButtonController : MonoBehaviour {
    public UnityEngine.UI.InputField widthField;
    public UnityEngine.UI.InputField heightField;
    public GameObject game;

    public void ButtonPressed() {
        int width;
        int height;
        int.TryParse(widthField.text, out width);
        int.TryParse(heightField.text, out height);

        var board = game.GetComponent<Board>();
        Debug.Assert(board != null, "Error, ButtonController.cs - Could not find board component on gameobject");
        board.SetWidth(width);
        board.SetHeight(height);
        board.ReBuildBoard();
    }
}
