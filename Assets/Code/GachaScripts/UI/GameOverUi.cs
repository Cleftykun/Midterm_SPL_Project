using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverUi : MonoBehaviour
{
    public GameObject gameOverPanel;
    
    public void ShowGameOver()
    {
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
    }

    public void PlayAgain()
    {
        Time.timeScale = 1f;
        string currentScene = SceneManager.GetActiveScene().name;

        PlayerPrefs.SetString("NextScene", currentScene);
        StartCoroutine(LoadLoaderScene());
    }

    private IEnumerator LoadLoaderScene()
    {
        SceneManager.LoadScene("LoaderScene");

        while (SceneManager.GetActiveScene().name != "LoaderScene")
        {
            yield return null;
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }
}
