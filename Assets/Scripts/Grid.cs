using UnityEngine;
using System.Collections.Generic;

public class Grid
{
    public int width;
    public int height;
    public float cellSize;
    public GridBlock[,] blocks;
    public HintBlock[] hintsRow;
    public HintBlock[] hintsCol;
    public int maxScore;
    public int sumOfPoints;

    // Constructor: now takes an additional 'level' parameter.
    public Grid(int level, float cellSize)
    {
        // Fixed grid size 5x5.
        this.width = 5;
        this.height = 5;
        this.cellSize = cellSize;
        blocks = new GridBlock[width, height];
        hintsRow = new HintBlock[width];
        hintsCol = new HintBlock[height];

        int totalCells = width * height; // 25 cells

        // Calculate bombCount based on level.
        int baseBombCount = 2;
        int bombCount = baseBombCount + level;
        bombCount = Mathf.Clamp(bombCount, 1, totalCells - 1); // Ensure at least one safe cell.

        // Safe count is the rest.
        int safeCount = totalCells - bombCount;

        // Calculate safeSum.
        // For example: safeSum = safeCount + level * 2.
        int safeSum = safeCount + level * 2;
        // Clamp safeSum to the valid range [safeCount, safeCount * 3].
        safeSum = Mathf.Clamp(safeSum, safeCount, safeCount * 3);


        Debug.Log($"Level {level} Grid: BombCount = {bombCount}, SafeCount = {safeCount}, SafeSum = {safeSum}");

        // Generate safe cell values.
        List<int> safeValues = new List<int>();
        // Start by initializing all safe cells to 1.
        for (int i = 0; i < safeCount; i++)
        {
            safeValues.Add(1);
        }
        // Distribute the extra points.
        int diff = safeSum - safeCount; // extra points to assign

        while (diff > 0)
        {
            int idx = UnityEngine.Random.Range(0, safeCount);
            if (safeValues[idx] < 3)
            {
                safeValues[idx]++;
                diff--;
            }
        }

        // Create bomb values (zeros).
        List<int> bombValues = new List<int>();
        for (int i = 0; i < bombCount; i++)
        {
            bombValues.Add(0);
        }

        // Combine safe and bomb values.
        List<int> values = new List<int>();
        values.AddRange(safeValues);
        values.AddRange(bombValues);

        // Ensure we have exactly totalCells values.
        if (values.Count != totalCells)
        {
            Debug.LogError("Mismatch in total cell count!");
            return;
        }

        // Shuffle the list (Fisher–Yates).
        Shuffle(values);

        // Assign values to blocks and calculate maxScore.
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
                    if(value != 1)
                    {
                        sumOfPoints +=value;
                    }
                }
                index++;
            }
        }
        maxScore = tempMaxScore;
        //Debug.Log("Max score for level " + level + ": " + maxScore);

        // Create HintBlocks for each row.
        for (int i = 0; i < width; i++)
        {
            int valueSum = 0;
            int mineCount = 0;
            for (int j = 0; j < height; j++)
            {
                int temp = blocks[i, j].GetBlockValue();
                valueSum += temp;
                if (temp == 0)
                {
                    mineCount++;
                }
            }
            hintsRow[i] = new HintBlock(valueSum, mineCount, i);
        }

        // Create HintBlocks for each column.
        for (int j = 0; j < height; j++)
        {
            int valueSum = 0;
            int mineCount = 0;
            for (int i = 0; i < width; i++)
            {
                int temp = blocks[i, j].GetBlockValue();
                valueSum += temp;
                if (temp == 0)
                {
                    mineCount++;
                }
            }
            hintsCol[j] = new HintBlock(valueSum, mineCount, j);
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
    public int GetSumOfPoints() { return sumOfPoints; }
    public GridBlock GetBlock(int x, int y) { return blocks[x, y]; }
    public GridBlock[,] GetBlocks() { return blocks; }
    public HintBlock[] GetHintRowArray() { return hintsRow; }
    public HintBlock[] GetHintColArray() { return hintsCol; }
    public HintBlock GetHintBlockFromRow(int x) { return hintsRow[x]; }
    public HintBlock GetHintBlockFromCol(int y) { return hintsCol[y]; }
}
