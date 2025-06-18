using UnityEngine;

public class HomeScreenSkinSetter : MonoBehaviour
{
    public SkinnedMeshRenderer playerSkinnedMeshRenderer;
    public Material[] playerSkins;

    private const string PlayerSkinKey = "PlayerSkinKey";  // Use the same key as your Profile script

    void Start()
    {
        if (playerSkins == null || playerSkins.Length == 0)
        {
            Debug.LogWarning("Player skins array is empty or not assigned.");
            return;
        }

        if (playerSkinnedMeshRenderer == null)
        {
            Debug.LogError("Player SkinnedMeshRenderer is not assigned!");
            return;
        }

        int savedSkinIndex = 0;

        if (PlayerPrefs.HasKey(PlayerSkinKey))
        {
            savedSkinIndex = PlayerPrefs.GetInt(PlayerSkinKey, 0);
            savedSkinIndex = Mathf.Clamp(savedSkinIndex, 0, playerSkins.Length - 1);
        }
        else
        {
            // Set default skin index 0 if none saved yet
            PlayerPrefs.SetInt(PlayerSkinKey, 0);
            PlayerPrefs.Save();
        }

        ApplyMaterial(savedSkinIndex);
    }

    void ApplyMaterial(int index)
    {
        playerSkinnedMeshRenderer.material = playerSkins[index];
    }
}