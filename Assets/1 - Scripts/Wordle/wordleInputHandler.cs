using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WordleInputHandler : MonoBehaviour
{
    public MakeWordle wordleGrid;
    public CheckWordle wordChecker;

    private int currentRow = 0;
    private int currentCol = 0;
    private string currentInput = "";

    void Update()
    {
        HandleInput();
    }
    
    void HandleInput()
    {
        if (currentRow >= MakeWordle.rows) return;

        foreach (char c in Input.inputString)
        {
            if (char.IsLetter(c) && currentCol < CheckWordle.columns)
            {
                char upperC = char.ToUpper(c);  
                wordleGrid.grid[currentRow, currentCol].GetComponentInChildren<TextMeshProUGUI>().text =
                    upperC.ToString();
                currentInput += upperC;
                currentCol++;
            }
            else if (c == '\b' && currentCol > 0)
            {
                currentCol--;
                wordleGrid.grid[currentRow, currentCol].GetComponentInChildren<TextMeshProUGUI>().text = "";
                currentInput = currentInput.Substring(0, currentInput.Length - 1);
            }
        }
    }
}


