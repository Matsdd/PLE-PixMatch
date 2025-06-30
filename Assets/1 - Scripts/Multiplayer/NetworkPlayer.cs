using UnityEngine;
using Fusion;

public class NetworkPlayer : NetworkBehaviour {
    [Networked] public NetworkString<_32> PlayerName { get; set; }
    [Networked] public int SkinIndex { get; set; }

    private BoardManager boardManager;

    public override void Spawned() {
        boardManager = FindObjectOfType<BoardManager>();

        if (Object.HasInputAuthority) {
            Debug.Log("[Local] I am the local player, requesting host to set my player info.");

            string localName = PlayerPrefs.GetString("PlayerName", "Player");
            int localSkin = PlayerPrefs.GetInt("PlayerSkin", 0);

            RPC_RequestSetPlayerInfo(localName, localSkin);
        } else {
            Debug.Log("[Remote] I am a remote player.");
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_RequestSetPlayerInfo(string name, int skinIndex) {
        Debug.Log($"[Host] Received player info request. Setting PlayerName: {name}, SkinIndex: {skinIndex}");
        PlayerName = name;
        SkinIndex = skinIndex;
    }

    public override void FixedUpdateNetwork() {
        if (boardManager != null) {
            if (Object.HasInputAuthority) {
                boardManager.SetLocalPlayerInfo(PlayerName.ToString(), SkinIndex);
            } else {
                boardManager.SetRemotePlayerInfo(PlayerName.ToString(), SkinIndex);
            }
        }
    }
}