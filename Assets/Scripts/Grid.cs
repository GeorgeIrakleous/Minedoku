using UnityEngine;
using System.Collections.Generic;

public class Grid
{
    // Grid dimensions and cell spacing.
    private int width;
    private int height;
    private float cellSize;

    // Maximum points the player can score in this grid.
    private int maxScore;

    // 2D array with all grid blocks.
    private GridBlock[,] blocks;

    // 1D arrays for the hint blocks (one per row and column).
    private HintBlock[] hintsRow;
    private HintBlock[] hintsCol;

    // Constructor: now takes an additional 'level' parameter.
    public Grid(int width, int height, float cellSize, int level)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        // Create the grid array.
        blocks = new GridBlock[width, height];

        // Create the hint arrays.
        hintsRow = new HintBlock[width];
        hintsCol = new HintBlock[height];

        // Determine the total number of cells.
        int totalCells = width * height;

        // Base frequencies for level 1.
        int baseZeros = 6;   // Mines
        int baseOnes = 15;
        int baseTwos = 3;
        int baseThrees = 1;

        // Adjust frequencies based on level.
        // For higher levels, increase mines and higher numbers.
        // These formulas are arbitrary and can be balanced later.
        int extraMines = level * 2; // Increase mines by a factor of level.
        int extraTwos = level;      // Increase twos a bit.
        int extraThrees = level / 2; // Increase threes gradually.

        int zerosCount = baseZeros + extraMines;
        int onesCount = baseOnes;   // We'll adjust ones to fill the grid.
        int twosCount = baseTwos + extraTwos;
        int threesCount = baseThrees + extraThrees;

        // Calculate the sum of frequencies.
        int frequencySum = zerosCount + onesCount + twosCount + threesCount;

        // Adjust onesCount so the total matches the grid size.
        if (frequencySum < totalCells)
        {
            onesCount += (totalCells - frequencySum);
        }
        else if (frequencySum > totalCells)
        {
            int diff = frequencySum - totalCells;
            onesCount = Mathf.Max(onesCount - diff, 0);
        }

        // Debug: Print chosen frequencies.
        Debug.Log($"Level {level} Grid Frequencies: Zeros={zerosCount}, Ones={onesCount}, Twos={twosCount}, Threes={threesCount}");

        // Create a list with these values.
        List<int> values = new List<int>();
        for (int i = 0; i < zerosCount; i++) { values.Add(0); }
        for (int i = 0; i < onesCount; i++) { values.Add(1); }
        for (int i = 0; i < twosCount; i++) { values.Add(2); }
        for (int i = 0; i < threesCount; i++) { values.Add(3); }

        // Shuffle the list using the Fisher–Yates algorithm.
        Shuffle(values);

        // Assign the shuffled values to each cell in the grid and calculate maxScore.
        int index = 0;
        int tempMaxScore = 1;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int value = values[index];
                blocks[i, j] = new GridBlock(value, i, j);
                if (value != 0)
                {
                    tempMaxScore *= value;
                }
                index++;
            }
        }
        maxScore = tempMaxScore;
        Debug.Log("Max score for level " + level + ": " + maxScore);

        // Create HintBlocks for each row.
        for (int i = 0; i < width; i++)
        {
            int valueSum = 0;
            int mineSum = 0;
            for (int j = 0; j < height; j++)
            {
                int temp = blocks[i, j].GetBlockValue();
                valueSum += temp;
                if (temp == 0)
                {
                    mineSum++;
                }
            }
            hintsRow[i] = new HintBlock(valueSum, mineSum, i);
        }

        // Create HintBlocks for each column.
        for (int j = 0; j < height; j++)
        {
            int valueSum = 0;
            int mineSum = 0;
            for (int i = 0; i < width; i++)
            {
                int temp = blocks[i, j].GetBlockValue();
                valueSum += temp;
                if (temp == 0)
                {
                    mineSum++;
                }
            }
            hintsCol[j] = new HintBlock(valueSum, mineSum, j);
        }
    }

    // Fisher–Yates shuffle algorithm.
    private void Shuffle(List<int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    // Public accessors.
    public int GetWidth() { return width; }
    public int GetHeight() { return height; }
    public float GetCellSize() { return cellSize; }
    public int GetMaxScore() { return maxScore; }
    public GridBlock GetBlock(int x, int y) { return blocks[x, y]; }
    public GridBlock[,] GetBlocks() { return blocks; }
    public HintBlock[] GetHintRowArray() { return hintsRow; }
    public HintBlock[] GetHintColArray() { return hintsCol; }
    public HintBlock GetHintBlockFromRow(int x) { return hintsRow[x]; }
    public HintBlock GetHintBlockFromCol(int y) { return hintsCol[y]; }
}
