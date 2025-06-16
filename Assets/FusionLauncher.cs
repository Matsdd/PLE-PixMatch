using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class NetworkStarter : MonoBehaviour
{
    public NetworkRunner runnerPrefab;

    async void Start()
    {
        var runner = Instantiate(runnerPrefab);
        runner.ProvideInput = true;
        
        var sceneManager = runner.gameObject.AddComponent<NetworkSceneManagerDefault>();

        await runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Host,
            SessionName = "ClickRoom",
            SceneManager = sceneManager
        });
    }
}