using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Groundcheck : MonoBehaviour
{
    PlayerLogic playerLogic;

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Touch the Ground");
        playerLogic.groundedChanger();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerLogic = this.GetComponentInParent<PlayerLogic>();
    }

}
