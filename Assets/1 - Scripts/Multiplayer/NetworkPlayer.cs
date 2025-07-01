using Fusion;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    [Networked] public NetworkString<_32> PlayerName { get; set; }
    [Networked] public int SkinIndex { get; set; }

    public override void Spawned()
    {
        if (HasInputAuthority)
        {
            string localName = PlayerPrefs.GetString("PlayerNameKey", "Player");
            int localSkin = PlayerPrefs.GetInt("PlayerSkinKey", 0);

            PlayerName = localName;
            SkinIndex = localSkin;

            Debug.Log($"[Local Player] Set own PlayerName={localName}, SkinIndex={localSkin}");
        }
    }
}