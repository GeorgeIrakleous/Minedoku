using UnityEngine;
using DG.Tweening;
using System.Collections;

public class BackgroundManager : MonoBehaviour
{
    [Header("Square Prefabs (Chessboard Pattern)")]
    // Assign the two square prefabs (different colors) in the Inspector.
    public GameObject squarePrefab1;
    public GameObject squarePrefab2;

    [Header("Grid Settings")]
    // Number of columns and rows in each grid.
    public int gridColumns = 10;
    public int gridRows = 10;
    // Size of each square (world units). Set to 5 if that works best.
    public float squareSize = 5f;
    // The position at which each new grid will spawn.
    public Vector2 gridSpawnOffset = Vector2.zero;
    // How frequently (in seconds) a new grid is spawned.
    public float spawnInterval = 2f;

    [Header("Movement Settings")]
    // Direction for the squares to move.
    public Vector2 moveDirection = new Vector2(-1f, 1f);
    // Movement speed in world units per second.
    public float moveSpeed = 50f;
    // How far each square moves before it is destroyed.
    public float moveDistance = 100f;

    private void Start()
    {
        // Normalize the movement direction so that speed is consistent.
        moveDirection.Normalize();
        // Start continuously spawning grids.
        StartCoroutine(SpawnGridCoroutine());
    }

    private IEnumerator SpawnGridCoroutine()
    {
        while (true)
        {
            SpawnGrid();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnGrid()
    {
        // The starting position for this grid is defined by gridSpawnOffset.
        Vector2 spawnPosition = gridSpawnOffset;
        // Loop through columns and rows to spawn the chessboard pattern.
        for (int col = 0; col < gridColumns; col++)
        {
            for (int row = 0; row < gridRows; row++)
            {
                // Calculate the position for this square.
                Vector2 pos = spawnPosition + new Vector2(col * squareSize, -row * squareSize);
                // Alternate between the two prefabs based on (col + row).
                GameObject prefabToUse = ((col + row) % 2 == 0) ? squarePrefab1 : squarePrefab2;
                // Instantiate the square as a child of the BackgroundManager.
                GameObject square = Instantiate(prefabToUse, pos, Quaternion.identity, transform);
                // Animate the square's movement.
                AnimateSquare(square);
            }
        }
    }

    private void AnimateSquare(GameObject square)
    {
        // Calculate the duration of the movement based on moveDistance and moveSpeed.
        float duration = moveDistance / moveSpeed;
        // Determine the target position by moving along the normalized moveDirection.
        Vector3 targetPos = square.transform.position + (Vector3)(moveDirection * moveDistance);
        // Animate the square's movement linearly.
        square.transform.DOMove(targetPos, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() => Destroy(square));
    }
}
