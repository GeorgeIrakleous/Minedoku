using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    // Private grid instance from your plain C# Grid class.
    private Grid grid;

    private int currentScore = 1;
    private int maxScore;

    public event Action OnScoreZero;
    public event Action<int> OnScoreUpdate;
    public event Action<int> OnScoreMax;
    public event Action OnDisplayBoard;

    private bool levelCompleted = false;

    // Optionally, you can set these via the Inspector.
    [SerializeField] private int rows = 5;
    [SerializeField] private int cols = 5;
    [SerializeField] private float cellSize = 1;

    [SerializeField] private Camera mainCamera;  // Assign your main camera in the Inspector
    [SerializeField] private LayerMask gridBlockLayer;  // Ensure your grid blocks are on this layer

    private int flagNumber;

    private UIButtons uiButtons;

    public ShopPanel shopPanel;

    public GameManager gameManager;

    private Vector3 mouseDownPos;
    private bool isDragging = false;
    public float dragThreshold = 5f; // pixels

    private void Awake()
    {
        // Create the grid using the provided parameters.
        
        levelCompleted = false;
        flagNumber = 0;

    }

    private void Start()
    {

        if (gameManager != null)
        {
            grid = new Grid(rows, cols, cellSize, gameManager.GetCurrentLevel());
            maxScore = grid.GetMaxScore();
        }
        else
        {
            Debug.Log("Couldn't find GameManager GameObject.");
        }


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

        
        if (shopPanel != null)
        {
            shopPanel.OnContinue += ContinueToNextLevel;
        }
        else
        {
            Debug.Log("Couldn't find ShopPanel GameObject.");
        }

        



    }
    private void Update()
    {
        // On mouse down, record the initial position.
        if (Input.GetMouseButtonDown(0))
        {
            mouseDownPos = Input.mousePosition;
            isDragging = false;
        }

        // If the mouse is held down, check if we've moved enough to consider it a drag.
        if (Input.GetMouseButton(0))
        {
            if (Vector3.Distance(Input.mousePosition, mouseDownPos) > dragThreshold)
            {
                isDragging = true;
            }
        }

        // On mouse release, if we didn't drag significantly, treat it as a click.
        if (Input.GetMouseButtonUp(0))
        {
            if (!isDragging)
            {
                CheckForGridBlockClick();
            }
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
                        if ((!clickedBlock.GetIsBlockClicked()) && (!levelCompleted) && (currentScore != 0))
                        {
                            // Signal the block that it was clicked to remove the cover and set blockIsClicked to true.
                            clickedBlock.SetBlockClicked();

                            currentScore *= clickedBlock.GetBlockValue(); //Update score of this specific level

                            OnScoreUpdate?.Invoke(currentScore);

                            Debug.Log("score: " + currentScore);

                            if (currentScore == 0)
                            {
                                // Notify the gameManager that the player lost
                                OnScoreZero?.Invoke();
                            }
                            else if (currentScore == maxScore)
                            {
                                // Notify the gameManager that this level is completed
                                OnScoreMax?.Invoke(currentScore);
                                levelCompleted = true;
                            }

                        }
                    }
                    // If flag number is 1,2 or 3 then the clicked grid Block gets flagged
                    else if (((flagNumber == 1) || (flagNumber == 2) || (flagNumber == 3) || (flagNumber == 4))&&(!clickedBlock.GetIsBlockClicked()))
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

    private void ContinueToNextLevel()
    {
        Debug.Log("Esinexisa");

        // Create a new grid for the next level
        grid = new Grid(rows, cols, cellSize, gameManager.GetCurrentLevel());
        maxScore = grid.GetMaxScore();

        // Fire an event to let the grid visual manager know that it needs to create new visuals for the new board
        OnDisplayBoard?.Invoke();

        // Initialise level variables
        Invoke("ResetLevel", 0.5f); // Calls ResetLevel() after 2 seconds
    }

    private void ResetLevel()
    {
        levelCompleted = false;
        currentScore = 1;
    }
    
}
