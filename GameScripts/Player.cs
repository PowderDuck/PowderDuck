//using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public delegate void SlideAction(Vector2 slideDirection);

    [SerializeField] private Vector2 movementSpeedRange = new Vector2(5f, 10f);
    public float progress = 0f;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private float yRotationRange = 15f;
    [SerializeField] private float minDragDistance = 50f;
    [SerializeField] private Vector3 gravityDirection = -Vector3.up;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float sideTransitionDuration = 0.5f;
    [SerializeField] private float runningLines = 3f;
    [SerializeField] private float lineDisplacement = 4f;
    [SerializeField] private float sideRotationThreshold = 0.2f;
    [SerializeField] private GameObject playerSkin;
    [SerializeField] private List<Collider> playerColliders = new List<Collider>();
    [SerializeField] private AnimationClip rollAnimation;
    [SerializeField] private float maxProgressDuration = 180f;

    [Header("JumpSettings\n")]
    [SerializeField] private float verticalLeap = 5f;
    [SerializeField] private Vector2 attractionForce = new Vector2(-20f, -50f);
    [SerializeField] private AnimationCurve gravityCurve;
    [SerializeField] private float jumpTransitionDuration = 0.5f;
    [SerializeField] private GameObject groundCheck;
    [SerializeField] private float groundCheckRadius = 2f;

    [Header("CameraSettings\n")]
    public bool controlCamera = true;
    [SerializeField] private Vector3 cameraOffset = new Vector3(0f, 5f, -10f);
    [SerializeField] private Vector3 cameraRotation = new Vector3(10f, 0f, 0f);
    [SerializeField] private float regainControlTransitionDuration = 1f;
    [SerializeField] private AnimationCurve transitionCurve;

    private bool dragging = false;
    private Vector2 clickMousePosition = Vector2.zero;
    private Rigidbody rb;
    private MapController mapController;
    private Dictionary<Vector2, SlideAction> actions = new Dictionary<Vector2, SlideAction>();
    [HideInInspector] public float movementSpeed = 0f;
    private List<Multiplier> speedMultipliers = new List<Multiplier>();

    private float currentTransitionTime = 0f;
    private float currentSideTransitionTime = 0f;
    private float currentLine = 0f;
    private float sideDirection = 1f;
    private float sideVelocity = 0f;
    private Vector3 initialPlayerRotation = Vector3.zero;
    private Vector3 checkpointCameraPosition = Vector3.zero;
    private Vector3 checkpointCameraRotation = Vector3.zero;
    private float currentPlayTime = 0f;

    private const int groundLayer = 128;
    private float verticalVelocity = -9.81f;
    private bool previousGround = true;
    private float currentJumpTime = 0f;
    private float gravityMultiplier = 1f;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        mapController = FindObjectOfType<MapController>();
        currentTransitionTime = regainControlTransitionDuration;
        currentSideTransitionTime = sideTransitionDuration;
        currentLine = Mathf.Floor(runningLines / 2f);
        sideVelocity = lineDisplacement / sideTransitionDuration;
        initialPlayerRotation = playerSkin.transform.eulerAngles;
        playerAnimator.SetBool("Flying", true);
        playerAnimator.SetTrigger("Dancing");
        playerAnimator.SetFloat("DancingInter", Random.Range(0f, 1f));
        verticalVelocity = gravity;
        previousGround = true;

        rollAnimation.AddEvent(new AnimationEvent()
        {
            functionName = "SetCollider",
            intParameter = 0,
            time = 0.933f
        });
        
        InitializeActions();
    }

    private void InitializeActions()
    {
        actions.Add(new Vector2(1f, 0f), HorizontalSlide);
        actions.Add(new Vector2(0f, 1f), VerticalSlide);
    }

    private void HorizontalSlide(Vector2 slideDirection)
    {
        float checkpoint = Mathf.Min(Mathf.Max(0f, currentLine + Mathf.Sign(slideDirection.x)), runningLines - 1f);
        float difference = checkpoint - currentLine;

        if(Mathf.Abs(difference) > 0f)
        {
            sideDirection = Mathf.Sign(difference);
            //currentTransitionTime = 0f;
            currentLine = checkpoint;
            currentSideTransitionTime = 0f;

            /*AnimatorClipInfo[] currentClips = playerAnimator.GetCurrentAnimatorClipInfo(0);
            string currentAnimation = currentClips.Length > 0f ? currentClips[currentClips.Length - 1].clip.name : "";

            if(currentAnimation == "Running")
            {
                playerAnimator.SetTrigger("Dash");
            }

            currentLine = checkpoint;*/
        }

        Debug.Log("HorizontalSlide;");
    }

    private void SetCollider(int colliderIndex)
    {
        for (int i = 0; i < playerColliders.Count; i++)
        {
            playerColliders[i].enabled = i == colliderIndex;
        }
    }

    private void VerticalSlide(Vector2 slideDirection)
    {
        string animationName = slideDirection.y > 0f ? "Jump" : "Slide";
        bool action = slideDirection.y > 0f ? Jump() : Slide();
        playerAnimator.SetTrigger(animationName);
        SetCollider(slideDirection.y > 0f ? 0 : 1);
        Debug.Log("VerticalSlide;");
    }

    private bool Jump()
    {
        bool ground = Physics.OverlapCapsule(transform.position, groundCheck.transform.position, groundCheckRadius, groundLayer).Length > 0f;
        gravityMultiplier = 1f;

        if(ground)
        {
            playerAnimator.SetTrigger("Jump");
            playerAnimator.ResetTrigger("JumpLanding");
            verticalVelocity = verticalLeap;
            currentJumpTime = 0f;
        }

        return true;
    }

    private bool Slide()
    {
        currentJumpTime = jumpTransitionDuration;
        gravityMultiplier = previousGround ? 1f : 2f;

        return true;
    }

    private void Land()
    {
        if(currentJumpTime < jumpTransitionDuration || !previousGround)
        {
            currentJumpTime += Time.deltaTime;
            currentJumpTime = Mathf.Min(currentJumpTime, jumpTransitionDuration);

            float gravityForce = Mathf.Lerp(attractionForce.x, attractionForce.y * gravityMultiplier, gravityCurve.Evaluate(currentJumpTime / jumpTransitionDuration));

            verticalVelocity += gravityForce * Time.deltaTime;
            //verticalVelocity = Mathf.Max(verticalVelocity, gravity);

            bool ground = Physics.OverlapCapsule(transform.position, groundCheck.transform.position, groundCheckRadius, groundLayer).Length > 0f;

            if(ground && !previousGround)
            {
                playerAnimator.SetTrigger("JumpLanding");
                verticalVelocity = gravity;
                currentJumpTime = jumpTransitionDuration;
            }

            previousGround = ground;
        }    
    }

    private void Update()
    {
        ProgressTimer();
        Sider();
        SideTransition();
        Land();
        Movement();
        CameraManagement();
    }

    private void ProgressTimer()
    {
        if(currentPlayTime < maxProgressDuration)
        {
            currentPlayTime += Time.deltaTime;
            currentPlayTime = Mathf.Min(currentPlayTime, maxProgressDuration);
        }

        float maxValue = 0f;
        for (int i = 0; i < speedMultipliers.Count; i++)
        {
            maxValue = Mathf.Max(maxValue, speedMultipliers[i].multiplier);
        }

        progress = Mathf.Max(maxValue, currentPlayTime / maxProgressDuration);
    }

    public void AddSpeedMultiplier(Multiplier sMultiplier, bool add)
    {
        if(add)
        {
            if(!speedMultipliers.Contains(sMultiplier))
            {
                speedMultipliers.Add(sMultiplier);
            }
        }
        else
        {
            speedMultipliers.Remove(sMultiplier);
        }
    }

    public void StartGame()
    {
        playerAnimator.SetTrigger("EndDancing");
    }


    private void CameraManagement()
    {
        if(controlCamera)
        {
            if(currentTransitionTime < regainControlTransitionDuration)
            {
                currentTransitionTime += Time.deltaTime;
                currentTransitionTime = Mathf.Min(currentTransitionTime, regainControlTransitionDuration);
            }

            float transitionPercentage = transitionCurve.Evaluate(currentTransitionTime / regainControlTransitionDuration);

            Camera.main.transform.position = Vector3.Lerp(checkpointCameraPosition, transform.position + mapController.worldRight * cameraOffset.x + mapController.worldUp * cameraOffset.y + mapController.worldDirection * cameraOffset.z, transitionPercentage);
            Camera.main.transform.eulerAngles = Vector3.Lerp(checkpointCameraRotation, cameraRotation, transitionPercentage);
        }
    }
    private void Movement()
    {
        movementSpeed = Mathf.Lerp(movementSpeedRange.x, movementSpeedRange.y, progress);
        Vector3 sideMovement = mapController.worldRight * sideDirection * (1f - Mathf.Floor(currentSideTransitionTime / sideTransitionDuration)) * sideVelocity; //Speed;
        rb.velocity = mapController.worldDirection * movementSpeed + mapController.worldUp * verticalVelocity + sideMovement;
        //transform.rotation = Quaternion.LookRotation(mapController.worldDirection, -gravityDirection.normalized);
        playerAnimator.SetFloat("Running", progress);
    }

    private void SideTransition()
    {
        if(currentSideTransitionTime < sideTransitionDuration)
        {
            currentSideTransitionTime += Time.deltaTime;
            currentSideTransitionTime = Mathf.Min(currentSideTransitionTime, sideTransitionDuration);

            float rotationPercentage = Mathf.Min(Mathf.Clamp01((currentSideTransitionTime / sideTransitionDuration) / sideRotationThreshold), Mathf.Clamp01((1f - (currentSideTransitionTime / sideTransitionDuration)) / sideRotationThreshold));
            float yRotation = Mathf.Lerp(0f, sideDirection * yRotationRange, rotationPercentage);
            playerSkin.transform.eulerAngles = new Vector3(initialPlayerRotation.x, yRotation, initialPlayerRotation.z);

            if(currentSideTransitionTime >= sideTransitionDuration)
            {
                sideDirection = 0f;
            }
        }
    }

    private void Sider()
    {
        if(currentSideTransitionTime >= sideTransitionDuration)
        {
            if (Input.GetMouseButtonDown(0))
            {
                dragging = true;
                clickMousePosition = Input.mousePosition;
                StartGame();
            }

            dragging = Input.GetMouseButtonUp(0) ? false : dragging;

            Vector2 dynamicMousePosition = Input.mousePosition;
            Vector2 mouseDirection = dynamicMousePosition - clickMousePosition;

            Vector2 slideDirection = new Vector2(Mathf.Clamp01(Mathf.Floor(Mathf.Abs(mouseDirection.x) / minDragDistance)) * Mathf.Sign(mouseDirection.x), Mathf.Clamp01(Mathf.Floor(Mathf.Abs(mouseDirection.y) / minDragDistance)) * Mathf.Sign(mouseDirection.y));
        
            if(dragging && slideDirection.magnitude >= 1f)
            {
                Vector2 key = new Vector2(Mathf.Abs(slideDirection.x), Mathf.Abs(slideDirection.y));
                if (actions.ContainsKey(key))
                {
                    actions[key](slideDirection);
                }
            }

            dragging = slideDirection.magnitude >= 1f ? false : dragging;
        }
    }

    public void RegainCameraControl()
    {
        controlCamera = true;
        currentTransitionTime = 0f;
        checkpointCameraPosition = Camera.main.transform.position;
        checkpointCameraRotation = Camera.main.transform.eulerAngles;
    }
}
