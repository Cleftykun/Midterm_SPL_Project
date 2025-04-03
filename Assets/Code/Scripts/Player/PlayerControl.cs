using Spine;
using UnityEngine;
using Spine.Unity;
using System.Collections;
using System;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Adjust speed
    private float speed;
    private Rigidbody2D rb;
    private Vector2 movement;
    private SkeletonAnimation skeletonAnimation;
    private Coroutine idleCoroutine;
    private Spine.AnimationState animationstate;

    [Header("Camera Control")]
    public Camera playerCamera;
    public float zoomDuration = 1;
    public float zoomInSize = 0.5f;
    public float zoomOutSize = 2f;

    private Coroutine zoomCoroutine;

    [Header("Test")]
    public PlayerCharacter player;
    bool isPhoneOpen = false;
    void Start()
    {
        speed = moveSpeed;

        rb = GetComponent<Rigidbody2D>();

        skeletonAnimation = GetComponent<SkeletonAnimation>(); // Get Spine Animation

        if (skeletonAnimation == null)
        {
            Debug.LogError("SkeletonAnimation component is missing on " + gameObject.name);
            return;
        }

        animationstate = skeletonAnimation.AnimationState;

        if (animationstate == null)
        {
            Debug.LogError("AnimationState is null on " + gameObject.name);
            return;
        }

        animationstate.SetAnimation(0, "[face]idle", true);
        animationstate.Event += HandleAnimationEvent;
    }

    void Update()
    {
        // Get movement input
        movement.x = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
        movement.y = Input.GetAxisRaw("Vertical");   // W/S or Up/Down

        // Get current animation
        Spine.TrackEntry currentAnimation = animationstate.GetCurrent(0);

        if (movement.magnitude > 0)
        {
            // Cancel any existing idle transition coroutine
            if (idleCoroutine != null)
            {
                StopCoroutine(idleCoroutine);
                idleCoroutine = null;
            }

            // Only set "walk" animation if it's not already playing
            if (currentAnimation == null || currentAnimation.Animation.Name != "walk")
            {
                animationstate.SetAnimation(0, "walk", true);
                animationstate.SetAnimation(1, "[face]attack", true);
            }

            // Flip sprite based on direction
            skeletonAnimation.skeleton.ScaleX = movement.x < 0 ? -1 : (movement.x > 0 ? 1 : skeletonAnimation.skeleton.ScaleX);
        }
        else
        {
            // Only set "idle" if not already playing
            if (currentAnimation == null || currentAnimation.Animation.Name != "idle")
            {
                animationstate.SetAnimation(0, "idle", false);
                if (idleCoroutine == null)
                {
                    idleCoroutine = StartCoroutine(SetIdleAfterDelay(0.5f));
                }
            }
        }
    }

    private const string IdleFace = "[face]idle";
    private const string AttackFace = "[face]attack";
    private const string HurtFace = "[face]damaged";
    private const string ClearFace = "clear";
    public void Face(String FaceExpression)
    {
        animationstate.SetAnimation(1, FaceExpression, true);
    }

    public void FacePrio(String FaceExpression)
    {
        if (FaceExpression == "clear")
            animationstate.ClearTrack(3);
        else
            animationstate.SetAnimation(3, FaceExpression, true);
    }

    void FixedUpdate()
    {
        // Apply movement
        rb.linearVelocity = movement.normalized * moveSpeed;
    }

    IEnumerator SetIdleAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        //skeletonAnimation.AnimationName = "reload"; // Switch to idle animation
        animationstate.SetAnimation(0, "reload", false);
        animationstate.AddAnimation(1, "[face]idle", true, 0f);
        idleCoroutine = null; // Reset coroutine reference
    }


    public void PhoneEquip()
    {
        Spine.TrackEntry current = animationstate.GetCurrent(4);
        if (current == null || current.Animation.Name != "phone_out")
        {
            animationstate.SetAnimation(4, "phone_out", false);
            animationstate.AddAnimation(2, "[face]idle", true, 0.5f);
            // StartZoom(zoomInSize);
        }
    }
    public void PhoneEquipSide()
    {
        Spine.TrackEntry current = animationstate.GetCurrent(4);
        if (current == null || current.Animation.Name != "phone_landscape")
        {
            animationstate.SetAnimation(4, "phone_landscape", false);
            animationstate.AddAnimation(2, "[face]idle", true, 0.5f);
            // StartZoom(zoomInSize);
        }
    }

    public void PhoneUnEquip()
    {

        // StartZoom(zoomOutSize);
        StartCoroutine(FadeOutTrack(4, 0.2f));

        animationstate.ClearTrack(2);
    }

    private void StartZoom(float targetZoom)
    {
        if (zoomCoroutine != null)
            StopCoroutine(zoomCoroutine); // Stop smoothly instead of snapping

        zoomCoroutine = StartCoroutine(ZoomCamera(targetZoom));

    }

    // 🔥 Improved Smooth Zoom Transition
    private IEnumerator ZoomCamera(float targetZoom)
    {
        float startZoom = playerCamera.orthographicSize;
        float elapsedTime = 0f;

        while (elapsedTime < zoomDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / zoomDuration;
            t = 1f - Mathf.Pow(2f, -10f * t); // Exponential ease-out

            // Interpolating smoothly between the previous zoom and the new target
            playerCamera.orthographicSize = Mathf.Lerp(startZoom, targetZoom, t);

            yield return null;
        }

        playerCamera.orthographicSize = targetZoom; // Ensure exact target is reached
        zoomCoroutine = null; // Reset coroutine tracker
    }
    public void Build()
    {

        Spine.TrackEntry trackEntry = animationstate.SetAnimation(5, "build_tower", false);
        trackEntry.MixDuration = 0.5f;
        StartCoroutine(FadeInTrack(trackEntry, 0.2f));
        trackEntry.Complete += delegate
        {
            StartCoroutine(FadeOutTrack(5, 0.2f));
        };
    }

    public void Cast()
    {
        moveSpeed = 0;
        FacePrio(AttackFace);
        Spine.TrackEntry trackEntry = animationstate.SetAnimation(5, "aoe_cast", false);
        trackEntry.Alpha = 0f;
        StartCoroutine(FadeInTrack(trackEntry, 0.5f));
        trackEntry.MixDuration = 0.5f;

        trackEntry.Complete += delegate
        {

            StartCoroutine(FadeOutTrack(5, 0.2f));
            FacePrio(ClearFace);
            moveSpeed = speed;
        };
    }

    private void HandleAnimationEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name == "cast")
        {
            player.EmitDecipheringWave();
        }
    }



    private IEnumerator FadeInTrack(Spine.TrackEntry trackEntry, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float fadeAmount = elapsedTime / duration; // Increase alpha over time
            trackEntry.Alpha = fadeAmount;
            yield return null;
        }

        trackEntry.Alpha = 1f; // Ensure full visibility
    }
    private IEnumerator FadeOutTrack(int track, float duration)
    {
        float elapsedTime = 0f;
        Spine.TrackEntry trackEntry = animationstate.GetCurrent(track);

        if (trackEntry == null) yield break;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float fadeAmount = 1f - (elapsedTime / duration);
            trackEntry.Alpha = fadeAmount;
            yield return null;
        }
        if (track == 4)
            skeletonAnimation.skeleton.FindSlot("side/phone").Attachment = null;
        animationstate.ClearTrack(track);
    }

    void OnEnable()
    {
        Plot.onBuild += Build;
        KeybindManager.OnGacha += PhoneEquipSide;
        KeybindManager.OnMessages += PhoneEquip;
        KeybindManager.OnAbility1 += Cast;
        KeybindManager.OnAbility2 += null;//soon
    }
    void OnDisable()
    {
        Plot.onBuild -= Build;
        KeybindManager.OnGacha -= PhoneEquipSide;
        KeybindManager.OnMessages -= PhoneEquip;
        KeybindManager.OnAbility1 -= Cast;
        KeybindManager.OnAbility2 -= null;//soon
    }
}
