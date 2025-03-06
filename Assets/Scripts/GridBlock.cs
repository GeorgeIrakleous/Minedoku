using System;
using UnityEngine;

public class GridBlock
{
    private int blockValue;
    private bool blockIsClicked;
    private int x;
    private int y;

    public event EventHandler OnBlockRevealed;
    public event Action<int> OnBlockFlagged;

    public GridBlock(int blockValue, int x, int y)
    {
        this.blockValue = blockValue;
        this.blockIsClicked = false;
        this.x = x;
        this.y = y;
    }

    // Returns the value stored in the block.
    public int GetBlockValue()
    {
        return blockValue;
    }

    public bool GetIsBlockClicked()
    {
        return blockIsClicked;
    }

    public int GetBlockX()
    {
        return x;
    }

    public int GetBlockY()
    {
        return y;
    }

    public void SetBlockX(int x)
    {
        this.x = x;
    }

    public void SetBlockY(int y)
    {
        this.y = y;
    }


    public void SetBlockValue(int blockValue)
    {
        this.blockValue = blockValue;
    }

    // Sets the block as clicked and fires the event if it hasn't been clicked before.
    public void SetBlockClicked()
    {
        if (!blockIsClicked)
        {
            blockIsClicked = true;
            BlockRevealed(EventArgs.Empty);
        }
    }
    
    public void SetBlockFlagged(int flagNumber)
    {
        BlockFlagged(flagNumber);
    }

    // Protected virtual method to allow derived classes to override event invocation.
    private void BlockRevealed(EventArgs e)
    {
        OnBlockRevealed?.Invoke(this, e);
    }

    private void BlockFlagged(int flagNumber)
    {
        OnBlockFlagged?.Invoke(flagNumber);
    }

    // In this design, a block with value 0 is treated as a mine.
    public bool IsMine()
    {
        return blockValue == 0;
    }

    // (Optional) You can add additional properties or methods later,
    // such as whether the block is flagged or revealed.
}