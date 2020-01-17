using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    [SerializeField] GeneralShip player = null;
    [SerializeField] float loadTime = 2f;
    // Start is called before the first frame update

    int nextScene = 0;
    void Start()
    {
        nextScene = GetNextSceneIndex();
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
        
        SceneManager.LoadScene(nextScene);
    }

    private void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
    }

    private void ProcessPlayer()
    {
        if (player.IsDying())
        {
            Invoke("LoadFirstScene", loadTime);
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

            
            Invoke("LoadNextScene", loadTime);
            player.SetTranscending();
        }
    }

    private int GetNextSceneIndex()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        int nextScene = index+1;
        if (nextScene > SceneManager.sceneCountInBuildSettings)
        {
            nextScene = 0;
        }
        return nextScene;
    }
}
