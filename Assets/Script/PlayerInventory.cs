using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    public UnityEvent<PlayerInventory> getKey;
    public int keys;
    public AudioSource audio;

    public void keyCollected()
    {
        keys++;
        getKey.Invoke(this);
        audio.Play();
    }
}
