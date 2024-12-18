using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicscript : MonoBehaviour
{
    [Header("-------- Audio Source --------")]
    [SerializeField] AudioSource MusicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("-------- Audio Clip --------")]
    public AudioClip background;
   private void Start()
    {
        MusicSource.clip = background;
        MusicSource.Play();
    }

    
}
