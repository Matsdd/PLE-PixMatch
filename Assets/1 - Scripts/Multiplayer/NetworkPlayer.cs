using Fusion;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    [Networked] public NetworkString<_32> PlayerName { get; set; }
    [Networked] public int SkinIndex { get; set; }
    
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetPlayerData(string name, int skin)
    {
        PlayerName = name;
        SkinIndex = skin;

        Debug.Log($"[Server] RPC_SetPlayerData: Name={PlayerName}, Skin={SkinIndex}");
        
        RPC_BroadcastPlayerData(PlayerName.ToString(), SkinIndex);
    }
    
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

            // Set local properties immediately for local view
            PlayerName = localName;
            SkinIndex = skin;

            Debug.Log($"[Client] Local player set PlayerName and SkinIndex immediately: {localName}, {skin}");

            // Then notify the server so it can update and broadcast to others
            RPC_SetPlayerData(localName, skin);
        }
    }

}