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

    public string _receivedHostName;
    public int _receivedHostSkin;

    public string ReceivedHostName => _receivedHostName;
    public int ReceivedHostSkin => _receivedHostSkin;

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            string name = PlayerPrefs.GetString("PlayerName", "Player");
            int skin = PlayerPrefs.GetInt("PlayerSkin", 0);

            RPC_SendPlayerInfoToHost(name, skin);
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_SendPlayerInfoToHost(string playerName, int skinIndex)
    {
        PlayerName = playerName;
        SkinIndex = skinIndex;

        string hostName = PlayerPrefs.GetString("PlayerName", "Host");
        int hostSkin = PlayerPrefs.GetInt("PlayerSkin", 0);
        RPC_SendHostInfoToClient(hostName, hostSkin);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority)]
    private void RPC_SendHostInfoToClient(string hostName, int hostSkin)
    {
        _receivedHostName = hostName;
        _receivedHostSkin = hostSkin;

        Debug.Log(hostName);

        boardManager.ReceiveOpponentData(_receivedHostName, SkinIndex);
    }
}