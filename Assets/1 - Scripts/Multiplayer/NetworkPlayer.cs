using Fusion;
using UnityEngine;

public class NetworkPlayerData : NetworkBehaviour
{
    [Networked]
    public int SkinIndex { get; set; }

    [Networked]
    public NetworkString<_16> PlayerName { get; set; }

    public Renderer characterRenderer;
    public Material[] skinMaterials;

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            // This is the local player, set skin and name from PlayerPrefs
            SkinIndex = PlayerPrefs.GetInt("currentSkinIndex", 0);
            PlayerName = PlayerPrefs.GetString("PlayerName", "Player");
        }
        else
        {
            // This is the remote player, apply their synced values
            ApplySkin(SkinIndex);
            Debug.Log($"Opponent joined: {PlayerName.ToString()} with skin {SkinIndex}");
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasInputAuthority)
        {
            ApplySkin(SkinIndex);
        }
    }

    void ApplySkin(int index)
    {
        if (characterRenderer != null && skinMaterials.Length > 0)
        {
            index = Mathf.Clamp(index, 0, skinMaterials.Length - 1);
            characterRenderer.material = skinMaterials[index];
        }
    }
}