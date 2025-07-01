using Fusion;
using UnityEngine;
using System.Collections;

public class NetworkPlayer : NetworkBehaviour
{
    [Networked] public string PlayerName { get; set; }
    [Networked] public int SkinIndex { get; set; }

    public override void Spawned()
    {
        if (HasInputAuthority)
        {
            RPC_SetPlayerData(
                PlayerPrefs.GetString("PlayerName", "Player"),
                PlayerPrefs.GetInt("currentSkinIndex", 0)
            );
        }
    }

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    private void RPC_SetPlayerData(string name, int skinIndex)
    {
        PlayerName = name;
        SkinIndex = skinIndex;
    }
}
