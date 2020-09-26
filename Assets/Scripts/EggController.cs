using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggController : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        PlayerManager.instance.AddFollower();
        gameObject.SetActive(false);
    }
}
