using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    private bool isGameOver=false;
    //
    public void GameOver()
    {
        Time.timeScale = 0;
        isGameOver = true;
        gameOverPanel.SetActive(true);
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        isGameOver = false;
        gameOverPanel.SetActive(false);
        SceneManager.LoadScene("Level");
    }

}
