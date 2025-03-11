using UnityEngine;
using TMPro;
using static UnityEngine.Rendering.DebugUI.Table;
using System;

public class GridVisualManager : MonoBehaviour
{
    // Reference to the GameObject that contains the GridManager component.
    public GameObject gridManagerGameObject;

    // Prefab for individual block visuals.
    public GameObject blockPrefab;

    // Parent transform for organizing instantiated blocks.
    public Transform gridParent;

    public GameObject hintPrefab;

    private GridManager gridManager;

    private void Start()
    {
        // Get the GridManager component from the assigned GameObject.
        gridManager = gridManagerGameObject.GetComponent<GridManager>();

        if (gridManager == null)
        {
            Debug.LogError("GridManager component not found on the assigned GameObject!");
            return;
        }

        gridManager.OnDisplayBoard += DisplayBoard;

        // Retrieve the grid from the GridManager.
        Grid grid = gridManager.GetGrid();

        // Use the grid's exposed properties for rows and columns.
        int rows = grid.GetHeight();
        int cols = grid.GetWidth();

        // Debug.Log($"GridVisualManager: Grid dimensions: {rows} x {cols}");

        // Retrieve the hints from the GridManager.
        HintBlock[] hintsRowArray = grid.GetHintRowArray();
        HintBlock[] hintsColArray = grid.GetHintColArray();

        DisplayHints(grid,rows,cols);

        // Now instantiate visual blocks based on grid data.
        DisplayGrid(grid, rows, cols);
    }

    // This function displays the entire board ( both the grid and hints )
    private void DisplayBoard()
    {
        ClearOldVisuals();

        // Retrieve the grid from the GridManager.
        Grid grid = gridManager.GetGrid();

        // Use the grid's exposed properties for rows and columns.
        int rows = grid.GetHeight();
        int cols = grid.GetWidth();

        // Debug.Log($"GridVisualManager: Grid dimensions: {rows} x {cols}");

        // Retrieve the hints from the GridManager.
        HintBlock[] hintsRowArray = grid.GetHintRowArray();
        HintBlock[] hintsColArray = grid.GetHintColArray();

        DisplayHints(grid, rows, cols);

        // Now instantiate visual blocks based on grid data.
        DisplayGrid(grid, rows, cols);
    }
    private void DisplayGrid(Grid grid, int rows, int cols)
    {
        // Retrieve the blocks array (if needed).
        GridBlock[,] gridBlocks = grid.GetBlocks();

        float CellSize= grid.GetCellSize();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {

                // Calculate the world position for the block (adjust as needed).
                Vector3 position = new Vector3(j*CellSize, -i*CellSize, 0);

                // Instantiate the block prefab.
                GameObject blockObj = Instantiate(blockPrefab, position, Quaternion.identity, gridParent);

                // Get the BlockView component attached to the prefab.
                BlockView blockView = blockObj.GetComponent<BlockView>();
                if (blockView != null)
                {
                    // Initialize the visual block using the grid block data.
                    blockView.Initialize(gridBlocks[i, j]);
                }
                else
                {
                    Debug.LogError("BlockView component not found on block prefab: " + blockObj.name);
                }
            }
        }
    }

    private void DisplayHints(Grid grid,int rows,int cols)
    {
        float CellSize = grid.GetCellSize();
        

        for (int i = 0;i < rows; i++)
        {
            // Calculate the world position for the block (adjust as needed).
            Vector3 position = new Vector3(rows * CellSize, -i * CellSize, 0);

            // Instantiate the block prefab.
            GameObject blockObj = Instantiate(hintPrefab, position, Quaternion.identity, gridParent);

            HintView hintView = blockObj.GetComponent<HintView>();
            if (hintView != null) 
            {
                hintView.Initialize(grid.GetHintBlockFromRow(i));
            }
            else
            {
                Debug.LogError("HintView component not found on block prefab: " + blockObj.name);
            }

        }

        for (int j = 0; j < rows; j++)
        {
            // Calculate the world position for the block (adjust as needed).
            Vector3 position = new Vector3(j * CellSize, -cols * CellSize, 0);

            // Instantiate the block prefab.
            GameObject blockObj = Instantiate(hintPrefab, position, Quaternion.identity, gridParent);

            HintView hintView = blockObj.GetComponent<HintView>();
            if (hintView != null)
            {
                hintView.Initialize(grid.GetHintBlockFromCol(j));
            }
            else
            {
                Debug.LogError("BlockView component not found on block prefab: " + blockObj.name);
            }

        }
    }

    private void ClearOldVisuals()
    {
        // Loop through each child in gridParent and destroy it.
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }
    }

    public Vector3 GetGridCenter()
    {
        Grid grid = gridManager.GetGrid();
        float cellSize= grid.GetCellSize();

        float centerX = (grid.GetWidth() - 1) * cellSize / 2f;
        float centerY = -(grid.GetHeight() - 1) * cellSize / 2f;

        Vector3 gridCenter = new Vector3(centerX, centerY, 0)+gridParent.position;

        return gridCenter;
    }
}
