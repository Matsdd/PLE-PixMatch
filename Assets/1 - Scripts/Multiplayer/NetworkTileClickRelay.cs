using UnityEngine;
using UnityEngine.EventSystems;
using Fusion;

public class NetworkTileClickRelay : MonoBehaviour, IPointerClickHandler
{
    private PicrossTile tile;
    private int x, y;

    void Start()
    {
        tile = GetComponent<PicrossTile>();
        var initField = typeof(PicrossTile).GetField("x", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        x = (int)initField.GetValue(tile);
        y = (int)typeof(PicrossTile).GetField("y", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(tile);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (NetworkGameManager.Instance != null)
        {
            var runner = NetworkRunner.GetRunnerForGameObject(NetworkGameManager.Instance.gameObject);
            var player = runner.LocalPlayer;
            NetworkGameManager.Instance.TryClickTile(player, x, y);
        }
    }
}