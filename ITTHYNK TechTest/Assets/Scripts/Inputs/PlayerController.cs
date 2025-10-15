using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    public static PlayerController Instance;

    [Header ("Debug")]
    [SerializeField] Vector2 moveInput;

    [SerializeField] Vector3 moveDirection;
    [SerializeField] Vector2 lookInput;
    [SerializeField] bool isMoving;
    [SerializeField] bool isDashing;

    Vector3 cubeRotate;

    [Header ("References")]
    //
    //Player References
    //
    [SerializeField] GameObject playerGameObject;

    [SerializeField] Rigidbody playerRigidbody;

    [SerializeField] Transform playerTransform;

    //
    //Camera References
    //
    [SerializeField] Transform cameraTransform;

    [SerializeField] CinemachineVirtualCamera virtualCamera;

    //
    //Cube References
    //
    [SerializeField] GameObject cubeGameObject;
    [SerializeField] Transform cubeTransform;
    [SerializeField] Rigidbody cubeRigidbody;
    Vector3 cubeRotation;

    public PlayerInput playerInput;
    InputHandler inputHandler;

    TechInputs techInputs;

    [Header ("Settings")]
    [SerializeField] float moveSpeed;

    [SerializeField] float dashSpeed;
    [SerializeField] float rotationSpeed;
    public float mouseSensitivityHori;
    public float mouseSensitivityVert;
    public float controllerSensitivity;
    [SerializeField] float dashTimer;
    [SerializeField] float dashMax;

    /// <summary>
    /// Awake functions that assign components on the script's game object, as well as assign the player inputs
    /// </summary>
    void Awake () {
        Instance = this;
        playerInput = this?.GetComponent<PlayerInput> ();
        playerTransform = this?.GetComponent<Transform> ();
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera> ();
        if (playerInput == null) {
            playerInput = new PlayerInput ();
        }

        if (inputHandler == null) {
            inputHandler = new InputHandler ();
        }

        if (techInputs == null) {
            techInputs = InputHandler.techInputs;
        }
    }

    /// <summary>
    /// Player input event listners assignment and removal
    /// </summary>
    public void OnEnable () {
        InputHandler.Enable ();
        InputHandler.OnMovePerformed.AddListener (InputMove);
        InputHandler.OnDashPerformed.AddListener (InputDash);
        InputHandler.OnLookPerformed.AddListener (InputLook);
    }

    public void OnDisable () {
        InputHandler.OnMovePerformed.RemoveListener (InputMove);
        InputHandler.OnDashPerformed.RemoveListener (InputDash);
        InputHandler.OnLookPerformed.RemoveListener (InputLook);
    }

    public void OnDestroy () {
        InputHandler.OnMovePerformed.RemoveListener (InputMove);
        InputHandler.OnDashPerformed.RemoveListener (InputDash);
        InputHandler.OnLookPerformed.RemoveListener (InputLook);
    }

    /// <summary>
    /// Fixed update used for physics updates and timer
    /// </summary>
    void FixedUpdate () {
        if (moveInput != Vector2.zero) {
            isMoving = true;
        }

        OnPlayerMove ();
        cubeTransform.localPosition = Vector3.zero;
        OnPlayerLook ();
        if (dashTimer > 0) {
            dashTimer -= Time.fixedDeltaTime;
        }

        if (dashTimer <= 0 && isDashing) {
            isDashing = false;
        }
    }

    /// <summary>
    /// Move Input for event
    /// </summary>
    /// <param name="arg0"></param>
    void InputMove (Vector2 arg0) {
        moveInput = arg0;
    }

    /// <summary>
    /// Movement function
    /// </summary>
    void OnPlayerMove () {
        //Debug.Log ($"moveInput {moveInput}");
        moveDirection = moveInput.x * playerTransform.right + moveInput.y * playerTransform.forward;
        cubeRotate = (playerTransform.right * moveInput.y + playerTransform.forward * -moveInput.x);

        if (!isDashing) {
            playerRigidbody.velocity = isMoving ? new Vector3 (moveDirection.x * moveSpeed, playerRigidbody.velocity.y, moveDirection.z * moveSpeed) : Vector3.zero;
            cubeRigidbody.angularVelocity = cubeRotate * rotationSpeed * moveSpeed;
        } else {
            playerRigidbody.velocity = isMoving ? new Vector3 (moveDirection.x * dashSpeed, playerRigidbody.velocity.y, moveDirection.z * dashSpeed) : Vector3.zero;
            cubeRigidbody.angularVelocity = cubeRotate * rotationSpeed * dashSpeed;
        }
    }

    /// <summary>
    /// Dash input for event
    /// </summary>
    void InputDash () {
        if (!isDashing) {
            isDashing = true;
            dashTimer = dashMax;
        }
    }

    /// <summary>
    /// Look input for event
    /// </summary>
    /// <param name="arg0"></param>
    void InputLook (Vector2 arg0) {
        lookInput = arg0;
    }

    /// <summary>
    /// Player look function
    /// </summary>
    void OnPlayerLook () {
        //Debug.Log ($"lookInput {lookInput}");
        playerTransform.localRotation *= Quaternion.Euler (0, lookInput.x, 0);
    }
}