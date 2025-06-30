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
        Debug.Log($"[NetworkPlayer] Spawned. HasInputAuthority: {Object.HasInputAuthority}, HasStateAuthority: {Object.HasStateAuthority}");

        if (Object.HasInputAuthority)
        {
            string name = PlayerPrefs.GetString("PlayerName", "Player");
            int skin = PlayerPrefs.GetInt("PlayerSkin", 0);

            Debug.Log($"[NetworkPlayer] I have Input Authority, sending my info to host: {name}, skin: {skin}");

            RPC_SendPlayerInfoToHost(name, skin);
        }

        if (Object.HasStateAuthority)
        {
            string hostName = PlayerPrefs.GetString("PlayerName", "Host");
            int hostSkin = PlayerPrefs.GetInt("PlayerSkin", 0);

            Debug.Log($"[NetworkPlayer] I am the Host. Setting my PlayerName = {hostName}, SkinIndex = {hostSkin}");

            PlayerName = hostName;
            SkinIndex = hostSkin;
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_SendPlayerInfoToHost(string playerName, int skinIndex)
    {
        Debug.Log($"[NetworkPlayer] RPC_SendPlayerInfoToHost called. Received: playerName = {playerName}, skinIndex = {skinIndex}");

        PlayerName = playerName;
        SkinIndex = skinIndex;

        Debug.Log($"[NetworkPlayer] PlayerName now set to: {PlayerName}, SkinIndex now set to: {SkinIndex}");

        string hostName = PlayerPrefs.GetString("PlayerName", "Host");
        int hostSkin = PlayerPrefs.GetInt("PlayerSkin", 0);

        Debug.Log($"[NetworkPlayer] Sending host info back to client. HostName: {hostName}, HostSkin: {hostSkin}");

        RPC_SendHostInfoToClient(hostName, hostSkin);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority)]
    private void RPC_SendHostInfoToClient(string hostName, int hostSkin)
    {
        Debug.Log($"[NetworkPlayer] RPC_SendHostInfoToClient called. Received: hostName = {hostName}, hostSkin = {hostSkin}");

        _receivedHostName = hostName;
        _receivedHostSkin = hostSkin;

        Debug.Log($"[NetworkPlayer] Host info saved locally: _receivedHostName = {_receivedHostName}, _receivedHostSkin = {_receivedHostSkin}");

        if (boardManager == null)
        {
            Debug.LogError("[NetworkPlayer] BoardManager is NULL! Did you assign it?");
        }
        else
        {
            Debug.Log("[NetworkPlayer] Sending opponent data to BoardManager...");
            boardManager.ReceiveOpponentData(_receivedHostName, SkinIndex);
        }
    }
}
