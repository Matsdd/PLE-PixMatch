using Fusion;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    [Networked]
    public NetworkString<_32> PlayerName { get; set; }

    [Networked]
    public int SkinIndex { get; set; }

    public BoardManager boardManager;
    public BasicSpawner spawner;

    // Local cached info received from the host
    private string _receivedHostName;
    private int _receivedHostSkin;

    public string ReceivedHostName => _receivedHostName;
    public int ReceivedHostSkin => _receivedHostSkin;

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            // When this player spawns, send their saved info to the host
            string name = PlayerPrefs.GetString("PlayerName", "Player");
            int skin = PlayerPrefs.GetInt("PlayerSkin", 0);

            RPC_SendPlayerInfoToHost(name, skin);
        }
    }

    // Call this to manually set player name (e.g. UI or on join)
    public void SetPlayerName(string playerName)
    {
        if (Object.HasInputAuthority)
        {
            RPC_SendPlayerInfoToHost(playerName, SkinIndex);
        }
        else
        {
            Debug.LogWarning("Only InputAuthority can set player name");
        }
    }

    // Call this to manually set skin index
    public void SetSkinIndex(int skinIndex)
    {
        if (Object.HasInputAuthority)
        {
            RPC_SendPlayerInfoToHost(PlayerName.ToString(), skinIndex);
        }
        else
        {
            Debug.LogWarning("Only InputAuthority can set skin index");
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_SendPlayerInfoToHost(string playerName, int skinIndex)
    {
        // Host sets the networked values
        PlayerName = playerName;
        SkinIndex = skinIndex;

        // Then host broadcasts its own info back to the client
        string hostName = PlayerPrefs.GetString("PlayerName", "Host");
        int hostSkin = PlayerPrefs.GetInt("PlayerSkin", 0);
        RPC_SendHostInfoToClient(hostName, hostSkin);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority)]
    private void RPC_SendHostInfoToClient(string hostName, int hostSkin)
    {
        _receivedHostName = hostName;
        _receivedHostSkin = hostSkin;

        Debug.Log($"Host Name: {hostName}, Host Skin: {hostSkin}");

        // Notify board manager about the opponent data (using received host name and local SkinIndex)
        boardManager.ReceiveOpponentData(_receivedHostName, SkinIndex);
    }
}
