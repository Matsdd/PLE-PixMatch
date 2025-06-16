using Fusion;
using UnityEngine;

public class NetworkGameManager : NetworkBehaviour
{
    public BoardManager boardManager;
    
    public static Color Player1Color = Color.green;
    public static Color Player2Color = Color.blue;

    public static NetworkGameManager Instance;

    [Networked] private int playerCount { get; set; }

    public override void Spawned()
    {
        Instance = this;

        if (Object.HasStateAuthority)
        {
            playerCount = 0;
        }
    }

    public Color GetPlayerColor(PlayerRef player)
    {
        return player.RawEncoded % 2 == 0 ? Player1Color : Player2Color;
    }

    public void TryClickTile(PlayerRef player, int x, int y)
    {
        RPC_HandleTileClick(player, x, y);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    void RPC_HandleTileClick(PlayerRef player, int x, int y)
    {
        var tile = boardManager.tileObjects[x, y];
        var tileComp = tile.GetComponent<PicrossTile>();
        if (tileComp != null)
        {
            tileComp.OnPointerClick(new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current));
            tileComp.tileImage.color = GetPlayerColor(player);
        }
    }
}