using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startGame : MonoBehaviour
{
    public void LoadPlay(){
        SceneManager.LoadScene("save1");
    }

    public void LoadRules(){
        SceneManager.LoadScene("scene3");
    }
    
}
