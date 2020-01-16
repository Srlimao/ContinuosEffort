using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    [SerializeField] GeneralShip player = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
        ProcessPlayer();


    }

    void ProcessInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ChangeLevel(0);
        }
        
    }

    private void ChangeLevel(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }

    private void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
    }

    private void ProcessPlayer()
    {
        if (player.IsDying())
        {
            Invoke("LoadFirstScene", 1);
            player.SetTranscending();
        }
        else
        if (player.IsAlive())
        {

        }
        else
        if (player.IsTranscending())
        {

        }
        else
        if (player.IsWinning())
        {
            Invoke("LoadNextScene", 1);
            player.SetTranscending();
        }
    }
}
