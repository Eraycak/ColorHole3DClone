using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillControl : MonoBehaviour
{
    private GameObject holeParent;
    private Camera mCamera;
    private int eatableCount = 0;
    private int eatableCount2 = 0;
    private bool changeIsTrue = false;
    private bool movedFirstLocationIsTrue = false;
    private Vector3 firstAreaPosition = new Vector3(0f, 0f, 0f);
    private Vector3 secondAreaPosition = new Vector3(0f, 0f, 65f);
    private Vector3 cameraSecondAreaPosition;
    private float time = 0f;
    private float timeToReachInArea1 = 100f;
    private float timeToReachInArea2 = 250f;

    private void Start()
    {
        eatableCount = GameObject.FindGameObjectsWithTag("EatableObjects").Length;
        eatableCount2 = GameObject.FindGameObjectsWithTag("EatableObjects2").Length;
        holeParent = GameObject.Find("HoleParent");
        mCamera = Camera.main;
        cameraSecondAreaPosition = mCamera.transform.position + new Vector3(0f, 0f, 95f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag== "NonEatableObstacles")
        {
            Debug.LogError("gameover");
            //GameObject.Find("GameManager").GetComponent<GameManager>().ReplayLevel();
        }
        else
        {
            if (mCamera.transform.position.z == -48.4f)
            {
                eatableCount--;
                if (eatableCount == 0)
                {
                    changeIsTrue = true;
                }
            }
            else if (mCamera.transform.position.z == 46.6f)
            {
                eatableCount2--;
                if (eatableCount2 == 0)
                {
                    Debug.LogError("nextlevel");
                    //GameObject.Find("GameManager").GetComponent<GameManager>().NextLevel();
                }
            }
        }
        Destroy(other.gameObject);
        if (GameObject.Find("GameManager").GetComponent<GameManager>().vibrationIsActive)
        {
            Handheld.Vibrate();
            Debug.LogError("vibrated");
        }
    }

    private void Update()
    {
        if (changeIsTrue && holeParent.transform.position.z != firstAreaPosition.z)
        {
            GameObject.Find("HoleParent").GetComponent<OnChangePosition>().ActivateAutoControl();
            time += Time.deltaTime / timeToReachInArea1;
            holeParent.transform.position = Vector3.Lerp(holeParent.transform.position, firstAreaPosition, time);
        }

        if ((holeParent.transform.position == firstAreaPosition) && changeIsTrue)
        {
            movedFirstLocationIsTrue = true;
            changeIsTrue = false;
            time = 0f;
        }

        if (movedFirstLocationIsTrue && holeParent.transform.position.z != secondAreaPosition.z)
        {
            time += Time.deltaTime / timeToReachInArea2;
            holeParent.transform.position = Vector3.Lerp(holeParent.transform.position, secondAreaPosition, time);
            mCamera.transform.position = Vector3.Lerp(mCamera.transform.position, cameraSecondAreaPosition, time);
        }

        if ((holeParent.transform.position.z > (secondAreaPosition.z - 0.002f)) && movedFirstLocationIsTrue)
        {
            movedFirstLocationIsTrue = false;
            GameObject.Find("HoleParent").GetComponent<OnChangePosition>().DeactivateAutoControl();
        }
    }
}
