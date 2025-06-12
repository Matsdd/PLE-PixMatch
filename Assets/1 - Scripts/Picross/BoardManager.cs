using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro; // Make sure this is included

public class BoardManager : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public GameObject game;
    public GameObject cross1;
    public GameObject cross2;
    public GameObject cross3;
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
    
    public enum DrawMode { Fill, Mark, Wrong}
    public DrawMode currentMode = DrawMode.Fill;
    
    private int wrongAttempts = 0;
    private const int maxWrongAttempts = 3;


    void Start()
    {
        GeneratePuzzle();
        SpawnGridWithClues();
    }

    void GeneratePuzzle()
    {
        solutionGrid = new bool[width, height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                solutionGrid[y, x] = Random.value < 0.4f;
            }
        }
    }

    bool[] GetRow(int y)
    {
        bool[] row = new bool[width];
        for (int x = 0; x < width; x++)
            row[x] = solutionGrid[y, x];
        return row;
    }

    bool[] GetColumn(int x)
    {
        bool[] col = new bool[height];
        for (int y = 0; y < height; y++)
            col[y] = solutionGrid[y, x];
        return col;
    }

    string GetClueLine(bool[] line)
    {
        string clue = "";
        int count = 0;
        foreach (bool cell in line)
        {
            if (cell) count++;
            else if (count > 0)
            {
                clue += count + " ";
                count = 0;
            }
        }
        if (count > 0) clue += count;
        return clue == "" ? "0" : clue.Trim();
    }

    void SpawnGridWithClues()
    {
        foreach (Transform child in gridParent) Destroy(child.gameObject);
        foreach (Transform child in rowCluesParent) Destroy(child.gameObject);
        foreach (Transform child in colCluesParent) Destroy(child.gameObject);

        tileObjects = new GameObject[width, height];
        
        for (int y = 0; y < height; y++)
        {
            string clue = GetClueLine(GetRow(y));
            GameObject clueObj = Instantiate(rowClueTextPrefab, rowCluesParent);
            clueObj.GetComponent<TMP_Text>().text = clue;
        }

        for (int x = 0; x < width; x++)
        {
            string clue = GetClueLine(GetColumn(x));
            GameObject clueObj = Instantiate(colClueTextPrefab, colCluesParent);
            clueObj.GetComponent<TMP_Text>().text = clue;
        }
        
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
            cross1.;
        }
        if (wrongAttempts >= 2)
        {
            cross2.SetActive(true);
        }

        if (wrongAttempts >= maxWrongAttempts)
        {
            Lose();
            cross3.SetActive(true);
        }
    }

    public void Lose()
    {
        game.SetActive(false);
        loss.SetActive(true);
    }

    public bool IsCorrect(int x, int y, bool filled)
    {
        return solutionGrid[x, y] == filled;
    }
}
