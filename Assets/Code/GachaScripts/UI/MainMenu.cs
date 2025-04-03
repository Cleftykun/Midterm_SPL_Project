using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartNewGame()
    {
        SceneManager.LoadScene("1");
    }

    public void ContinueGame()
    {
        int lastCompletedChapter = PlayerPrefs.GetInt("CompletedChapter", -1);
        if (lastCompletedChapter >= 0 && lastCompletedChapter < SceneManagerScript.Instance.chapterScenes.Length)
        {
            SceneManagerScript.Instance.LoadChapter(lastCompletedChapter + 1); // Load next chapter
        }
        else
        {
            Debug.Log("No saved progress. Starting a new game.");
            StartNewGame();
        }
    }

    public void QuitGame()
    {
        SceneManagerScript.Instance.QuitGame();
    }
}
