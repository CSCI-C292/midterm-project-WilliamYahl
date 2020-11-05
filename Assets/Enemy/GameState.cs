using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    [SerializeField] GameObject _spawnee;
    [SerializeField] GameObject _scoreText;
    [SerializeField] GameObject _gameOverText;

    bool gameOver = false;
    int score = 0;

    public static GameState instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        InvokeRepeating("SpawnEnemy", 0, 10);
    }

    private void Update()
    {
        if (gameOver && Input.GetButtonDown("Submit"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void SpawnEnemy()
    {
        float randX = Random.Range(-5f, 5f);
        Instantiate(_spawnee, new Vector3(randX, 6, -1), Quaternion.identity);
    }

    public void IncreaseScore()
    {
        score++;
        _scoreText.GetComponent<Text>().text = "Enemies Defeated: " + score;
        Debug.Log(score);
    }

    public void GameOver()
    {
        gameOver = true;
        _gameOverText.SetActive(true);
        _gameOverText.GetComponent<Text>().text = "Game Over\nEnemies Defeated: " + score;
        _scoreText.SetActive(false);
    }
}
