using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Grid settings (adjust as needed or obtain them from your grid manager)
    public int rows = 5;
    public int cols = 5;
    public float cellSize = 1f;

    // Multipliers to adjust the view area
    public float verticalPaddingMultiplier = 1.5f;
    public float horizontalPaddingMultiplier = 1.0f;

    // New option to force a square view (ignoring the screen's aspect ratio)
    public bool forceSquareView = false;

    void Start()
    {
        // Calculate the grid center in local space
        float centerX = (cols - 1) * cellSize / 2f;
        float centerY = -(rows - 1) * cellSize / 2f;
        Vector3 gridCenter = new Vector3(centerX, centerY, transform.position.z);

        // Position the camera at the grid center
        transform.position = gridCenter;
        Debug.Log("Camera centered at: " + transform.position);

        // Get the main camera component
        Camera cam = GetComponent<Camera>();
        if (cam != null && cam.orthographic)
        {
            // Calculate the grid dimensions in world units
            float gridHeight = rows * cellSize;
            float gridWidth = cols * cellSize;
            float screenAspect = (float)Screen.width / Screen.height;

            if (forceSquareView)
            {
                // For a square view, use the larger of the grid's dimensions.
                // Orthographic size is half the vertical viewing size in world units.
                float desiredSize = Mathf.Max(gridHeight, gridWidth) / 2f;
                cam.orthographicSize = desiredSize * verticalPaddingMultiplier;
            }
            else
            {
                // Standard calculation using the aspect ratio.
                float sizeBasedOnHeight = gridHeight / 2f;
                float sizeBasedOnWidth = gridWidth / (2f * screenAspect);
                float requiredSizeVertical = sizeBasedOnHeight * verticalPaddingMultiplier;
                float requiredSizeHorizontal = sizeBasedOnWidth * horizontalPaddingMultiplier;
                cam.orthographicSize = Mathf.Max(requiredSizeVertical, requiredSizeHorizontal);
            }

            Debug.Log("Camera orthographicSize set to: " + cam.orthographicSize);
        }
    }
}
