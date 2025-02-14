using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestDoWhile : MonoBehaviour
{
    public TextMeshProUGUI tmProText;
    private int rows = 4;
    private int columns = 5;
    // Start is called before the first frame update
    void Start()
    {
        
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns-i-1; j++)
            {

                tmProText.text += " - ";

            }
            for(int v = 0; v < (2 * i) + 1; v++)
            {
                tmProText.text += " * ";
            }
            tmProText.text += "\n";

        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
