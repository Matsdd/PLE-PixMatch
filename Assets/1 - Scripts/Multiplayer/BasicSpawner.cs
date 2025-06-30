using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    public NetworkObject playerPrefab;

    private Dictionary<PlayerRef, NetworkObject> _spawnedPlayers = new();

    private NetworkRunner _runner;

    private void Update()
    {
        if (_spawnedPlayers.Count >= 2)
        {
            TrySetupPlayers(_runner);
        }
    }

    
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            Vector3 spawnPos = new Vector3(player.RawEncoded % 5, 0, 0);
            NetworkObject playerObj = runner.Spawn(playerPrefab, spawnPos, Quaternion.identity, player);

            _spawnedPlayers[player] = playerObj;

            TrySetupPlayers(runner);
        }
    }

    private void TrySetupPlayers(NetworkRunner runner)
    {
        if (_spawnedPlayers.Count < 2) return;

        NetworkPlayer localPlayer = null;
        NetworkPlayer opponentPlayer = null;

        foreach (var kvp in _spawnedPlayers)
        {
            PlayerRef playerRef = kvp.Key;
            NetworkPlayer netPlayer = kvp.Value.GetComponent<NetworkPlayer>();

            // 🚩 If names aren't set yet, wait for next call
            if (string.IsNullOrEmpty(netPlayer.PlayerName.ToString()))
            {
                Debug.Log($"Player {playerRef} has no name yet, waiting...");
                return;
            }

            if (playerRef == runner.LocalPlayer)
            {
                localPlayer = netPlayer;
                Debug.Log("I am: " + netPlayer.PlayerName.ToString());
            }
            else
            {
                opponentPlayer = netPlayer;
                Debug.Log("Opponent is: " + netPlayer.PlayerName.ToString());
            }
        }

        if (localPlayer != null && opponentPlayer != null)
        {
            BoardManager board = FindObjectOfType<BoardManager>();
            if (board != null)
            {
                Debug.Log("Sending opponent data to BoardManager: " + opponentPlayer.PlayerName.ToString());
                board.SetOpponent(opponentPlayer.PlayerName.ToString(), opponentPlayer.SkinIndex);
            }
        }
    }


    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) => Debug.Log("Connected to server");
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }

    private async void StartGame(GameMode mode)
    {
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);

        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }

    private void OnGUI()
    {
        if (_runner == null)
        {
            if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
            {
                StartGame(GameMode.Host);
            }
            if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
            {
                StartGame(GameMode.Client);
            }
        }
    }
}
