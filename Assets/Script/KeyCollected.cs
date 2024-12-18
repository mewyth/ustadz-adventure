using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCollected : MonoBehaviour
{
    public AudioSource KeyAudio;
    private void Start()
    {
    }

    public void OnTriggerEnter(Collider Col)
    {
        if (Col.CompareTag("Player"))
        {
            PlayerInventory playerInventory = Col.GetComponent<PlayerInventory>();
            KeyAudio.Play();

            if (playerInventory != null)
            {
                Debug.Log("Key Collected!");
                playerInventory.keyCollected();
                Destroy(gameObject);
            }
        }
        

    }
}
