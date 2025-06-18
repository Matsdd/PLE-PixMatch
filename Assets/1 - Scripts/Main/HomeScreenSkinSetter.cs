using System.Runtime.CompilerServices;
using UnityEngine;

public class HomeScreenSkinSetter : MonoBehaviour
{
    public SkinnedMeshRenderer playerCharacterRenderer;
    public Material[] skinMaterials;

    void Start()
    {
        int mySkinIndex = PlayerPrefs.GetInt("PlayerSkin", 0);
        ApplyMaterial(playerCharacterRenderer, mySkinIndex);
    }

    void ApplyMaterial(SkinnedMeshRenderer renderer, int index)
    {
        if (renderer != null && skinMaterials.Length > 0)
        {
            index = Mathf.Clamp(index, 0, skinMaterials.Length - 1);
            renderer.material = skinMaterials[index];
        }
    }

}