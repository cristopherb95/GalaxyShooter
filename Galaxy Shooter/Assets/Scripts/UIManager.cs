using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Text _restartText;

    private GameManager _gameManager;
    private Player _player;

    [SerializeField]
    private Text _bestScoreText;
    public int bestScore;
    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0.ToString();
        bestScore = PlayerPrefs.GetInt("highscore", 0);
        _bestScoreText.text = "Best: " + bestScore.ToString();
        _gameOverText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();

        if (!_gameManager)
            Debug.LogError("Game Manager is NULL");
        if (!_player)
            Debug.LogError("Player is NULL");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void CheckForBestScore()
    {
        int score = _player.getPlayerScore();
        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("highscore", bestScore);
            _bestScoreText.text = "Best: " + bestScore.ToString();
        }
    }

    public void UpdateLives(int remainingLives)
    {
        _livesImage.sprite = _liveSprites[remainingLives];

        if (remainingLives == 0)
        {
            GameOverSequence();
        }
    }

    private void GameOverSequence()
    {
        _gameManager.GameOver();

        _gameOverText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
        _restartText.gameObject.SetActive(true);
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(1.0f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(1.0f);
        }
    }

    public void ResumePlay()
    {
        var gm = _gameManager.GetComponent<GameManager>();
        gm.UnPause();

    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
    }

}
