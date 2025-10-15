using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputHandler {
    public static TechInputs techInputs;

    static InputHandler instance;

    //
    //Events for buttons
    //
    public static UnityEvent<Vector2> OnMovePerformed = new UnityEvent<Vector2> ();
    public static UnityEvent<Vector2> OnLookPerformed = new UnityEvent<Vector2> ();
    public static UnityEvent OnDashPerformed = new UnityEvent ();

    //
    //Input Values
    //
    public static Vector2 moveInput;
    public static Vector2 lookInput;

    public static InputHandler Instance {
        get {
            if (instance == null) {
                instance = new InputHandler ();
            }

            return instance;
        }
        private set {
            instance = value;
        }
    }

    //Input Handler Class functions
    public static void Enable () {
        if (techInputs == null) {
            techInputs = new TechInputs ();
        }

        RegisterInputs ();
        techInputs.Enable ();
    }

    public static void Disable () {
        if (techInputs == null) {
            return;
        }

        techInputs.Disable ();
    }

    public static void Dispose () {
        if (techInputs == null) {
            return;
        }

        techInputs.Dispose ();
    }

    /// <summary>
    /// Register the Inputs from the control scheme into unity events
    /// </summary>
    static void RegisterInputs () {
        //Debug.Log ("Register Inputs");
        //Move
        techInputs.Player.Move.performed += MovePerformed;
        techInputs.Player.Move.canceled += MovePerformed;
        //Dash
        techInputs.Player.Dash.performed += DashPerformed;
        //Look
        techInputs.Player.Look.performed += LookPerformed;
        techInputs.Player.Look.canceled += LookPerformed;
        //Pause
        //techInputs.Player.Pause.performed += PausePerformed;
    }

    /// <summary>
    /// Move Perform event
    /// </summary>
    /// <param name="obj"></param>
    static void MovePerformed (InputAction.CallbackContext obj) {
        if (obj.ReadValue<Vector2> ().normalized != Vector2.zero) {
            moveInput = obj.ReadValue<Vector2> ().normalized;
        }

        if (obj.ReadValue<Vector2> ().normalized == Vector2.zero) {
            moveInput = Vector2.zero;
        }

        OnMovePerformed?.Invoke (moveInput);
    }

    /// <summary>
    /// Dash Perform event
    /// </summary>
    /// <param name="obj"></param>
    static void DashPerformed (InputAction.CallbackContext obj) {
        OnDashPerformed?.Invoke ();
    }

    /// <summary>
    /// Look Perform event
    /// </summary>
    /// <param name="obj"></param>
    static void LookPerformed (InputAction.CallbackContext obj) {
        if (obj.ReadValue<Vector2> ().normalized != Vector2.zero) {
            lookInput = obj.ReadValue<Vector2> ().normalized;
        }

        if (obj.ReadValue<Vector2> ().normalized == Vector2.zero) {
            lookInput = Vector2.zero;
        }

        OnLookPerformed?.Invoke (lookInput);
    }
}