using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public static SceneManagerScript Instance;

    [Header("Scene Indexes")]
    [SerializeField] private int mainMenuIndex = 0;
    [SerializeField] public int[] chapterScenes;

    private int currentChapter = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenuIndex);
    }

    public void LoadChapter(int chapterIndex)
    {
        if (chapterIndex >= 0 && chapterIndex < chapterScenes.Length)
        {
            currentChapter = chapterIndex;
            SceneManager.LoadScene(chapterScenes[chapterIndex]);
        }
        else
        {
            Debug.LogError("Invalid chapter index.");
        }
    }

    public void LoadNextChapter()
    {
        int nextChapter = currentChapter + 1;
        if (nextChapter < chapterScenes.Length)
        {
            currentChapter = nextChapter;
            SceneManager.LoadScene(chapterScenes[nextChapter]);
        }
        else
        {
            Debug.Log("No more chapters available.");
        }
    }

    public void RestartChapter()
    {
        SceneManager.LoadScene(chapterScenes[currentChapter]);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit"); // Debug log for editor testing
    }
}
