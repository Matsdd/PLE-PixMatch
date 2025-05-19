using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PicrossBoard : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public GameObject tilePrefab;
    public Transform gridParent;
    public Transform rowCluesParent;
    public Transform colCluesParent;

    public bool[,] solutionGrid;
    public GameObject[,] tileObjects;

    void Start()
    {
        GeneratePuzzle();
        CalculateClues();
        SpawnGrid();
    }

    void GeneratePuzzle()
    {
        solutionGrid = new bool[width, height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                solutionGrid[x, y] = Random.value < 0.4f;
            }
        }
    }

    void CalculateClues()
    {
        // Example placeholder; implement actual clue UI rendering separately.
        for (int y = 0; y < height; y++)
        {
            string clue = GetClueLine(GetRow(y));
            Debug.Log($"Row {y}: {clue}");
        }
        for (int x = 0; x < width; x++)
        {
            string clue = GetClueLine(GetColumn(x));
            Debug.Log($"Col {x}: {clue}");
        }
    }

    bool[] GetRow(int y)
    {
        bool[] row = new bool[width];
        for (int x = 0; x < width; x++)
            row[x] = solutionGrid[x, y];
        return row;
    }

    bool[] GetColumn(int x)
    {
        bool[] col = new bool[height];
        for (int y = 0; y < height; y++)
            col[y] = solutionGrid[x, y];
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

    void SpawnGrid()
    {
        tileObjects = new GameObject[width, height];
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

    public bool IsCorrect(int x, int y, bool filled)
    {
        return solutionGrid[x, y] == filled;
    }
}