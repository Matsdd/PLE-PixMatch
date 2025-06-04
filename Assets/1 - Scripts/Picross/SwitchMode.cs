using UnityEngine;
using TMPro;

public class SwitchMode : MonoBehaviour
{
    public BoardManager board;
    public TMP_Text modeLabel;

    void Start()
    {
        UpdateModeLabel();
    }

    public void ToggleMode()
    {
        if (board.currentMode == BoardManager.DrawMode.Fill)
            board.currentMode = BoardManager.DrawMode.Mark;
        else
            board.currentMode = BoardManager.DrawMode.Fill;

        UpdateModeLabel();
    }

    void UpdateModeLabel()
    {
        if (modeLabel != null)
        {
            modeLabel.text = "Mode: " + board.currentMode.ToString();
        }
    }
}