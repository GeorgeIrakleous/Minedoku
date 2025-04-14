using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    // Grid settings (adjust as needed or obtain them from your grid manager)
    public int rows = 5;
    public int cols = 5;
    public float cellSize = 1f;

    // Multipliers to adjust the view area when initially sizing the camera.
    public float verticalPaddingMultiplier = 1.5f;
    public float horizontalPaddingMultiplier = 1.0f;

    // New option to force a square view (ignoring the screen's aspect ratio)
    public bool forceSquareView = false;

    // Variables for dragging
    public float dragSpeed = 0.005f;
    private Vector3 lastMousePosition;

    // Zoom settings
    public float zoomSpeed = 1f;
    public float minZoom = 2f;
    public float maxZoom = 10f;

    public float startingPosX = -90f;

    private Camera cam;

    [SerializeField] private GameObject settingsLayout;

    void Start()
    {
        cam = GetComponent<Camera>();

        // Calculate the grid center in local space
        float centerX = (cols - 1) * cellSize / 2f;
        float centerY = -(rows - 1) * cellSize / 2f;
        Vector3 gridCenter = new Vector3(centerX, centerY, transform.position.z);

        // Position the camera at the grid center
        transform.position = gridCenter+new Vector3(startingPosX,0,0);
        //Debug.Log("Camera centered at: " + transform.position);

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

            //Debug.Log("Camera orthographicSize set to: " + cam.orthographicSize);
        }
    }

    void Update()
    {
        HandleDrag();
        HandleZoom();
        ClampCameraPosition();
    }

    // Allows dragging the camera with the mouse.
    private void HandleDrag()
    {
        if (settingsLayout.activeInHierarchy)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            // Multiply by dragSpeed to adjust movement sensitivity.
            Vector3 move = new Vector3(-delta.x * dragSpeed, -delta.y * dragSpeed, 0);
            transform.Translate(move);
            lastMousePosition = Input.mousePosition;
        }
    }

    // Adjusts the camera's orthographic size with the mouse wheel.
    private void HandleZoom()
    {
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        if (scrollDelta != 0)
        {
            cam.orthographicSize -= scrollDelta * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
    }

    // Clamps the camera position so that the grid is always at least partially visible.
    private void ClampCameraPosition()
    {
        // Define grid boundaries (assuming grid extends from (0,0) to (cols-1)*cellSize horizontally,
        // and from 0 to -(rows-1)*cellSize vertically).
        float gridLeft = 0f;
        float gridRight = (cols - 1) * cellSize;
        float gridTop = 0f;
        float gridBottom = -(rows - 1) * cellSize;

        // Calculate camera view extents in world units.
        float halfHeight = cam.orthographicSize;
        float halfWidth = cam.orthographicSize * ((float)Screen.width / Screen.height);

        // Calculate allowed camera center ranges so that the camera view overlaps with the grid.
        // The camera's left edge must be <= gridRight and right edge >= gridLeft.
        float minX = gridLeft - halfWidth;
        float maxX = gridRight + halfWidth;
        // Similarly, for vertical:
        float minY = gridBottom - halfHeight;
        float maxY = gridTop + halfHeight;

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }
}
