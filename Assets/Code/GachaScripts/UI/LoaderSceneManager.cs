using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections;

public class LoaderSceneManager : MonoBehaviour
{
    private string nextSceneName;

    public void SetNextScene(string sceneName)
    {
        nextSceneName = sceneName;
    }

    void Start()
    {
        nextSceneName = PlayerPrefs.GetString("NextScene", string.Empty);

        VideoPlayer videoPlayer = GetComponentInChildren<VideoPlayer>();

        if (videoPlayer != null)
        {
            // Enable looping to simulate the loading process
            videoPlayer.playbackSpeed = 2f;
            videoPlayer.isLooping = true;  // Keep the video looping

            // Subscribe to the loop point reached event
            videoPlayer.loopPointReached += OnLoopPointReached;

            videoPlayer.Play(); // Start playing the video
        }
        else
        {
            UnityEngine.Debug.LogError("No VideoPlayer found in LoaderScene.");
        }
    }

    private void OnLoopPointReached(VideoPlayer vp)
    {
        // This will be called after the video completes a loop
        UnityEngine.Debug.Log("Video finished one loop, transitioning to the next scene.");

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            UnityEngine.Debug.Log($"Loading next scene: {nextSceneName}");
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            UnityEngine.Debug.LogError("Next scene name is not set.");
        }
    }
}
