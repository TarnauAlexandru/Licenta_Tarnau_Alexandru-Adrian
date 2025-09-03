using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    private PlayerInput playerInput;
    protected Vector2 moveInput;
    [HideInInspector] public PlayerBaseNou player;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        player = GetComponent<PlayerBaseNou>();

        playerInput.actions["Move"].performed += OnMove;
        playerInput.actions["Move"].canceled += OnMove;

        playerInput.actions["PivotLeft"].performed += OnPivotLeft;
        playerInput.actions["PivotRight"].performed += OnPivotRight;
    }

    private void Update()
    {
       // if (moveInput.magnitude > 0.1f)
            //HandleMovement();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        UnityEngine.Debug.Log($"[{gameObject.name}] OnMove - player: {player} ({(player != null ? player.GetType().Name : "NULL")})", this);
        moveInput = context.ReadValue<Vector2>();
        HandleMovement();
    }

    public void OnPivotLeft(InputAction.CallbackContext context)
    {

        UnityEngine.Debug.Log($"[{gameObject.name}] OnPivotLeft - player: {player} ({(player != null ? player.GetType().Name : "NULL")})", this);
        if (context.performed)
            player.TryPlayPivot(player.GetPivotLeft(), player.GetPivotStamina()); 
    }
    public void OnPivotRight(InputAction.CallbackContext context)
    {
        UnityEngine.Debug.Log($"[{gameObject.name}] OnPivotRight - player: {player} ({(player != null ? player.GetType().Name : "NULL")})", this);
        if (context.performed)
            player.TryPlayPivot(player.GetPivotRight(), player.GetPivotStamina());
 
    }

    protected void HandleMovement()
    {
        UnityEngine.Debug.Log($"[{gameObject.name}] HandleMovement - player: {player} ({(player != null ? player.GetType().Name : "NULL")})", this);
        float magnitude = moveInput.magnitude;
        if (magnitude < 0.1f) return;

        float angle = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;

        if (angle >= 315 || angle <= 45)
            player.TryPlayMove(player.GetStepForward(), player.GetStepStamina());
        else if (angle > 45 && angle <= 135)
            player.TryPlayMove(player.GetStepRight(), player.GetStepStamina());
        else if (angle > 135 && angle <= 225)
            player.TryPlayMove(player.GetStepBack(), player.GetStepStamina());
        else if (angle > 225 && angle <= 315)
            player.TryPlayMove(player.GetStepLeft(), player.GetStepStamina());
    }
}
