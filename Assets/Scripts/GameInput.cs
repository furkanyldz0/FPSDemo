using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnJumpAction;
    public event EventHandler OnLandAction;
    public event EventHandler OnDashAction;
    public event EventHandler OnReloadAction;
    public event EventHandler OnInteractAction;

    private InputActionSystem inputActionSystem;
    private bool isSprinting, isFiring;

    private void Awake() {
        inputActionSystem = new InputActionSystem();
        inputActionSystem.Player.Enable();
    }
    private void OnEnable() {
        //inputActionSystem.Player.Sprint.performed += OnSprint;
        //inputActionSystem.Player.Sprint.canceled += OnSprint;
        inputActionSystem.Player.Jump.performed += Jump_performed;
        inputActionSystem.Player.Land.performed += Land_performed;
        inputActionSystem.Player.Fire.performed += OnFire;
        inputActionSystem.Player.Fire.canceled += OnFire;
        inputActionSystem.Player.Dash.performed += Dash_performed;
        inputActionSystem.Player.Reload.performed += Reload_performed;
        inputActionSystem.Player.Interact.performed += Interact_performed;
    }
    private void OnDisable() {
        //inputActionSystem.Player.Sprint.performed -= OnSprint;
        //inputActionSystem.Player.Sprint.canceled -= OnSprint;
        inputActionSystem.Player.Jump.performed -= Jump_performed;
        inputActionSystem.Player.Land.performed -= Land_performed;
        inputActionSystem.Player.Fire.performed -= OnFire;
        inputActionSystem.Player.Fire.canceled -= OnFire;
        inputActionSystem.Player.Dash.performed -= Dash_performed;
        inputActionSystem.Player.Reload.performed -= Reload_performed;
        inputActionSystem.Player.Interact.performed -= Interact_performed;
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }
    private void Reload_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnReloadAction?.Invoke(this, EventArgs.Empty);
    }
    private void Dash_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnDashAction?.Invoke(this, EventArgs.Empty);
    }
    private void Land_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnLandAction?.Invoke(this, EventArgs.Empty);
    }
    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnJumpAction?.Invoke(this, EventArgs.Empty);
    } //double jump check için alttakini kullanmam lazým
    private void OnFire(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        if (obj.performed) {
            isFiring = true;
        }

        if (obj.canceled) {
            isFiring = false;
        }
    }

    //private void OnSprint(UnityEngine.InputSystem.InputAction.CallbackContext obj) {

    //    if (obj.performed) {
    //        isSprinting = true;
    //        //Debug.Log("Sprinting");
    //    }

    //    if (obj.canceled) {
    //        isSprinting = false;
    //        //Debug.Log("Sprint canceled");
    //    }
    //}

    public Vector2 GetMovementVector2() {
        Vector2 inputVector = inputActionSystem.Player.Move.ReadValue<Vector2>(); ;
        
        return inputVector;
    }

    public Vector2 GetMouseDelta() {
        Vector2 lookVector = inputActionSystem.Player.Look.ReadValue<Vector2>();

        return lookVector;
    }

    public bool IsSprinting() {
        return isSprinting;
    }
    public bool IsJumping() {
        return inputActionSystem.Player.Jump.IsPressed(); //getbutton'ýn karþýlýðý
    }
    public bool IsFiring() {
        return isFiring;
    }







    //private void Fire_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
    //    if (obj.interaction is TapInteraction) {
    //        //singleshot
    //        OnSingleShot?.Invoke(this, EventArgs.Empty);
    //    }
    //    else if (obj.interaction is HoldInteraction) {
    //        //spray
    //        isFiring = true;
    //    }
    //}
    //private void Fire_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
    //    isFiring = false; //obj.canceled'a gerek yokmuþ
    //}





    //public Vector2 GetMovementVector2() {
    //    Vector2 inputVector = Vector2.zero;

    //    if (Input.GetKey(KeyCode.W)) {
    //        inputVector.y += 1;
    //    }
    //    if (Input.GetKey(KeyCode.S)) {
    //        inputVector.y -= 1;
    //    }
    //    if (Input.GetKey(KeyCode.A)) {
    //        inputVector.x -= 1;
    //    }
    //    if (Input.GetKey(KeyCode.D)) {
    //        inputVector.x += 1;
    //    }
    //    //inputVector = inputVector.normalized;
    //    return inputVector;
    //}
}
