using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        Screen.SetResolution(1280, 720, false);
    }

    public void GameStartBtn()
    {
        SceneManager.LoadScene("Game");
    }
}
