using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void ChangeTo(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}