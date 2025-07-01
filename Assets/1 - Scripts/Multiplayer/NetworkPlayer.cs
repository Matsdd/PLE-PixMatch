using Fusion;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    [Networked] public NetworkString<_32> PlayerName { get; set; }
    [Networked] public int SkinIndex { get; set; }

    // RPC for client to send their data to the host (StateAuthority)
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SendPlayerData(string name, int skin)
    {
        PlayerName = name;
        SkinIndex = skin;

        Debug.Log($"[Host] Received player data from client: Name={name}, Skin={skin}");
    }

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            // Get local saved data
            string localName = PlayerPrefs.GetString("PlayerNameKey", "Player");
            int localSkin = PlayerPrefs.GetInt("PlayerSkinKey", 0);

            // Immediately set local networked vars (client-side prediction)
            PlayerName = localName;
            SkinIndex = localSkin;

            // Send data to host
            RPC_SendPlayerData(localName, localSkin);

            Debug.Log($"[Local Player] Sent player data: Name={localName}, Skin={localSkin}");
        }
    }
}