using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public PlayerController playerControllerRef;
    public UIHandler uihandler;
    public static GameManager _inst;
    public Text highestScoreTxt;
    public Text levelScoreTxt;
    
    public int highestScore;
    public int levelScore=0;
    [HideInInspector]
    public  int  score = 0;
    public float speed = 0;
    public bool gameStatus = false;
    public int selectCharector;
   

   
    private void Awake()
    {
      //  PlayerPrefs.DeleteAll();
        _inst = this;

        highestScore = PlayerPrefs.GetInt("HighestScore",0);
        highestScoreTxt.text = highestScore.ToString();
        levelScoreTxt.text = levelScore.ToString();
            
    }
    // Start is called before the first frame update
    void Start()
    {
       // uihandler.startgamePanel.SetActive(true);
        for(int i = 0; i < playerControllerRef.allCharecters.Count; i++)
        {
            playerControllerRef.allCharecters[i].gameObject.SetActive(false);
        }
        playerControllerRef.allCharecters[0].gameObject.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ScoreUpdater(int _score,Text txt_score)
    {
      //  PlayerPrefs.SetInt("score", _score);
        txt_score.text = _score.ToString();
    }
    public void GameOver( int score)
    {
        uihandler.restartgamePanel.SetActive(true);
        if (score > PlayerPrefs.GetInt("score")) {
            PlayerPrefs.SetInt("score", GameManager._inst.score);
        }       
    }

    public void Change_Charactor(int indexval)
    {
        selectCharector = selectCharector + indexval;
        selectCharector = Mathf.Clamp(selectCharector, 0, 7);
        for(int i = 0; i < playerControllerRef.allCharecters.Count; i++)
        {
            playerControllerRef.allCharecters[i].gameObject.SetActive(false);
        }
        playerControllerRef.allCharecters[selectCharector].gameObject.SetActive(true);
        // Apply Animator to all Charectors
        playerControllerRef.characterAnimator = playerControllerRef.allCharecters[selectCharector].GetComponent<Animator>();

    }
}
