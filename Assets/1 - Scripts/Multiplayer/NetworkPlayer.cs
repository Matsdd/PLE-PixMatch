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
            PlayerName = localName;
            SkinIndex = PlayerPrefs.GetInt("PlayerSkinKey", 0);

            Debug.Log($"Spawned NetworkPlayer with name: {PlayerName} skin: {SkinIndex}");
        }
    }
}