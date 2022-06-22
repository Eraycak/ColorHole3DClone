using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillControl : MonoBehaviour
{
    private GameObject holeParent;
    private int eatableCount = 0;
    private int eatableCount2 = 0;
    private bool changeIsTrue = false;
    private bool movedFirstLocationIsTrue = false;
    private Vector3 firstAreaPosition = new Vector3(0, 0, 0);
    private Vector3 secondAreaPosition = new Vector3(0, 0, 65);

    private void Start()
    {
        eatableCount = GameObject.FindGameObjectsWithTag("EatableObjects").Length;
        eatableCount2 = GameObject.FindGameObjectsWithTag("EatableObjects2").Length;
        holeParent = GameObject.Find("HoleParent");
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
            if (Camera.main.transform.position.z == -48.4f)
            {
                eatableCount--;
            }
            else if (Camera.main.transform.position.z == 46.6f)
            {
                eatableCount2--;
            }
            if (eatableCount == 0)
            {
                changeIsTrue = true;
            }
            if (eatableCount2 == 0)
            {
                Debug.LogError("nextlevel");
                //GameObject.Find("GameManager").GetComponent<GameManager>().NextLevel();
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
        if (changeIsTrue && holeParent.transform.position != firstAreaPosition)
        {
            holeParent.transform.position = Vector3.Lerp(holeParent.transform.position, firstAreaPosition, Time.deltaTime);
        }

        if (holeParent.transform.position == firstAreaPosition)
        {
            movedFirstLocationIsTrue = true;
            changeIsTrue = false;
        }

        if (movedFirstLocationIsTrue && holeParent.transform.position != secondAreaPosition)
        {
            holeParent.transform.position = Vector3.Lerp(holeParent.transform.position, secondAreaPosition, Time.deltaTime);
        }
    }

}
