using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver;

    [SerializeField]
    private GameObject _pauseMenuPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver)
        {
            SceneManager.LoadScene(1); // Current Game Scene
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Time.timeScale == 1)
            {
                Pause();
            }
            else
            {
                UnPause();
            }
        }
    }

    public void GameOver()
    {
        _isGameOver = true;
    }

    public void Pause()
    {
        Time.timeScale = 0;
        _pauseMenuPanel.SetActive(true);
    }
    public void UnPause()
    {
        Time.timeScale = 1;
        _pauseMenuPanel.SetActive(false);
    }
}
