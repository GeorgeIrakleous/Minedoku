using UnityEngine;

public class DebugMode : MonoBehaviour
{
    void Update()
    {
        // Check if the D key was pressed
        if (Input.GetKeyDown(KeyCode.D))
        {
            // Find all BlockView components in the scene.
            BlockView[] blockViews = Object.FindObjectsByType<BlockView>(FindObjectsSortMode.None);


            // Loop through each BlockView and hide its cover.
            foreach (BlockView view in blockViews)
            {
                view.RevealBlock();
            }

            Debug.Log("Debug Mode: All block covers removed.");
        }
    }
}
