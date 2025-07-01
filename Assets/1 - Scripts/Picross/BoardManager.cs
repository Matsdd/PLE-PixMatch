using System;
using UnityEngine;
using TMPro;

public class BoardManager : MonoBehaviour
{
    [Header("Characters")]
    public GameObject OpponentCharacter;
    public GameObject PlayerCharacter;

    [Header("Text")]
    public TMP_Text ModeText;

    [Header("Characters Mesh + Skins")]
    public SkinnedMeshRenderer playerCharacter;  // local player mesh
    public SkinnedMeshRenderer opponentCharacter; // opponent mesh
    public Material[] playerSkins;

    [Header("Grid Settings")]
    public int width = 10;
    public int height = 10;
    public GameObject game;
    public GameObject crossPrefab;
    public Transform crossParent;
    public GameObject loss;
    public GameObject win;
    public GameObject tilePrefab;
    public Transform gridParent;
    public Transform rowCluesParent;
    public Transform colCluesParent;
    public GameObject rowClueTextPrefab;
    public GameObject colClueTextPrefab;
    public bool[,] solutionGrid;
    public GameObject[,] tileObjects;

    public enum DrawMode { Fill, Mark, Wrong }
    public DrawMode currentMode = DrawMode.Fill;

    private int wrongAttempts = 0;
    private const int maxWrongAttempts = 3;

    private string _localName;
    private int _localSkinIndex;

    private string _opponentName = "Waiting...";
    private int _opponentSkinIndex = 0;

    void Start()
    {
        GeneratePuzzle();
        SpawnGridWithClues();
        SetupModeUI();
        ApplySavedSkins();
    }

    void SetupModeUI()
    {
        if (ModeSelector.IsQuickplay)
        {
            OpponentCharacter.SetActive(false);
            PlayerCharacter.SetActive(false);

            int streak = PlayerPrefs.GetInt("CurrentStreak", 0);
            ModeText.text = $"Streak: {streak}";
        }
        else
        {
            OpponentCharacter.SetActive(true);
            PlayerCharacter.SetActive(true);

            string myName = PlayerPrefs.GetString("PlayerName", "You");
            _localName = myName;

            ModeText.text = $"{_localName} vs {_opponentName}";
        }
    }

    void ApplySavedSkins()
    {
        int playerSkinIndex = PlayerPrefs.GetInt("PlayerSkin", 0);
        _localSkinIndex = playerSkinIndex;
        ApplyMaterial(playerCharacter, _localSkinIndex);
    }

    void ApplyMaterial(SkinnedMeshRenderer renderer, int index)
    {
        if (renderer != null && playerSkins.Length > 0)
        {
            index = Mathf.Clamp(index, 0, playerSkins.Length - 1);
            renderer.material = playerSkins[index];
        }
    }

    public void ApplyPlayerInfo(bool isLocal, string name, int skinIndex)
    {
        if (isLocal)
        {
            _localName = name;
            _localSkinIndex = skinIndex;

            Debug.Log($"Set local player: {_localName} skin: {_localSkinIndex}");
            ApplyMaterial(playerCharacter, _localSkinIndex);
        }
        else
        {
            _opponentName = name;
            _opponentSkinIndex = skinIndex;

            Debug.Log($"Set opponent player: {_opponentName} skin: {_opponentSkinIndex}");
            ApplyMaterial(opponentCharacter, _opponentSkinIndex);
        }

        UpdateModeText();
    }

    private void UpdateModeText()
    {
        if (!ModeSelector.IsQuickplay && !string.IsNullOrEmpty(_localName) && !string.IsNullOrEmpty(_opponentName))
        {
            ModeText.text = $"{_localName} vs {_opponentName}";
        }
    }
    
