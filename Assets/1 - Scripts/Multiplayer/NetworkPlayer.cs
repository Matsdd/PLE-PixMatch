using Fusion;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    [Networked] public NetworkString<_16> PlayerName { get; set; }
    [Networked] public int SkinIndex { get; set; }

    public Renderer playerRenderer;
    public Material[] skinMaterials;

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            // This is the local player
            string savedName = PlayerPrefs.GetString("PlayerName", "Player");
            int savedSkin = PlayerPrefs.GetInt("currentSkinIndex", 0);

            RPC_SetPlayerInfo(savedName, savedSkin);
        }

        ApplySkin(SkinIndex);
    }

    public override void Render()
    {
        ApplySkin(SkinIndex);
    }

    void ApplySkin(int index)
    {
        if (playerRenderer != null && skinMaterials.Length > 0 && index < skinMaterials.Length)
        {
            playerRenderer.material = skinMaterials[index];
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    void RPC_SetPlayerInfo(string name, int skin)
    {
        PlayerName = name;
        SkinIndex = skin;
    }
}