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
        Cursor.lockState= CursorLockMode.None;
        gameOverPanel.SetActive(true);
    }
    public void RestartGame()
    {
        Debug.Log("11");
        Time.timeScale = 1f;
        isGameOver = false;
        gameOverPanel.SetActive(false);
        SceneManager.LoadScene("Level");
    }

}