        void GeneratePuzzle()
    {
        solutionGrid = new bool[width, height];
        bool valid;

        do
        {
            valid = true;

            // Generate a random board
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    solutionGrid[y, x] = UnityEngine.Random.value < 0.4f;

            // Check row runs
            for (int y = 0; y < height; y++)
            {
                if (CountRuns(GetRow(y)) > 4)
                {
                    valid = false;
                    break;
                }
            }

            // Check column runs
            for (int x = 0; x < width; x++)
            {
                if (CountRuns(GetColumn(x)) > 4)
                {
                    valid = false;
                    break;
                }
            }

        } while (!valid);
    }

    int CountRuns(bool[] line)
    {
        int runs = 0;
        bool inRun = false;

        foreach (bool cell in line)
        {
            if (cell)
            {
                if (!inRun)
                {
                    runs++;
                    inRun = true;
                }
            }
            else
            {
                inRun = false;
            }
        }

        return runs;
    }

    bool[] GetRow(int y)
    {
        bool[] row = new bool[width];
        for (int x = 0; x < width; x++)
            row[x] = solutionGrid[y, x]; // Correct: Y is row, X is column
        return row;
    }

    bool[] GetColumn(int x)
    {
        bool[] col = new bool[height];
        for (int y = 0; y < height; y++)
            col[y] = solutionGrid[y, x]; // Correct: Y is row, X is column
        return col;
    }


    string GetClueLine(bool[] line)
    {
        string clue = "";
        int count = 0;
        foreach (bool cell in line)
        {
            if (cell)
            {
                count++;
            }
            else if (count > 0)
            {
                clue += count + " ";
                count = 0;
            }
        }
        if (count > 0)
            clue += count;
        return clue == "" ? "0" : clue.Trim();
    }

    void SpawnGridWithClues()
    {
        foreach (Transform child in gridParent) Destroy(child.gameObject);
        foreach (Transform child in rowCluesParent) Destroy(child.gameObject);
        foreach (Transform child in colCluesParent) Destroy(child.gameObject);

        tileObjects = new GameObject[width, height];

        // Spawn row clues
        for (int y = 0; y < height; y++)
        {
            string clue = GetClueLine(GetRow(y));
            GameObject clueObj = Instantiate(rowClueTextPrefab, rowCluesParent);
            clueObj.GetComponent<TMP_Text>().text = clue;
        }

        // Spawn column clues
        for (int x = 0; x < width; x++)
        {
            string clue = GetClueLine(GetColumn(x));
            GameObject clueObj = Instantiate(colClueTextPrefab, colCluesParent);
            clueObj.GetComponent<TMP_Text>().text = clue;
        }

        // Spawn tile grid
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject tile = Instantiate(tilePrefab, gridParent);
                tile.GetComponent<PicrossTile>().Init(this, x, y);
                tileObjects[x, y] = tile;
            }
        }
    }

    
    public void RegisterWrongAttempt()
    {
        wrongAttempts++;
        if (wrongAttempts >= 1)
        {
            GameObject cross = Instantiate(crossPrefab, crossParent);
        }

        if (wrongAttempts >= maxWrongAttempts)
        {
            Lose();
        }
    }

    public void CheckForWin()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                bool shouldBeFilled = solutionGrid[x, y];
                bool isFilled = tileObjects[x, y].GetComponent<PicrossTile>().IsFilled();

                // If a tile that should be filled isn't, or a tile that shouldn't be is, we return early.
                if (isFilled != shouldBeFilled)
                    return;
            }
        }
        
        Win();
    }
    
    public void Lose()
    {
        PlayerPrefs.SetInt("CurrentStreak", 0);
        PlayerPrefs.Save();
        
        game.SetActive(false);
        loss.SetActive(true);
    }
    
    public void Win()
    {
        int currentStreak = PlayerPrefs.GetInt("CurrentStreak", 0);
        currentStreak += 1;
        PlayerPrefs.SetInt("CurrentStreak", currentStreak);
        PlayerPrefs.Save();

        game.SetActive(false);
        win.SetActive(true);
    }

    public bool IsCorrect(int x, int y, bool filled)
    {
        return solutionGrid[x, y] == filled;
    }
}