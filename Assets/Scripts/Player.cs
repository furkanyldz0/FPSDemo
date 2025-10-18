using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GameInput gameInput;
    [SerializeReference] private Gun gun;
    private CharacterController characterController;

    private Vector3 playerVelocity;
    private bool isDoubleJumpAvailable;
    private float jumpHeight = 4f;
    private float gravityValue = -9.81f * 2f;
    private float landSpeed = -40f;
    private float jumpBufferTime = .1f;
    private float jumpBufferCounter;

    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    private Vector3 mouseWorldPosition;


    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        gameInput.OnJumpAction += GameInput_OnJumpAction;
        gameInput.OnLandAction += GameInput_OnLandAction;
        //gameInput.OnSingleShot += GameInput_OnSingleShot;
        isDoubleJumpAvailable = false;
    }

    //private void GameInput_OnSingleShot(object sender, EventArgs e) {
    //    if(gun != null) {
    //        gun.Shoot(mouseWorldPosition);
    //    }
    //}

    private void GameInput_OnLandAction(object sender, EventArgs e) {
        Land();
    }

    private void GameInput_OnJumpAction(object sender, System.EventArgs e) {
        //Jump();
        jumpBufferCounter = jumpBufferTime;
    }

    private void Update() {
        HandleJumping();
        HandleMovement();
        HandleShooting();

        mouseWorldPosition = Vector3.zero;

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask)) {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
        }
    }
    private void HandleShooting() {
        gun.SetState(gameInput.IsFiring(), mouseWorldPosition);
        
    }

    //private void HandleShooting() {
    //    if (gameInput.IsFiring() && gun != null) {
    //        gun.Shoot(mouseWorldPosition);
    //    }
    //}

    private void HandleJumping() {
        jumpBufferCounter -= Time.deltaTime;
        if (jumpBufferCounter > 0f) {
            if (IsGrounded() || isDoubleJumpAvailable) {
                Jump();
                jumpBufferCounter = 0f;
                isDoubleJumpAvailable = !isDoubleJumpAvailable;
            }
        }

        if (IsGrounded() && !gameInput.IsJumping()) { //!isjumping olarak da kontrol etmemiz lazým
            isDoubleJumpAvailable = false;
        }
    }

    private void HandleMovement() {
        Vector2 inputVector = gameInput.GetMovementVector2();

        Vector3 moveDirectionXZ = inputVector.x * transform.right + inputVector.y * transform.forward;
        moveDirectionXZ = moveDirectionXZ.normalized;

        if (gameInput.IsSprinting())
            moveSpeed = 15f;
        else
            moveSpeed = 5f;

     
        if (IsGrounded() && playerVelocity.y < 0f) {
            playerVelocity.y = 0f;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;

        //x,z + y ekseni
        Vector3 finalMove = (moveDirectionXZ * moveSpeed) + (playerVelocity.y * Vector3.up);

        characterController.Move(finalMove * Time.deltaTime);
    }

    private void Jump() {
        //if (IsGrounded()) {
        //    playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
        //    jumpBufferCounter = 0f;
        //    //Debug.Log("jump: " + playerVelocity.y);
        //}
        playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
        
    }

    private void Land() {
        if (!IsGrounded()) {
            playerVelocity.y = landSpeed;
        }
    }

    private bool IsGrounded() {
        return characterController.isGrounded;
    }


























    //private void HandleMovement() { capsule cast ile kendimiz collision kontrol etmek istersek
    //    Vector2 inputVector = gameInput.GetMovementVector2();
    //    Vector3 moveDirection = inputVector.x * transform.right + inputVector.y * transform.forward;
    //    moveDirection = moveDirection.normalized;

    //    //characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

    //    float moveDistance = moveSpeed * Time.deltaTime;
    //    float playerHeight = 2f;
    //    float playerRadius = .5f;

    //    bool canMove = !Physics.CapsuleCast(transform.position - playerHeight / 2 * Vector3.up,
    //        transform.position + playerHeight / 2 * Vector3.up,
    //        playerRadius, moveDirection, moveDistance);

    //    if (!canMove) {
    //        //x ekseninde hareket ediyor mu
    //        Vector3 moveDirectionX = inputVector.x * transform.right;
    //        moveDirectionX = moveDirectionX.normalized;

    //        canMove = !Physics.CapsuleCast(transform.position - playerHeight / 2 * Vector3.up,
    //            transform.position + playerHeight / 2 * Vector3.up,
    //            playerRadius, moveDirectionX, moveDistance);

    //        if (canMove) {
    //            moveDirection = moveDirectionX;
    //        }
    //        else {
    //            //z ekseninde hareket ediyor mu
    //            Vector3 moveDirectionZ = inputVector.y * transform.forward;
    //            moveDirectionZ = moveDirectionZ.normalized;

    //            canMove = !Physics.CapsuleCast(transform.position - playerHeight / 2 * Vector3.up,
    //                transform.position + playerHeight / 2 * Vector3.up,
    //                playerRadius, moveDirectionZ, moveDistance);

    //            if (canMove) {
    //                moveDirection = moveDirectionZ;
    //            }
    //            else {
    //                //hareket yok
    //            }
    //        }
    //    }
    //    if (canMove) {
    //        transform.position += moveDirection * moveDistance;
    //    }
    //}
}
