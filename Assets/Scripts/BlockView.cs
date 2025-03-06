using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;
using Unity.VisualScripting;

public class BlockView : MonoBehaviour
{
    // Reference to the TextMeshPro component for displaying the block's value.
    public TextMeshPro textMesh;

    // Reference to the GameObject for the mine sprite.
    public GameObject mineSpriteObject;

    // Reference to the cover layer GameObject (the 2D square you added).
    public GameObject coverLayer;

    // Reference to the mine flag of the Grid Block
    public GameObject mineFlag;

    // Reference to the flag 1 of the Grid Block
    public TextMeshPro flag1;

    // Reference to the flag 2 of the Grid Block
    public TextMeshPro flag2;

    // Reference to the flag 3 of the Grid Block
    public TextMeshPro flag3;

    // Data of the Grid Block
    private GridBlock blockData;

    // Reference to the Grid Manager
    private GridManager gridManager;


    // Called by your grid initialization code.
    public void Initialize(GridBlock data)
    {
        // Initialise grid block flags
        mineFlag.gameObject.SetActive(false);
        flag1.gameObject.SetActive(false);
        flag2.gameObject.SetActive(false);
        flag3.gameObject.SetActive(false);


        blockData = data;

        // Find the grid manager
        gridManager = GameObject.FindFirstObjectByType<GridManager>();

        // Subscribe to the GridBlock's event so that when it is clicked (logic-wise),
        // this BlockView reacts by hiding its cover.
        blockData.OnBlockRevealed += OnBlockRevealed;
        blockData.OnBlockFlagged += OnBlockFlagged;

        // Update visuals based on the block's data.
        if (blockData.IsMine())
        {
            Debug.Log("en pompa");
            if (mineSpriteObject != null)
            {
                Debug.Log("ivra to");
                mineSpriteObject.SetActive(true);

            }
            else
            {
                Debug.Log("en ivra to sprite tis pompa");
            }

            if (textMesh != null)
            {

                textMesh.gameObject.SetActive(false);
            }
        }
        else
        {
            if (mineSpriteObject != null)
                mineSpriteObject.SetActive(false);
            if (textMesh != null)
            {
                textMesh.gameObject.SetActive(true);
                textMesh.text = blockData.GetBlockValue().ToString();
            }
        }

        // Ensure the cover layer is enabled initially.
        if (coverLayer != null)
            coverLayer.SetActive(true);
    }

    // Function that gets triggered when the grid block is revealed.
    private void OnBlockRevealed(object sender, EventArgs e)
    {
        RevealBlock();
    }

    // Function that gets triggered when the grid block is flagged.
    private void OnBlockFlagged(int flagNumber)
    {
        SetFlag(flagNumber);
    }

    // This method returns the block connected to the BlockView object.
    public GridBlock GetGridBlock()
    {
        return this.blockData;
    }

    // This method is called to reveal the block (hide the cover).
    public void RevealBlock()
    {
        if (coverLayer != null)
        {
            // Disable or hide the cover layer to reveal the underlying value.
            coverLayer.SetActive(false);
            mineFlag.SetActive(false);
            flag1.gameObject.SetActive(false);
            flag2.gameObject.SetActive(false);
            flag3.gameObject.SetActive(false);
        }
    }

    // This method is called to flag the block.
    public void SetFlag(int flagNumber)
    {
        if (flagNumber == 1)
        {
            mineFlag.SetActive(!mineFlag.activeSelf);
        }
        else if (flagNumber == 2)
        {
            flag1.gameObject.SetActive(!flag1.gameObject.activeSelf);
        }
        else if (flagNumber == 3)
        {
            flag2.gameObject.SetActive(!flag2.gameObject.activeSelf);
        }
        else if (flagNumber == 4)
        {
            flag3.gameObject.SetActive(!flag3.gameObject.activeSelf);
        }
    }
}