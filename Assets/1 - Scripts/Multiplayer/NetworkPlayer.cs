using Fusion;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    [Networked] public string PlayerName { get; set; }
    [Networked] public int SkinIndex { get; set; }

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            PlayerName = PlayerPrefs.GetString("PlayerName", "Player");
            SkinIndex = PlayerPrefs.GetInt("SkinIndex", 0);
        }
    }
}