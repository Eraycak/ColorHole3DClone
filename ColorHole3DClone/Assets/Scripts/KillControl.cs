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
    private bool done = false;
    private float time = 0;
    private float timeToReach = 1000f;

    private void Start()
    {
        eatableCount = GameObject.FindGameObjectsWithTag("EatableObjects").Length;
        eatableCount2 = GameObject.FindGameObjectsWithTag("EatableObjects2").Length;
        holeParent = GameObject.Find("HoleParent");
        mCamera = Camera.main;
        cameraSecondAreaPosition = mCamera.transform.position + new Vector3(0, 0, 95);
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
        time += Time.deltaTime / timeToReach;
        Debug.LogError("change " + changeIsTrue + " moved " + movedFirstLocationIsTrue + " done " + done);
        if (changeIsTrue && holeParent.transform.position.z != firstAreaPosition.z)
        {
            holeParent.transform.position = Vector3.Lerp(holeParent.transform.position, firstAreaPosition, time);
        }

        if (holeParent.transform.position == firstAreaPosition && !done)
        {
            movedFirstLocationIsTrue = true;
            changeIsTrue = false;
        }

        if (movedFirstLocationIsTrue && holeParent.transform.position.z != secondAreaPosition.z)
        {
            holeParent.transform.position = Vector3.Lerp(holeParent.transform.position, secondAreaPosition, time);
            mCamera.transform.position = Vector3.Lerp(mCamera.transform.position, cameraSecondAreaPosition, time);
        }

        if (holeParent.transform.position == secondAreaPosition)
        {
            movedFirstLocationIsTrue = false;
            done = true;
        }
    }
}
