using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsCollector : MonoBehaviour
{
    public GameObject[] coinsSet1;
    public GameObject[] coinsSet2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CoinsRotation();
    }
    public void CoinsRotation()
    {
        for(int i = 0; i < coinsSet1.Length; i++)
        {
            coinsSet1[i].gameObject.transform.Rotate(Vector3.forward * Time.deltaTime * 125f);
        }
        for (int i = 0; i < coinsSet2.Length; i++)
        {
            coinsSet2[i].gameObject.transform.Rotate(Vector3.forward * Time.deltaTime * 125f);
        }
    }

    public void ActiveSetOneCoins()
    {
        for(int i = 0; i < coinsSet1.Length; i++)
        {
            coinsSet1[i].gameObject.GetComponent<MeshRenderer>().enabled = true;
            
            
        }
    }
    public void ActiveSetTwoCoins()
    {
        for(int i=0; i < coinsSet2.Length; i++)
        {
            coinsSet2[i].gameObject.GetComponent<MeshRenderer>().enabled = true;
            coinsSet1[i].gameObject.transform.Rotate(Vector3.up * Time.deltaTime * 7f);

        }
    }
}
