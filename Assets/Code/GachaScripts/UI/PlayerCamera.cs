using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform player;

    public Vector3 defaultOffset = new Vector3(0, 0, 0);
    public Vector3 phoneOffset = new Vector3(3, 0, 0);

    private Vector3 currentOffset;
    private Vector3 velocity = Vector3.zero; // For smooth UI transition

    public float uiSmoothTime = 0.3f; // Adjust for UI smoothness

    private bool isPhoneOpen = false;

    public Camera playerCamera;
    public float defaultZoom = 5f;
    public float phoneZoom = 3.5f;
    private float currentZoom;
    private float zoomVelocity = 0f;

    void Start()
    {
        currentOffset = defaultOffset;
        currentZoom = defaultZoom;
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Instantly track the player, NO delay
        Vector3 basePosition = player.position;

        // Smoothly adjust only the offset and zoom
        currentOffset = Vector3.SmoothDamp(currentOffset, isPhoneOpen ? phoneOffset : defaultOffset, ref velocity, uiSmoothTime);
        currentZoom = Mathf.SmoothDamp(currentZoom, isPhoneOpen ? phoneZoom : defaultZoom, ref zoomVelocity, uiSmoothTime);

        // Apply updated camera position & zoom
        transform.position = basePosition + currentOffset;
        playerCamera.orthographicSize = currentZoom;
    }


    void OnEnable()
    {
        PhoneManager.PhoneOpened += () => SetPhoneState(true);
        PhoneManager.PhoneClosed += () => SetPhoneState(false);
    }
    void OnDisable()
    {
        PhoneManager.PhoneOpened -= () => SetPhoneState(true);
        PhoneManager.PhoneClosed -= () => SetPhoneState(false);
    }

    void SetPhoneState(bool isOpen)
    {
        isPhoneOpen = isOpen;
    }


}
