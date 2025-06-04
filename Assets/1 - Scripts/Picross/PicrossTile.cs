using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PicrossTile : MonoBehaviour, IPointerClickHandler
{
    public Image tileImage;
    private BoardManager board;
    private int x, y;

    private enum State { Empty, Filled, Marked, Wrong }
    private State currentState = State.Empty;

    public Color emptyColor = Color.white;
    public Color filledColor = Color.black;
    public Color markedColor = Color.gray;
    public Color wrongColor = Color.red;

    public void Init(BoardManager board, int x, int y)
    {
        this.board = board;
        this.x = x;
        this.y = y;
        UpdateVisual();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentState == State.Filled || currentState == State.Wrong)
            return;

        if (board.currentMode == BoardManager.DrawMode.Fill)
        {
            if (board.IsCorrect(x, y, true))
            {
                currentState = State.Filled;
            }
            else
            {
                currentState = State.Wrong;
                board.RegisterWrongAttempt();
            }
        }
        else if (board.currentMode == BoardManager.DrawMode.Mark)
        {
            currentState = State.Marked;
        }

        UpdateVisual();
    }


    void UpdateVisual()
    {
        switch (currentState)
        {
            case State.Empty: tileImage.color = emptyColor; break;
            case State.Filled: tileImage.color = filledColor; break;
            case State.Marked: tileImage.color = markedColor; break;
            case State.Wrong: tileImage.color = wrongColor; break;
        }
    }

    public bool IsFilled() => currentState == State.Filled;
}