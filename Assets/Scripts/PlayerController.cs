using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputActionAsset InputActions;
    public CharacterController playerCharacterController;

    private InputAction playerMoveAction;
    private InputAction playerJumpAction;

    [SerializeField]
    private Transform playerCamera;

    private Vector2 playerMoveAmount;

    private float playerWalkSpeed = 5.0f;
    private float playerRotateDampening = 0.1f;
    private float turnSmoothingVelocity;

    private float verticalVelocity = 0f;
    private float gravity = -9.8f;
    private float jumpHeight = 5.0f;

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
    }

    private void Update()
    {
        playerMoveAmount = playerMoveAction.ReadValue<Vector2>();
        PlayerMoveAndRotate();
        Jump();
    }

    private void PlayerMoveAndRotate()
    {
        Vector3 playerDirection = new Vector3(playerMoveAmount.x, 0f, playerMoveAmount.y).normalized;
        Vector3 verticalMove = new Vector3(0f, verticalVelocity, 0f) * Time.deltaTime;

        if(playerDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(playerDirection.x, playerDirection.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            float smoothTargetAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothingVelocity, playerRotateDampening);

            transform.rotation = Quaternion.Euler(0f, smoothTargetAngle, 0f);

            Vector3 moveDirection = Quaternion.Euler (0f, targetAngle, 0f) * Vector3.forward;
            playerCharacterController.Move(moveDirection.normalized * playerWalkSpeed * Time.deltaTime + verticalMove);

        }
        else
        {
            playerCharacterController.Move(verticalMove);
        }
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



}
