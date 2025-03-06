using UnityEngine;
using System.Collections.Generic;

public class Grid
{
    // Width and height of the grid
    private int width;
    private int height;

    // Maximum points that the player can score in this grid
    private int maxScore;

    // Space between each block in the grid
    private float cellSize;

    // 2D Array with all the blocks of the grid
    private GridBlock[,] blocks; 

    // 1D Arrays of the hint blocks of the grid
    private HintBlock[] hintsRow;
    private HintBlock[] hintsCol;

    // Constructor: initializes the grid with the given dimensions and cell size.
    public Grid(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        blocks = new GridBlock[width, height];

        // Create a list with the desired frequency distribution.
        // For a grid with (width * height) cells.
        List<int> values = new List<int>();

        // Create empty arrays of hintBlocks 
        hintsRow = new HintBlock[width];
        hintsCol = new HintBlock[height];

        // Randomly choose which distribution to use.
        // For a 50/50 chance between Distribution A and B:
        bool useDistributionA = Random.value < 0.5f;

        if (useDistributionA)
        {
            // Distribution A: 6 zeros, 15 ones, 3 twos, 1 three.
            for (int i = 0; i < 6; i++) { values.Add(0); }
            for (int i = 0; i < 15; i++) { values.Add(1); }
            for (int i = 0; i < 3; i++) { values.Add(2); }
            values.Add(3);
        }
        else
        {
            // Distribution B: 6 zeros, 15 ones, 2 twos, 2 threes.
            for (int i = 0; i < 6; i++) { values.Add(0); }
            for (int i = 0; i < 15; i++) { values.Add(1); }
            for (int i = 0; i < 2; i++) { values.Add(2); }
            for (int i = 0; i < 2; i++) { values.Add(3); }
        }

        // Debug which distribution is chosen.
        Debug.Log("Using Distribution " + (useDistributionA ? "A" : "B"));

        // Shuffle the list using the Fisher–Yates algorithm.
        Shuffle(values);

        // Assign the shuffled values to each cell in the grid.
        int index = 0;
        int tempMaxScore = 1;

        for (int i = 0; i < width; i++)
        {
   
            for (int j = 0; j < height; j++)
            {
                int value = values[index];
                blocks[i, j] = new GridBlock(value,i,j);
                if (value!=0)
                {
                    tempMaxScore *= value;
                }
                //Debug.Log($"Block at ({i},{j}) initialized with value: {blocks[i, j].GetBlockValue()}");
                index++;
            }
        }

        this.maxScore = tempMaxScore;

        //Debug.Log("max score:"+maxScore);

        // Calculate value and mine sum of each row and column and assign values to hintblocks
        for (int i = 0; i < width; i++)
        {
            int valueSum=0;
            int mineSum = 0;
            for (int j = 0; j < height; j++)
            {
                int temp = blocks[i, j].GetBlockValue();
                valueSum += temp;
                if(temp == 0)
                {
                    mineSum++;
                }
            }
            //Debug.Log("ROW:"+i+"     ,"+"Value sum: "+valueSum+"  Mine Sum: "+mineSum);
            hintsRow[i]= new HintBlock(valueSum, mineSum,i);
        }

        for (int j = 0; j < width; j++)
        {
            int valueSum = 0;
            int mineSum = 0;
            for (int i = 0; i < height; i++)
            {
                int temp = blocks[i, j].GetBlockValue();
                valueSum += temp;
                if (temp == 0)
                {
                    mineSum++;
                }
            }
            //Debug.Log("COL:" + j + "     ," + "Value sum: " + valueSum + "  Mine Sum: " + mineSum);
            hintsCol[j]= new HintBlock (valueSum, mineSum, j);
        }
    }

    // Fisher–Yates shuffle algorithm to randomize the list.
    private void Shuffle(List<int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            // Random.Range for ints is min inclusive and max exclusive, so use i + 1.
            int j = Random.Range(0, i + 1);
            int temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    // Public accessors.
    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public float GetCellSize()
    {
        return cellSize;
    }

    public int GetMaxScore()
    {
        return maxScore;
    }

    // Retrieve a block at a specific position.
    public GridBlock GetBlock(int x, int y)
    {
        return blocks[x, y];
    }

    // Optionally, expose the entire blocks array.
    public GridBlock[,] GetBlocks()
    {
        return blocks;
    }

    public HintBlock[] GetHintRowArray()
    {
        return hintsRow;
    }

    public HintBlock[] GetHintColArray()
    {
        return hintsCol;
    }

    public HintBlock GetHintBlockFromRow(int x)
    {
        return hintsRow[x];
    }

    public HintBlock GetHintBlockFromCol(int y)
    {
        return hintsCol[y];
    }
}
