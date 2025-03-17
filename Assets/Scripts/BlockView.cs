using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;
using Unity.VisualScripting;
using DG.Tweening;


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
        // Ensure flags are disabled initially
        mineFlag.SetActive(false);
        flag1.gameObject.SetActive(false);
        flag2.gameObject.SetActive(false);
        flag3.gameObject.SetActive(false);


        // Set initial scale to zero (invisible)
        transform.localScale = Vector3.zero;

        // Play the scale-up animation over 0.5 seconds with a bounce effect
        transform.DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.OutBack) // Makes the animation feel natural and bouncy
            .SetDelay(UnityEngine.Random.Range(0f, 0.3f)); // Optional: Adds slight delay for variation

        // Rotate effect (rotates 360° over time and resets)
        transform.DORotate(Vector3.forward * 360, 0.5f, RotateMode.FastBeyond360)
            .SetEase(Ease.OutCubic);

        blockData = data;
        gridManager = GameObject.FindFirstObjectByType<GridManager>();

        blockData.OnBlockRevealed += OnBlockRevealed;
        blockData.OnBlockFlagged += OnBlockFlagged;

        // Update visuals
        if (blockData.IsMine())
        {
            if (mineSpriteObject != null) mineSpriteObject.SetActive(true);
            if (textMesh != null) textMesh.gameObject.SetActive(false);
        }
        else
        {
            if (mineSpriteObject != null) mineSpriteObject.SetActive(false);
            if (textMesh != null)
            {
                textMesh.gameObject.SetActive(true);
                textMesh.text = blockData.GetBlockValue().ToString();
            }
        }

        if (coverLayer != null) coverLayer.SetActive(true);
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

    private void OnDestroy()
    {
        blockData.OnBlockRevealed -= OnBlockRevealed;
        blockData.OnBlockFlagged -= OnBlockFlagged;
    }

}