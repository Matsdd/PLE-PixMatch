using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MakeWordle : MonoBehaviour
{
    public GameObject letterTilePrefab;
    public Transform gridParent;
    public CheckWordle wordChecker;

    public const int rows = 6;
    public GameObject[,] grid = new GameObject[rows, CheckWordle.columns];

    void Start()
    {
        GenerateWordle();
    }

    void GenerateWordle()
    {
        float spacing = 3f;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < CheckWordle.columns; col++)
            {
                GameObject tile = Instantiate(letterTilePrefab, gridParent);
                tile.GetComponent<RectTransform>().anchoredPosition = new Vector2(col * spacing, -row * spacing);

                grid[row, col] = tile;
            }
        }
    }
}
