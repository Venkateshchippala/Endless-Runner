using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerControllerOld : MonoBehaviour
{
    public Transform player;
    public float[] xPositions = { -1.5f, 0f, 1.5f };
    public GameObject character;
    public Transform playerTrigger;
    public Slider movementSlider;
    public Text movementText;
    public Slider jumpHeightSlider;
    public Text jumpHeightText;
    public Slider swipeDistanceSlider; // New slider for swipe distance
    public Text swipeDistanceText; // New text for displaying swipe distance value

    public Animator characterAnimator;
    public bool isGameOver = false;
    public bool isFallDown = false;

    private float targetX;
    private int currentPosition = 1;
    private Vector3 firstTouchPos;
    private Vector3 lastTouchPos;
    private float dragDistance = 0.05f;
    private bool isJumping = false;
    private bool isRolling = false;

    private float movementSpeed = 0f;
    private float jumpHeight = 0f;

    private string[] rollStyle = { "Roll", "SpringRoll" };
    int[] temp_roll = { 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1 };

    public int fallingDownSpeed = 5;
    private void Awake()
    {
        targetX = xPositions[currentPosition];
        UpdateMovementSpeed();
        UpdateJumpHeight();
        UpdateSwipeDistance(); // Initialize swipe distance
    }

    private void Update()
    {
        if (isFallDown)
        {
            player.position = new Vector3(player.position.x, player.position.y - fallingDownSpeed * Time.deltaTime, player.transform.position.z);
        }
        //  HandleKeyboardInput();
        HandleTouchInput();
        MoveCharacter();
    }

    private void HandleTouchInput()
    {
        if (isGameOver)
        {
            return;
        }

        //  Debug.Log("touchcout::"+Input.touchCount);
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);          
            if (touch.phase == TouchPhase.Began)// First touch point
            {
                firstTouchPos = touch.position;
                lastTouchPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)// touch moved
            {
                lastTouchPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)// touch removed
            {
                lastTouchPos = touch.position;
                Debug.Log("FirstTouchPos=" + firstTouchPos + "LastTouchPos:=" + lastTouchPos);

                float horizontalDistance = Mathf.Abs(lastTouchPos.x - firstTouchPos.x);

                float verticalDistance = Mathf.Abs(lastTouchPos.y - firstTouchPos.y);
                Debug.Log("horizontalDistance::" + horizontalDistance + " , " + "Veticel distace:" + verticalDistance);
                if (horizontalDistance > dragDistance || verticalDistance > dragDistance)
                {
                    if (horizontalDistance > verticalDistance)
                    {
                        if (lastTouchPos.x > firstTouchPos.x)
                        {
                            MoveRight();
                        }
                        else
                        {
                            MoveLeft();
                        }
                    }
                    else
                    {
                        if (lastTouchPos.y > firstTouchPos.y)
                        {
                            Jump();
                        }
                        else
                        {
                            // Trigger roll on downward swipe
                            RollDown();
                        }
                    }
                }
            }
        }
    }

    private void RollDown()
    {
        if (!isRolling)
        {
            isRolling = true;
            int temp2 = UnityEngine.Random.Range(0, temp_roll.Length);
            string rollType = rollStyle[temp_roll[temp2]];
            characterAnimator.SetBool(rollType, true);
            playerTrigger.transform.localPosition = new Vector3(0, 0, 0);
            StartCoroutine(ResetRoll(rollType));
        }
    }

    private void MoveCharacter()
    {
        Vector3 targetPosition = new Vector3(targetX, character.transform.localPosition.y, character.transform.localPosition.z);
        character.transform.localPosition = Vector3.MoveTowards(character.transform.localPosition, targetPosition, movementSpeed * Time.deltaTime);
    }

    private void MoveLeft()
    {
        currentPosition--;
        currentPosition = Mathf.Clamp(currentPosition, 0, xPositions.Length - 1);
        targetX = xPositions[currentPosition];
    }

    private void MoveRight()
    {
        currentPosition++;
        currentPosition = Mathf.Clamp(currentPosition, 0, xPositions.Length - 1);
        targetX = xPositions[currentPosition];
    }

    private void Jump()
    {
        // Prevent jumping while rolling
        if (isJumping || isRolling)
        {
            return;
        }

        isJumping = true;
        characterAnimator.SetBool("Jump", true);
        StartCoroutine(JumpRoutine());
    }

    private IEnumerator JumpRoutine()
    {
        Transform child = character.transform.GetChild(0);
        float jumpDuration = 0.5f;
        float initialY = child.localPosition.y;
        float targetY = initialY + jumpHeight;

        // Jump up
        float elapsed = 0f;
        while (elapsed < jumpDuration)
        {
            elapsed += Time.deltaTime;
            float newY = Mathf.Lerp(initialY, targetY, elapsed / jumpDuration);
            child.localPosition = new Vector3(child.localPosition.x, newY, child.localPosition.z);
            yield return null;
        }

        // Fall down
        elapsed = 0f;
        while (elapsed < jumpDuration)
        {
            elapsed += Time.deltaTime;
            float newY = Mathf.Lerp(targetY, initialY, elapsed / jumpDuration);
            child.localPosition = new Vector3(child.localPosition.x, newY, child.localPosition.z);
            yield return null;
        }

        isJumping = false;
        if (!isGameOver)
            characterAnimator.SetBool("Jump", false);
    }

    private void Roll(string _roll)
    {
        if (!isRolling)
        {
            isRolling = true;
            characterAnimator.SetBool(_roll, true);
            playerTrigger.transform.localPosition = new Vector3(0, 0, 0);
            StartCoroutine(ResetRoll(_roll));
        }
    }


    private IEnumerator ResetRoll(string _roll)
    {
        Vector3 startPosition = new Vector3(0, 0, 0);
        Vector3 endPosition = new Vector3(0, 0.65f, 0);
        float downDuration = 0.35f; // Duration for going down
                                    //  float upDuration = 0.2f; // Duration for going up
        float elapsed = 0f;

        // Move playerTrigger down to end position
        while (elapsed < downDuration)
        {
            elapsed += Time.deltaTime;
            playerTrigger.transform.localPosition = Vector3.Lerp(endPosition, startPosition, elapsed / downDuration);
            //   Debug.Log("Going down");
            yield return null;
        }

        // Ensure it reaches the exact start position
        playerTrigger.transform.localPosition = startPosition;

        // Wait for 2 seconds before starting to move up
        yield return new WaitForSeconds(1f);


        isRolling = false;
        // Ensure it reaches the exact end position
        playerTrigger.transform.localPosition = endPosition;

        // isRolling = false;
        characterAnimator.SetBool(_roll, false);
    }



    public void StartRuning()
    {
        characterAnimator.SetBool("Running", true);
    }
    public void UpdateMovementSpeed()
    {
        movementSpeed = movementSlider.value * 50;
        movementText.text = movementSpeed.ToString("F2");
    }

    public void UpdateJumpHeight()
    {
        jumpHeight = jumpHeightSlider.value * 10;
        jumpHeightText.text = jumpHeight.ToString("F2");
    }

    public void UpdateSwipeDistance()
    {
        dragDistance = swipeDistanceSlider.value * Screen.height;
        swipeDistanceText.text = (dragDistance / Screen.height).ToString("F2");
    }
    public void PlayerFallDown()
    {
        characterAnimator.SetTrigger("Falling");
    }
    public void ResetScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}