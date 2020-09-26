using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Waiter());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Waiter()
    {
        while(true)
        {
            StartCoroutine(Rotator(1, 90));
            yield return new WaitForSeconds(2f);
            StartCoroutine(Rotator(0.5f, -20));
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(Rotator(1, 110));
            yield return new WaitForSeconds(2f);
            StartCoroutine(Rotator(0.5f, -20));
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(Rotator(1, -160));
            yield return new WaitForSeconds(1.5f);
            /*
            StartCoroutine(Rotator(1, 20));
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(Rotator(1, 70));
            yield return new WaitForSeconds(2f);
            */
        }
    }

    IEnumerator Rotator(float timeToRotate, float totalAngle)
    {
        float rotateThreshold = 0.01f; //pick a number that divides timeToRotate
        int numberOfIterations = Mathf.RoundToInt(timeToRotate / rotateThreshold);
        float rotateAngle = totalAngle / (timeToRotate / rotateThreshold);
        if (rotateAngle < 0)
            rotateAngle *= -1;
        for (int i = 0; i < numberOfIterations; ++i)
        {
            transform.Rotate(new Vector3(0, totalAngle, 0), rotateAngle);
            yield return new WaitForSeconds(rotateThreshold);
        }
    }
}
