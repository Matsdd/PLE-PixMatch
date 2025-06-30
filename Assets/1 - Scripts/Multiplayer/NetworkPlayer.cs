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
            string localName = PlayerPrefs.GetString("PlayerName", "Player");
            PlayerName = localName; // ✅ Implicit conversion works
            SkinIndex = PlayerPrefs.GetInt("SkinIndex", 0);

            Debug.Log($"Spawned NetworkPlayer with name: {PlayerName} skin: {SkinIndex}");
        }
    }
}