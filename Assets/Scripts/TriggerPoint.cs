using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TriggerPoint : MonoBehaviour
{
    public UIHandler uihandler;
    public CoinsCollector coinscollector;
    public PlayerController playercontroller;
    public RoadController roadcontroller;
    public Animator charecterAnimatorRef;

    private void Awake()
    {
        playercontroller = FindObjectOfType<PlayerController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TTTTTTTTT");
        if (other.gameObject.CompareTag("TriggerOne"))
        {
            roadcontroller.roads[1].transform.position = roadcontroller.roads[0].transform.position+new Vector3(0,0,108f);
            coinscollector.ActiveSetOneCoins();
            Debug.Log(other.name);
        }
        if (other.gameObject.CompareTag("TriggerTwo"))
        {
            roadcontroller.roads[0].transform.position = roadcontroller.roads[1].transform.position+new Vector3(0,0,108f);
            coinscollector.ActiveSetTwoCoins();
            Debug.Log(other.name);
            Debug.Log("Hi");
        }
        if (other.gameObject.CompareTag("Collider"))
        {
            GameManager._inst.speed = 0;
           // charecterAnimatorRef.SetBool("death", true);
            playercontroller.characterAnimator.SetBool("death", true);
            GameManager._inst.GameOver(GameManager._inst.score);
            GameManager._inst.gameStatus = false;
            
            Debug.Log("Collision");
        }
        if (other.gameObject.CompareTag("Coins"))
        {
            other.gameObject.GetComponent<MeshRenderer>().enabled = false;
            GameManager._inst.highestScore++;
            GameManager._inst.levelScore++;
            PlayerPrefs.SetInt("HighestScore", GameManager._inst.highestScore);
            GameManager._inst.highestScore = PlayerPrefs.GetInt("HighestScore");
            GameManager._inst.highestScoreTxt.text = GameManager._inst.highestScore.ToString();
            GameManager._inst.levelScoreTxt.text = GameManager._inst.levelScore.ToString();
           // GameManager._inst.ScoreUpdater(GameManager._inst.score,GameManager._inst.txt_score);
          
        }
    }
}
