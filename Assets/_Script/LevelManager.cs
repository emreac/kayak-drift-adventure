using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public bool isUserCompleteLevel;
    private float score;

    // Start is called before the first frame update
    void Start()
    {
        int levelNumber = SceneManager.GetActiveScene().buildIndex;
        TinySauce.OnGameStarted("Level Number"+levelNumber);
    }

    // Update is called once per frame
    void Update()
    {
      
        
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void FirstLevel()
    {
        SceneManager.LoadScene("Level1");
    }
}
