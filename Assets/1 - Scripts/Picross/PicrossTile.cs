using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PicrossTile : MonoBehaviour, IPointerClickHandler
{
    public Image tileImage;
    private PicrossBoard board;
    private int x, y;
    private enum State { Empty, Filled, Marked }
    private State currentState = State.Empty;

    public Color emptyColor = Color.white;
    public Color filledColor = Color.black;
    public Color markedColor = Color.gray;

    public void Init(PicrossBoard board, int x, int y)
    {
        this.board = board;
        this.x = x;
        this.y = y;
        UpdateVisual();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        currentState = (State)(((int)currentState + 1) % 3);
        UpdateVisual();
    }
    
    void OnMouseDown()
    {
        Debug.Log("OnPointerClick");
    }

    void UpdateVisual()
    {
        switch (currentState)
        {
            case State.Empty: tileImage.color = emptyColor; break;
            case State.Filled: tileImage.color = filledColor; break;
            case State.Marked: tileImage.color = markedColor; break;
        }
    }

    public bool IsFilled() => currentState == State.Filled;
}