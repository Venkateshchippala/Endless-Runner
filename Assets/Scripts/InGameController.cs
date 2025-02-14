using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameController : MonoBehaviour
{
    public Animator animatorController;
    public GameObject player;
    public Transform[] position;
    private Transform targetPosition;
    int currentposition = 1;
    public int value = 0;
    float speed = 5;
    public bool leftClick = false;
    public bool rightClick = false;
    private Touch theTouch;
    private Vector2 touchStartPosition, touchEndPosition;
  
    
    float jumpHeight = 2f;
    // Start is called before the first frame update


   
   
    private void Update()
    {

        if (Input.touchCount > 0)
        {
            theTouch = Input.GetTouch(0);

            if (theTouch.phase == TouchPhase.Began)
            {
                touchStartPosition = theTouch.position;
            }
            else if (theTouch.phase == TouchPhase.Ended)
            {
                touchEndPosition = theTouch.position;

                float x = touchEndPosition.x - touchStartPosition.x;
                float y = touchEndPosition.y - touchStartPosition.y;

                if (Mathf.Abs(x) == 0 && Mathf.Abs(y) == 0)
                {
                    Debug.Log( "Tapped");
                }
                else if (Mathf.Abs(x) > Mathf.Abs(y))
                {
                    // direction = x > 0 ? "Right" : "Left";
                    if (x > 0)
                    {
                        currentposition++;

                        currentposition = Mathf.Clamp(currentposition, 0, 2);
                        targetPosition = position[currentposition];
                        rightClick = true;
                    }
                    else
                    {
                        currentposition--;

                        currentposition = Mathf.Clamp(currentposition, 0, 2);
                        targetPosition = position[currentposition];
                        leftClick = true;
                    }
                }
                else
                {
                    // direction = y > 0 ? "Up" : "Down";
                    if (y > 0)
                    {
                       // Debug.Log("Up");
                       // isJump = true;
                        Jump();
                        animatorController.SetBool("jump", true);
                        

                    }
                    else
                    {
                        Debug.Log("Down");
                      
                        animatorController.SetBool("slide", true);


                    }
                }
            }
        }
        if (leftClick == true || rightClick == true)
        {
            Vector3 dir = targetPosition.transform.localPosition - player.transform.localPosition;

            float dist = Vector3.Distance(targetPosition.transform.localPosition, player.transform.localPosition);
            Debug.Log("dist :" + dist);
            if (dist > 0.1f)
            {
                player.transform.localPosition += (dir).normalized * Time.deltaTime * speed;

            }
            else
            {
                player.transform.localPosition = targetPosition.transform.localPosition;
            }
        }
       

    }
    private void Jump()
    {
        StartCoroutine(JumpRoutine());
        Debug.Log("I Am Jumping");
    }
    private IEnumerator JumpRoutine()
    {
        Transform child = player.transform.GetChild(0);
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

    }

   

    
}
