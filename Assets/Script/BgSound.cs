using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;


public class BgSound : MonoBehaviour
{
    AudioSource audioBg;
    AudioClip audioBgClip;

    void Start(){
        audioBg = gameObject.AddComponent<AudioSource>();
    }
}
