using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class PlayVideoOnCanvas : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Reference to the VideoPlayer component
    public RawImage rawImage;       // Reference to the RawImage component
    public string videoPath;        // Path to your video file (can be in the Resources folder or an URL)

    void Start()
    {
        if (videoPlayer != null && rawImage != null)
        {
            // Set up the VideoPlayer to use the RenderTexture
            RenderTexture renderTexture = new RenderTexture(1920, 1080, 0);
            videoPlayer.targetTexture = renderTexture;

            // Set the RawImage texture to the RenderTexture
            rawImage.texture = renderTexture;

            // Start playing the video
            videoPlayer.Play();
        }
    }
}
