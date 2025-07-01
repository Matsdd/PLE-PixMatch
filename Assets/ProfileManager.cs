using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfileManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField nameInputField;
    public Button saveButton;
    public Button nextSkinButton;

    [Header("Skin Settings")]
    public SkinnedMeshRenderer playerRenderer;
    public Material[] skinMaterials;

    private int currentSkinIndex = 0;

    private const string PlayerNameKey = "PlayerName";
    private const string PlayerSkinKey = "PlayerSkin";

    private void Start()
    {
        LoadProfile();

        saveButton.onClick.AddListener(SaveProfile);
        nextSkinButton.onClick.AddListener(SwitchToNextSkin);
    }

    void LoadProfile()
    {
        if (PlayerPrefs.HasKey(PlayerNameKey))
        {
            nameInputField.text = PlayerPrefs.GetString(PlayerNameKey);
        }

        currentSkinIndex = PlayerPrefs.GetInt(PlayerSkinKey, 0);
        ApplyMaterial(currentSkinIndex);
    }

    void SaveProfile()
    {
        string playerName = nameInputField.text;
        PlayerPrefs.SetString(PlayerNameKey, playerName);
        PlayerPrefs.SetInt(PlayerSkinKey, currentSkinIndex);
        PlayerPrefs.Save();

        Debug.Log($"Saved Name: {playerName}, SkinIndex: {currentSkinIndex}");
    }

    void SwitchToNextSkin()
    {
        currentSkinIndex = (currentSkinIndex + 1) % skinMaterials.Length;
        ApplyMaterial(currentSkinIndex);
    }

    void ApplyMaterial(int index)
    {
        if (playerRenderer != null && skinMaterials.Length > 0)
        {
            playerRenderer.material = skinMaterials[index];
        }
    }
}