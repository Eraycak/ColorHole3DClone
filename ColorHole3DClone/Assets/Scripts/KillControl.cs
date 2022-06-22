using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillControl : MonoBehaviour
{

    private int eatableCount = 0;
    private int eatableCount2 = 0;

    private void Start()
    {
        eatableCount = GameObject.FindGameObjectsWithTag("EatableObjects").Length;
        eatableCount2 = GameObject.FindGameObjectsWithTag("EatableObjects2").Length;
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
}
