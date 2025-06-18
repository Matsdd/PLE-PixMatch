using UnityEngine;

public class ModeButtonHandler : MonoBehaviour
{
    public void SetQuickplay()
    {
        ModeSelector.IsQuickplay = true;
    }

    public void SetMultiplayer()
    {
        ModeSelector.IsQuickplay = false;
    }
}