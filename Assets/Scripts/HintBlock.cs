using UnityEngine;

public class HintBlock
{
    private int valueSum;
    private int mineSum;
    private int position;

    public HintBlock(int valueSum, int mineSum, int position)
    {
        this.valueSum = valueSum;
        this.mineSum = mineSum;
        this.position = position;
    }

    public int GetValueSum()
    {
        return valueSum;
    }

    public int GetMineSum() 
    { 
        return mineSum; 
    }

    public int GetPosition()
    {
        return position;
    }

    public void SetPosition(int position)
    {
        this.position = position;
    }

    public void SetValueSum(int valueSum)
    {
        this.valueSum = valueSum;
    }

    public void SetMineSum(int mineSum)
    {
        this.mineSum = mineSum;
    }

}
