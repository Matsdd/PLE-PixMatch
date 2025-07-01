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

        Debug.Log($"[Server] RPC received: Name = {PlayerName}, Skin = {SkinIndex}");
    }
    
    public override void Spawned()
    {
        if (HasInputAuthority)
        {
            string localName = PlayerPrefs.GetString("PlayerNameKey", "Player");
            int skin = PlayerPrefs.GetInt("PlayerSkinKey", 0);

            RPC_SetPlayerData(localName, skin);

            Debug.Log($"[Client] Sent RPC to set name = {localName}, skin = {skin}");
        }
    }

}