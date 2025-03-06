using UnityEngine;
using TMPro;
using System;

public class HintView : MonoBehaviour
{
    // Reference to the TextMeshPro component for displaying the value sum of line or row.
    public TextMeshPro valueSumText;
    // Reference to the TextMeshPro component for displaying the mine sum of line or row.
    public TextMeshPro mineSumText;
    // Reference to the GameObject for the background placeholder sprite.
    public GameObject backgroundPlaceholderSprite;
    
    // Reference to the HintBlock object that holds the hint's data
    private HintBlock blockData;

    public void Initialize(HintBlock data)
    {
        blockData = data;

        //Debug.Log("Value Sum assigned:"+data.GetValueSum() +"     Mine Sum assigned:"+data.GetMineSum());

        // Make sure text objects are active
        valueSumText.gameObject.SetActive(true);
        mineSumText.gameObject.SetActive(true);

        // Assign data values to text objects
        valueSumText.text=data.GetValueSum().ToString();
        mineSumText.text = data.GetMineSum().ToString();   
    }
}