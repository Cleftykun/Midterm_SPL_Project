using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUi : MonoBehaviour
{
    public GameObject gameOverPanel; // Assign the Game Over UI panel in the Inspector

    public void ShowGameOver()
    {
        Time.timeScale = 0f; // Pause game
        gameOverPanel.SetActive(true);
    }

    public void PlayAgain()
    {
        Time.timeScale = 1f;
        Debug.Log(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the level
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
