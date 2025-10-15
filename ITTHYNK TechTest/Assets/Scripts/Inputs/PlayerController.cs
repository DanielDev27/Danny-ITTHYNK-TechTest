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


    void FixedUpdate () {
        if (moveInput != Vector2.zero) {
            isMoving = true;
            OnPlayerMove ();
        } else {
            isMoving = false;
        }

        cubeTransform.localPosition = Vector3.zero;
        OnPlayerLook ();
    }

    void InputMove (Vector2 arg0) {
        moveInput = arg0;
    }

    void OnPlayerMove () {
        //Debug.Log ($"moveInput {moveInput}");
        if (Camera.main != null) {
            moveDirection = moveInput.x * Camera.main.transform.right + moveInput.y * Camera.main.transform.forward;
        }

        if (!isDashing) {
            playerRigidbody.velocity = isMoving ? new Vector3 (moveDirection.x * moveSpeed, playerRigidbody.velocity.y, moveDirection.z * moveSpeed) : Vector3.zero;
            cubeTransform.Rotate (playerTransform.forward * -moveDirection.x * rotationSpeed * moveSpeed, Space.World);
            cubeTransform.Rotate (playerTransform.right * moveDirection.z * rotationSpeed * moveSpeed, Space.World);
        } else {
            playerRigidbody.velocity = isMoving ? new Vector3 (moveDirection.x * dashSpeed, playerRigidbody.velocity.y, moveDirection.z * dashSpeed) : Vector3.zero;
            cubeTransform.Rotate (playerTransform.forward * -moveDirection.x * rotationSpeed * dashSpeed, Space.World);
            cubeTransform.Rotate (playerTransform.right * moveDirection.z * rotationSpeed * dashSpeed, Space.World);
        }
    }

    void InputDash () {
        isDashing = true;
    }

    void InputLook (Vector2 arg0) {
        lookInput = arg0.normalized;
    }

    void OnPlayerLook () {
        //Debug.Log ($"lookInput {lookInput}");
        transform.rotation = Quaternion.Euler (0, virtualCamera.transform.eulerAngles.y, 0);
    }
}