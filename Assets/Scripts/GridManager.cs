using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridManager : MonoBehaviour
{
    // Private grid instance from your plain C# Grid class.
    private Grid grid;

    private int score = 1;
    private int maxScore;

    public event Action OnScoreZero;
    public event Action<int> OnScoreUpdate;
    public event Action<int> OnScoreMax;

    private bool levelCompleted = false;

    // Optionally, you can set these via the Inspector.
    [SerializeField] private int rows = 5;
    [SerializeField] private int cols = 5;
    [SerializeField] private float cellSize = 1;

    [SerializeField] private Camera mainCamera;  // Assign your main camera in the Inspector
    [SerializeField] private LayerMask gridBlockLayer;  // Ensure your grid blocks are on this layer

    private int flagNumber;

    private UIButtons uiButtons;

    private void Awake()
    {
        // Create the grid using the provided parameters.
        grid = new Grid(rows, cols, cellSize);
        maxScore = grid.GetMaxScore();
        levelCompleted = false;
        flagNumber = 0;

    }

    private void Start()
    {
        // Find the UIButtons object and subscribe to the event
        uiButtons = GameObject.FindFirstObjectByType<UIButtons>();

        if (uiButtons != null)
        {
            uiButtons.OnFlagButtonPressed += OnFlagButtonPressed;
        }
        else
        {
            Debug.Log("Couldn't find UIButtons GameObject.");
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))  // Check for left mouse click
        {
            CheckForGridBlockClick();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            flagNumber = 0;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            flagNumber = 1;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            flagNumber = 2;
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            flagNumber = 3;
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            flagNumber = 4;
        }
    }

    private void CheckForGridBlockClick()
    {
        // Convert the mouse position (in screen coordinates) to a world point.
        Vector2 worldPoint = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        // Use OverlapPoint to detect if there's a collider at the world point.
        // The gridBlockLayer is a LayerMask that filters which layers to check.
        Collider2D hitCollider = Physics2D.OverlapPoint(worldPoint, gridBlockLayer);

        if (hitCollider != null)
        {
            // Get the BlockView script from the collider.
            // This script holds a reference to the GridBlock.
            BlockView blockView = hitCollider.GetComponent<BlockView>();
            if (blockView != null)
            {
                // Retrieve the GridBlock using the BlockView's getter method.
                GridBlock clickedBlock = blockView.GetGridBlock();

                if (clickedBlock == null)
                {
                    Debug.Log("Couldn't find clicked grid block object.");
                }
                else
                {   // If flag number is 0 the clicked grid Block gets be revealed
                    if (flagNumber == 0)
                    {
                        if ((!clickedBlock.GetIsBlockClicked()) && (!levelCompleted) && (score != 0))
                        {
                            // Signal the block that it was clicked to remove the cover and set blockIsClicked to true.
                            clickedBlock.SetBlockClicked();

                            score *= clickedBlock.GetBlockValue(); //Update score of this specific level

                            OnScoreUpdate?.Invoke(score);

                            Debug.Log("score: " + score);

                            if (score == 0)
                            {
                                // Notify the gameManager that the player lost
                                OnScoreZero?.Invoke();
                            }
                            else if (score == maxScore)
                            {
                                // Notify the gameManager that this level is completed
                                OnScoreMax?.Invoke(score);
                                levelCompleted = true;
                            }

                        }
                    }
                    // If flag number is 1,2 or 3 then the clicked grid Block gets flagged
                    else if ((flagNumber == 1) || (flagNumber == 2) || (flagNumber == 3) || (flagNumber == 4))
                    {
                        clickedBlock.SetBlockFlagged(flagNumber);
                    }
                }


            }
        }
    }


    // Public accessor to get the grid.
    public Grid GetGrid()
    {
        return grid;
    }

    private void OnFlagButtonPressed(int flagNumber)
    {
        this.flagNumber = flagNumber;
    }
}

/*
using UnityEngine;
using System.Collections.Generic;

public class Grid
{
    // Width and height of the grid
    private int width;
    private int height;

    private int level;

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
    public Grid(int width, int height, float cellSize, int level)
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

        // Generate numbers for the grid
        GenerateGrid(values, 1);

        // Shuffle the list using the Fisher–Yates algorithm.
        ShuffleGrid(values);

        // Assign the shuffled values to each cell in the grid.
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
                //Debug.Log($"Block at ({i},{j}) initialized with value: {blocks[i, j].GetBlockValue()}");
                index++;
            }
        }

        this.maxScore = tempMaxScore;

        //Debug.Log("max score:"+maxScore);

        // Calculate value and mine sum of each row and column and assign values to hintblocks
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
            //Debug.Log("ROW:"+i+"     ,"+"Value sum: "+valueSum+"  Mine Sum: "+mineSum);
            hintsRow[i] = new HintBlock(valueSum, mineSum, i);
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
            hintsCol[j] = new HintBlock(valueSum, mineSum, j);
        }

        this.level = level; 
    }

    private void GenerateGrid(List<int> values,int level)
    {
        // Randomly choose which distribution to use.
        // For a 50/50 chance between Distribution A and B:
        bool useDistributionA = Random.value < 0.5f;

        if (level == 1) { 
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

        }


        // Debug which distribution is chosen.
        Debug.Log("Using Distribution " + (useDistributionA ? "A" : "B"));
    }

    // Fisher–Yates shuffle algorithm to randomize the list.
    private void ShuffleGrid(List<int> list)
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

 */