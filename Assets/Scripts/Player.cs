using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Player Instance { private set; get; }
    public event EventHandler<OnSelectedGunChangedEventArgs> OnSelectedGunChanged;
    public class OnSelectedGunChangedEventArgs : EventArgs{
        public Gun selectedGun;
    }
    private Gun selectedGun;

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private GameInput gameInput;
    [SerializeReference] private Gun gun;
    private CharacterController characterController;

    private Vector3 playerVelocity;
    private Vector3 moveDir;
    private Vector3 dashDir;
    private Vector3 lastInteractDir;
    private bool isDoubleJumpAvailable, isJumpAvailable;
    private float jumpHeight = 4f;
    private float gravityValue = -9.81f * 2f;
    private float landSpeed = -40f;
    private float jumpBufferTime = .1f;
    private float jumpBufferCounter;
    private float defaultDashTime = .2f;
    private float dashTimeCounter;
    private float dashEffectCounter;
    private int dashCounter = 3;

    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    private Vector3 mouseWorldPosition;


    private void Awake() {
        if(Instance != null) {
            Debug.LogError("Birden fazla player nesnesi var");            
        }
        Instance = this;
    }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        gameInput.OnJumpAction += GameInput_OnJumpAction;
        gameInput.OnLandAction += GameInput_OnLandAction;
        gameInput.OnDashAction += GameInput_OnDashAction;
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        isDoubleJumpAvailable = false;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e) {
        if(selectedGun != null) {
            selectedGun.Interact();
        }
    }

    private void GameInput_OnDashAction(object sender, EventArgs e) {
        if (dashCounter > 0) {
            dashCounter--;
            dashTimeCounter = defaultDashTime;
            Vector2 inputVector = gameInput.GetMovementVector2();
            dashDir = transform.forward * inputVector.y + transform.right * inputVector.x;
            dashDir = dashDir.magnitude <= 0f ? transform.forward : dashDir.normalized; //valla ben yazdým (input yoksa düz ileri dash atsýn)
        }
    }

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
        HandleDash();
        HandleInteractions();

        mouseWorldPosition = Vector3.zero;

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask)) { //layermaski sil
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
            if (gameInput.IsFiring() && raycastHit.collider.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rigidbody)) {
                rigidbody.AddExplosionForce(200f, mouseWorldPosition, 5f); //vurulan nesnelerin uçmasý için
            }
        }
    }

    private void HandleInteractions() {
        if (moveDir != Vector3.zero) {
            lastInteractDir = moveDir;
        }

        float interactDistance = 2f;

        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHitInteract, interactDistance)) {

            if (raycastHitInteract.transform.TryGetComponent(out Gun gun)) {
                if (gun != selectedGun) {
                    SetSelectedGun(gun); //event tetikliyor
                }
            }
            else {
                SetSelectedGun(null);
            }
        }
        else {
            SetSelectedGun(null);
        }
    }

    private void HandleMovement() {
        Vector2 inputVector = gameInput.GetMovementVector2();

        moveDir = inputVector.x * transform.right + inputVector.y * transform.forward;

        //if (gameInput.IsSprinting())
        //    moveSpeed = 15f;
        //else
        //    moveSpeed = 5f;
        //moveSpeed = 10f;

        if (IsGrounded() && playerVelocity.y < 0f) {
            playerVelocity.y = 0f;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;

        //x,z + y ekseni
        Vector3 finalMove = (moveDir * moveSpeed) + (playerVelocity.y * Vector3.up);

        characterController.Move(finalMove * Time.deltaTime);
    }

    private void HandleJumping() {
        jumpBufferCounter -= Time.deltaTime;
        if (jumpBufferCounter > 0f) {
            if (isJumpAvailable || isDoubleJumpAvailable) { //isJumpAvailable yerine isgrounded vardý
                Jump();
                jumpBufferCounter = 0f;
                isDoubleJumpAvailable = !isDoubleJumpAvailable;
                isJumpAvailable = false;
            }
        }

        if (IsGrounded() && !gameInput.IsJumping()) { //!isjumping olarak da kontrol etmemiz lazým
            isDoubleJumpAvailable = false;
            isJumpAvailable = true;
        }
    }

    private void Jump() {
        //if (IsGrounded()) {
        //    playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
        //    jumpBufferCounter = 0f;
        //    //Debug.Log("jump: " + playerVelocity.y);
        //}
        playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
        
    }
    private void HandleDash() {
        float dashPower = 2f;

        if (IsGrounded()) {
            dashCounter = 3;
        }

        if (dashTimeCounter > 0f) {
            dashTimeCounter -= Time.deltaTime;
            characterController.Move(Time.deltaTime * moveSpeed * dashPower * dashDir);
            playerVelocity.y = 0;
            dashEffectCounter = defaultDashTime;
            //Debug.Log("dash");
        }
        else if (dashEffectCounter > 0f) {
            dashEffectCounter -= Time.deltaTime;
            float dashEffectPower = (moveSpeed * dashPower) / 3;
            dashDir = gameInput.GetMovementVector2() == Vector2.zero ? Vector2.zero : dashDir;
            characterController.Move(Time.deltaTime * dashEffectPower * dashDir);
            //Debug.Log("dash effect");
        }
    }

    private void HandleShooting() {
        gun.SetState(IsShooting(), mouseWorldPosition);
    }
    //private void HandleShooting() {
    //    if (gameInput.IsFiring() && gun != null) {
    //        gun.Shoot(mouseWorldPosition);
    //    }
    //}

    private void Land() {
        if (!IsGrounded()) {
            playerVelocity.y = landSpeed;
        }
    }

    private bool IsGrounded() {
        return characterController.isGrounded;
    }

    public bool IsShooting() {
        return gameInput.IsFiring();
    }

    private void SetSelectedGun(Gun selectedGun) {
        this.selectedGun = selectedGun;

        OnSelectedGunChanged?.Invoke(this, new OnSelectedGunChangedEventArgs {
            selectedGun = selectedGun
        });
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
