using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PindahScene : MonoBehaviour
{   
    public void LoadScene(string tujuan)
    {
        SceneManager.LoadScene(tujuan);
    }
}