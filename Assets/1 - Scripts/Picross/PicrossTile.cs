using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PicrossTile : MonoBehaviour, IPointerClickHandler
{
    public Image tileImage;
    private BoardManager _board;
    private int _x, _y;

    private enum State { Empty, Filled, Marked, Wrong }
    private State _currentState = State.Empty;

    public Color emptyColor = Color.white;
    public Color filledColor = Color.black;
    public Color markedColor = Color.gray;
    public Color wrongColor = Color.red;

    public void Init(BoardManager board, int x, int y)
    {
        this._board = board;
        this._x = x;
        this._y = y;
        UpdateVisual();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_currentState == State.Filled || _currentState == State.Wrong)
            return;

        if (_board.currentMode == BoardManager.DrawMode.Fill)
        {
            if (_board.IsCorrect(_x, _y, true))
            {
                _currentState = State.Filled;
                _board.CheckForWin();
            }
            else
            {
                _currentState = State.Wrong;
                _board.RegisterWrongAttempt();
            }
        }
        else if (_board.currentMode == BoardManager.DrawMode.Mark)
        {
            _currentState = State.Marked;
        }

        UpdateVisual();
    }


    void UpdateVisual()
    {
        switch (_currentState)
        {
            case State.Empty: tileImage.color = emptyColor; break;
            case State.Filled: tileImage.color = filledColor; break;
            case State.Marked: tileImage.color = markedColor; break;
            case State.Wrong: tileImage.color = wrongColor; break;
        }
    }

    public bool IsFilled() => _currentState == State.Filled;
}