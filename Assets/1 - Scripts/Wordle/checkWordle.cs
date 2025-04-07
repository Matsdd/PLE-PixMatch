using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckWordle : MonoBehaviour
{
    public string targetWord = "WEIRD";
    public const int columns = 5;
    public GameObject correctTilePrefab;
    public GameObject placeTilePrefab;
    public GameObject wrongTilePrefab;

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
        
        for (int i = 0; i < columns; i++)
        {
            GameObject letterTile = grid[currentRow, i];
            TextMeshProUGUI letterText = letterTile.GetComponentInChildren<TextMeshProUGUI>();
            char guessedLetter = letterText.text[0];

            if (guessedLetter == targetWord[i])
            {
                ReplaceTile(letterTile, correctTilePrefab);
                targetLetterCounts[guessedLetter]--;
            }
        }
        
        for (int i = 0; i < columns; i++)
        {
            GameObject letterTile = grid[currentRow, i];
            TextMeshProUGUI letterText = letterTile.GetComponentInChildren<TextMeshProUGUI>();
            char guessedLetter = letterText.text[0];

            if (letterTile.transform.childCount == 1)
            {
                if (targetLetterCounts.ContainsKey(guessedLetter) && targetLetterCounts[guessedLetter] > 0)
                {
                    ReplaceTile(letterTile, placeTilePrefab);
                    targetLetterCounts[guessedLetter]--;
                }
                else
                {
                    ReplaceTile(letterTile, wrongTilePrefab);
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
