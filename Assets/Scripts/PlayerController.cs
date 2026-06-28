using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class PlayerController : MonoBehaviour
{
    public InputActionAsset InputActions;
    public CharacterController playerCharacterController;

    private InputAction playerMoveAction;
    private InputAction playerJumpAction;
    private InputAction playerAttackAction;
    private InputAction playerFeedAction;

    [SerializeField]
    private Transform playerCamera;

    private Vector2 playerMoveAmount;

    private float playerWalkSpeed = 5.0f;
    private float playerRotateDampening = 1f;
    private float turnSmoothingVelocity;

    private float verticalVelocity = 0f;
    private float gravity = -9.8f;
    private float jumpHeight = 5.0f;

    //bullet time
    public GameObject bloodBullet;
    public GameObject bulletSpawnLocation;
    public int ammo = 20;
    public TMP_Text ammoCounter;

    //health
    public int currentHealth = 96;
    public int maxHealth = 100;
    private int bloody;


    // === Add these new variables right here, OUTSIDE of any functions ===
    [SerializeField] private InputActionReference playerLookAction; 
    [SerializeField] private float mouseSensitivity = 15f;
    
    // THIS LINE FIXES THE CONTEXT ERROR:
    private float xRotation = 0f; 


    private void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        InputActions.FindActionMap("Player").Disable();
    }

    private void Awake()
    {
        playerMoveAction = InputSystem.actions.FindAction("Move");
        playerJumpAction = InputSystem.actions.FindAction("Jump");
        playerAttackAction = InputSystem.actions.FindAction("Attack");
        playerFeedAction = InputSystem.actions.FindAction("Interact");

    }

    private void Update()
    {
        ammoCounter.text = " " + ammo + " ";
        playerMoveAmount = playerMoveAction.ReadValue<Vector2>();

        Vector2 mouseInput = playerLookAction.action.ReadValue<Vector2>();
        PlayerLook(mouseInput);


        PlayerMoveAndRotate();
        Jump();
        Attack();
        Feed();
    }

    private void PlayerLook(Vector2 mouseInput)
    {
        float mouseX = mouseInput.x * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);
    }

    private void PlayerMoveAndRotate()
    {
        Vector3 moveDirection = (transform.forward * playerMoveAmount.y + transform.right * playerMoveAmount.x).normalized;
        moveDirection *= playerWalkSpeed;

        Vector3 verticalMove = new Vector3(0f, verticalVelocity, 0f);
        Vector3 finalMovement = (moveDirection + verticalMove) * Time.deltaTime;
        playerCharacterController.Move(finalMovement);
    }

    private void Jump()
    {
        if(playerCharacterController.isGrounded)
        {
            verticalVelocity = -1f;

            if (playerJumpAction.WasPressedThisFrame())
            {
                verticalVelocity = jumpHeight;
            }
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
    }

    private void Attack()
    {


        if (playerAttackAction.WasPressedThisFrame() && ammo > 0)
        {
            //I have no idea if we get a shooting animation but put it here


            //if we ever get it. reload is below
            Instantiate(bloodBullet, bulletSpawnLocation.transform.position, bulletSpawnLocation.transform.rotation);
            ammo -= 1;
            
        }
    }

    private void Feed()
    {
        if (playerFeedAction.WasPressedThisFrame())
        {
            float interactRange = 2f;
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange); 
            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent(out HumanBeBitten huBeBit))
                {
                    huBeBit.Interact();
                    //Player Reload Amination here


                    //in here. For victum explode go to HumanBeBitten
                    bloody = huBeBit.bloodAmount;
                    if (currentHealth >= maxHealth)
                    {
                        ammo += bloody;
                    }
                    else if ((currentHealth + bloody) > maxHealth)
                    {
                        int bloodMath = maxHealth - currentHealth;
                        currentHealth += bloodMath;
                        int remainingBlood = bloody - bloodMath;
                        ammo += remainingBlood;
                    }
                    else if ((currentHealth + bloody) <= maxHealth)
                    {
                        currentHealth += bloody;
                    }
                }
            }
        }
    }

}
