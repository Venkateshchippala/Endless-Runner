using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public UIHandler uihandler;
    public Animator characterAnimator;
    public GameObject player;
    public GameObject character;
    // public GameObject[] allCharecters;
    public List<GameObject> allCharecters = new List<GameObject>();
    private Coroutine moveCoroutine;
    public float[] xPositions = { -1.5f, 0f, 1.5f };
    private float movementSpeed = 5f;
    private Vector3 firstTouchPos;
    private Vector3 lastTouchPos;
    private float dragDistance = 0.02f;
    private int currentPosition = 0;
    public float playerMoveSpeed = 4f;
    private float targetX;
    private bool isJumping = false;
    private bool isRolling = false;
    public bool isGameOver = false;
    public bool isFallDown = false;
    private float jumpHeight = 2f;
    private string[] rollStyle = { "roll", "SpringRoll" };
    // int[] temp_roll = { 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1 };
    int[] temp_roll = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    private void Awake()
    {
        /* allCharecters[0].gameObject.SetActive(false);
         allCharecters.RemoveAt(0);*/

        targetX = xPositions[currentPosition];
        //UpdateMovementSpeed();
        //UpdateJumpHeight();
        //UpdateSwipeDistance(); // Initialize swipe distance
    }
    private void ProcessTouch(Touch touch)
    {
        switch (touch.phase)
        {
            case TouchPhase.Began:
                firstTouchPos = touch.position;
                lastTouchPos = touch.position;
                break;

            case TouchPhase.Moved:
                lastTouchPos = touch.position;
               // EvaluateSwipe();
                break;

            case TouchPhase.Ended:
                lastTouchPos = touch.position;
                EvaluateSwipe();
                break;
        }
    }
    private void HandleTouchInput()
    {
        if (isGameOver)
        {
            return;
        }

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            ProcessTouch(touch);
        }
    }
    private void EvaluateSwipe()
    {

        float horizontalDistance = Mathf.Abs(lastTouchPos.x - firstTouchPos.x);
        float verticalDistance = Mathf.Abs(lastTouchPos.y - firstTouchPos.y);

        if (horizontalDistance > dragDistance || verticalDistance > dragDistance)
        {
            if (horizontalDistance > verticalDistance)
            {
                if (lastTouchPos.x > firstTouchPos.x)
                {
                    MoveRight();
                    print("right");
                }
                else
                {
                    MoveLeft();
                    print("left");
                }
            }
            else
            {
                if (lastTouchPos.y > firstTouchPos.y)
                {
                    Jump();
                    print("up");
                }
                else
                {
                    RollDown();
                    print("down");
                }
            }
        }
    }

    private void SwipeControlls()
    {
       
    }

    private void Jump()
    {
        // Prevent jumping while rolling
        if (isJumping || isRolling)
        {
            return;
        }

        isJumping = true;
        characterAnimator.SetBool("jump", true);
        StartCoroutine(JumpRoutine());
    }
    private void RollDown()
    {
        if (!isRolling)
        {
            isRolling = true;
            int temp2 = UnityEngine.Random.Range(0, temp_roll.Length);
            string rollType = rollStyle[temp_roll[temp2]];
            characterAnimator.SetBool(rollType, true);
            //  playerTrigger.transform.localPosition = new Vector3(0, 0, 0);
            StartCoroutine(ResetRoll(rollType));


        }
    }
    private IEnumerator ResetRoll(string _roll)
    {
        BoxCollider box = character.transform.GetChild(0).GetComponent<BoxCollider>();
        float maxY = box.size.y;
        float minY = maxY / 2;
        Vector3 startPosition = new Vector3(0, 0, 0);
        Vector3 endPosition = new Vector3(0, 0.65f, 0);
        float downDuration = 0.35f; // Duration for going down
                                    //  float upDuration = 0.2f; // Duration for going up
        float elapsed = 0f;

        // Move playerTrigger down to end position
        while (elapsed < downDuration)
        {
            elapsed += Time.deltaTime;
            // playerTrigger.transform.localPosition = Vector3.Lerp(endPosition, startPosition, elapsed / downDuration);
            //   Debug.Log("Going down");
            box.size = new Vector3(box.size.x, minY, box.size.z);
            yield return null;
        }

        // Ensure it reaches the exact start position
        //playerTrigger.transform.localPosition = startPosition;

        // Wait for 2 seconds before starting to move up
        yield return new WaitForSeconds(1f);


        isRolling = false;
        box.size = new Vector3(box.size.x, maxY, box.size.z);
        // Ensure it reaches the exact end position
        // playerTrigger.transform.localPosition = endPosition;

        // isRolling = false;
        characterAnimator.SetBool(_roll, false);
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
            characterAnimator.SetBool("jump", false);
    }
    private void MoveLeft()
    {
        currentPosition--;
        currentPosition = Mathf.Clamp(currentPosition, 0, xPositions.Length - 1);
        targetX = xPositions[currentPosition];

        StartMoveCoroutine();
    }

    private void MoveRight()
    {
        currentPosition++;
        currentPosition = Mathf.Clamp(currentPosition, 0, xPositions.Length - 1);
        targetX = xPositions[currentPosition];

        StartMoveCoroutine();
    }
    private void StartMoveCoroutine()
    {
        Vector3 targetPosition = new Vector3(targetX, character.transform.localPosition.y, character.transform.localPosition.z);

        // If there is already a running coroutine, stop it before starting a new one
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        // Start the new coroutine and store its reference
        moveCoroutine = StartCoroutine(MoveCharacterRoutine(targetPosition));
    }
    private IEnumerator MoveCharacterRoutine(Vector3 targetPosition)
    {
        // Keep moving the character until it reaches the target position
        while (Vector3.Distance(character.transform.localPosition, targetPosition) > 0.01f)
        {
            character.transform.localPosition = Vector3.MoveTowards(character.transform.localPosition, targetPosition, movementSpeed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }
    }
    private void Update()
    {
        if (GameManager._inst.gameStatus == true)
        {
            HandleTouchInput();
            PlayerMove();
        }

    }
    public void PlayerMove()
    {
        // player.transform.position = Vector3.forward * Time.deltaTime * playerMoveSpeed;
        player.transform.Translate(Vector3.forward * GameManager._inst.speed * Time.deltaTime);
    }



}
