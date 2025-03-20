using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckWordle : MonoBehaviour
{
    public string targetWord = "WEIRD";
    public const int columns = 5;
    public GameObject correctTilePrefab; // Green tile
    public GameObject placeTilePrefab; // Yellow tile
    public GameObject wrongTilePrefab; // Gray tile

    public void CheckWord(GameObject[,] grid, int currentRow)
    {
        if (grid == null || currentRow >= grid.GetLength(0)) return;

        Dictionary<char, int> targetLetterCounts = new Dictionary<char, int>();

        foreach (char c in targetWord)
        {
            if (targetLetterCounts.ContainsKey(c))
                targetLetterCounts[c]++;
            else
                targetLetterCounts[c] = 1;
        }

        // First pass: Check for exact matches (Green)
        for (int i = 0; i < columns; i++)
        {
            GameObject letterTile = grid[currentRow, i];
            TextMeshProUGUI letterText = letterTile.GetComponentInChildren<TextMeshProUGUI>();
            char guessedLetter = letterText.text[0];

            if (guessedLetter == targetWord[i])
            {
                ReplaceTile(letterTile, correctTilePrefab); // Correct letter & position
                targetLetterCounts[guessedLetter]--; // Reduce count
            }
        }

        // Second pass: Check for misplaced letters (Yellow) or incorrect (Gray)
        for (int i = 0; i < columns; i++)
        {
            GameObject letterTile = grid[currentRow, i];
            TextMeshProUGUI letterText = letterTile.GetComponentInChildren<TextMeshProUGUI>();
            char guessedLetter = letterText.text[0];

            if (letterTile.transform.childCount == 1) // Not already set to correctTilePrefab
            {
                if (targetLetterCounts.ContainsKey(guessedLetter) && targetLetterCounts[guessedLetter] > 0)
                {
                    ReplaceTile(letterTile, placeTilePrefab); // Correct letter, wrong position
                    targetLetterCounts[guessedLetter]--; // Reduce count
                }
                else
                {
                    ReplaceTile(letterTile, wrongTilePrefab); // Incorrect letter
                }
            }
        }
    }

    private void ReplaceTile(GameObject oldTile, GameObject newTilePrefab)
    {
        Vector3 position = oldTile.transform.position;
        Transform parent = oldTile.transform.parent;
        Destroy(oldTile);

        GameObject newTile = Instantiate(newTilePrefab, position, Quaternion.identity, parent);
        newTile.GetComponentInChildren<TextMeshProUGUI>().text = oldTile.GetComponentInChildren<TextMeshProUGUI>().text;
    }
}
