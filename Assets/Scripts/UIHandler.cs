using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{
    public PlayerController playercontroller;
    public GameObject startgamePanel;
    public GameObject restartgamePanel;
    public GameObject playerSelectedPanel;
    public bool startGame = false;
    // Start is called before the first frame update
    void Start()
    {
        restartgamePanel.gameObject.SetActive(false);
       // startgamePanel.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Start_BtnClick()
    {
        GameManager._inst.gameStatus = true;
        startGame = true;
        startgamePanel.gameObject.SetActive(false);
        GameManager._inst.speed = playercontroller.playerMoveSpeed;
        playercontroller.characterAnimator.SetBool("run",true);
        playerSelectedPanel.gameObject.SetActive(false);
    }
    public void Restart_BtnClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        restartgamePanel.gameObject.SetActive(false);
       // startgamePanel.gameObject.SetActive(true);
    }
}
