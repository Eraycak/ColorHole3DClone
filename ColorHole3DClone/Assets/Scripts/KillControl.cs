using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillControl : MonoBehaviour
{
    private GameObject holeParent;
    private GameObject door;
    private Camera mCamera;
    private int eatableCount = 0;//eatableObjects count in the first area
    private int eatableCount2 = 0;//eatableObjects count in the second area
    private bool changeIsTrue = false;//bool for activating hole's position change for first area
    private bool movedFirstLocationIsTrue = false;//bool for activating hole's position change for second area
    private Vector3 firstAreaPosition = new Vector3(0f, 0f, 0f);//hole's first area position to move after eating eatableObjects in the first area
    private Vector3 secondAreaPosition = new Vector3(0f, 0f, 65f);//hole's second area position to move after moving first position area
    private Vector3 cameraSecondAreaPosition;//camera's second area position
    private Vector3 downedDoorPosition;//door's new position after player eats all eatableObjects
    private float cameraFirstAreaPositionZ = -48.4f;
    private float cameraSecondAreaPositionZ = 44.6f;
    private float time = 0f;
    private float vibrationTimer = 0f;
    private float timeToReachInArea1 = 100f;//time to move first position
    private float timeToReachInArea2 = 250f;//time to move second position

    private void Start()
    {
        eatableCount = GameObject.FindGameObjectsWithTag("EatableObjects").Length;
        eatableCount2 = GameObject.FindGameObjectsWithTag("EatableObjects2").Length;
        holeParent = GameObject.Find("HoleParent");
        door = GameObject.Find("Door");
        mCamera = Camera.main;
        cameraSecondAreaPosition = mCamera.transform.position + new Vector3(0f, 0f, 95f);//camera's second area position
        downedDoorPosition = door.transform.position + new Vector3(0f, -12.5f, 0f);//camera's second area position
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "NonEatableObstacles")
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().ReplayLevel();//if player eats nonEatableObject, level restarts
        }
        else
        {
            if (mCamera.transform.position.z == cameraFirstAreaPositionZ)//if camera is in the first area, eatableObjects are from first area
            {
                eatableCount--;
                if (eatableCount == 0)//if player eats all eatableObjects in the first area, location will change
                {
                    changeIsTrue = true;
                }
            }
            else if (mCamera.transform.position.z >= cameraSecondAreaPositionZ)//if camera is in the second area, eatableObjects are from second area
            {
                eatableCount2--;
                if (eatableCount2 == 0)
                {
                    GameObject.Find("GameManager").GetComponent<GameManager>().NextLevel();
                }
            }
        }
        Destroy(other.gameObject);//destroy eaten objects 
        if (GameObject.Find("GameManager").GetComponent<GameManager>().vibrationIsActive && vibrationTimer == 0)//if vibrate is activated in the settings and player eats objects, phone vibrates
        {
            Handheld.Vibrate();
            vibrationTimer = 2f;
        }
    }

    private void Update()
    {
        if (changeIsTrue && holeParent.transform.position.z != firstAreaPosition.z)//if hole's position is not first area position, move hole to the first area position in a specified time
        {
            GameObject.Find("HoleParent").GetComponent<OnChangePosition>().ActivateAutoControl();
            time += Time.deltaTime / timeToReachInArea1;
            holeParent.transform.position = Vector3.Lerp(holeParent.transform.position, firstAreaPosition, time);
            door.transform.position = Vector3.Lerp(door.transform.position, downedDoorPosition, time);
        }

        if ((holeParent.transform.position == firstAreaPosition) && changeIsTrue)//if hole reaches to the first area position, changes variables to start movement for the second area
        {
            movedFirstLocationIsTrue = true;
            changeIsTrue = false;
            time = 0f;//reset time to calculate correctly
        }

        if (movedFirstLocationIsTrue && holeParent.transform.position.z != secondAreaPosition.z)//if hole's position is not second area position, move hole to the second area position in a specified time
        {
            time += Time.deltaTime / timeToReachInArea2;
            holeParent.transform.position = Vector3.Lerp(holeParent.transform.position, secondAreaPosition, time);
            mCamera.transform.position = Vector3.Lerp(mCamera.transform.position, cameraSecondAreaPosition, time);
        }

        if ((holeParent.transform.position.z > (secondAreaPosition.z - 0.002f)) && movedFirstLocationIsTrue)//if hole reaches to the second area position, changes variables to stop automatic movement 
        {
            movedFirstLocationIsTrue = false;
            GameObject.Find("HoleParent").GetComponent<OnChangePosition>().DeactivateAutoControl();
        }

        if (vibrationTimer >= 0)
        {
            vibrationTimer -= Time.deltaTime;
        }
    }
}
