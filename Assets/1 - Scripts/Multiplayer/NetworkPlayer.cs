using Fusion;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    [Networked] public NetworkString<_32> PlayerName { get; set; }
    [Networked] public int SkinIndex { get; set; }

    // Called by client to send data to server
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetPlayerData(string name, int skin)
    {
        PlayerName = name;
        SkinIndex = skin;

        Debug.Log($"[Server] RPC_SetPlayerData: Name={PlayerName}, Skin={SkinIndex}");

        // Optionally broadcast to everyone that data changed
        RPC_BroadcastPlayerData(PlayerName.ToString(), SkinIndex);
    }

    // Server tells all clients about this player's data
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    void RPC_BroadcastPlayerData(string name, int skin)
    {
        PlayerName = name;
        SkinIndex = skin;

        Debug.Log($"[Client] Received broadcast data: Name={PlayerName}, Skin={SkinIndex}");
    }

    public override void Spawned()
    {
        if (HasInputAuthority)
        {
            string localName = PlayerPrefs.GetString("PlayerNameKey", "Player");
            int skin = PlayerPrefs.GetInt("PlayerSkinKey", 0);

            Debug.Log($"[Client] Sending RPC_SetPlayerData: Name={localName}, Skin={skin}");
            RPC_SetPlayerData(localName, skin);
        }
    }
}