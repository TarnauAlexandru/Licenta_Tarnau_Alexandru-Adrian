using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    [HideInInspector] public PlayerBaseNou player;

    protected Vector2 moveInput;
    [SerializeField] protected float mediumThreshold = 0.7f;

    private void Awake()
    {
        // Asigură-te că referința e mereu corectă la instanțiere
        player = GetComponent<PlayerBaseNou>();
        if (player == null)
        {
            UnityEngine.Debug.LogError($"[{gameObject.name}] PlayerBase component not found!", this);
        }
        else
        {
            UnityEngine.Debug.Log($"[{gameObject.name}] PlayerBase component found: {player.GetType().Name}", this);
        }

        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;


        UnityEngine.Debug.Log($"PlayerController AWAKE on {gameObject.name}");
    }

    private void OnEnable()
    {
        // Asigură-te că referința nu se pierde la reactivare
        if (player == null)
            player = GetComponent<PlayerBaseNou>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        UnityEngine.Debug.Log($"[{gameObject.name}] OnMove - player: {player} ({(player != null ? player.GetType().Name : "NULL")})", this);
        moveInput = context.ReadValue<Vector2>();
        HandleMovement();
    }

    /*public void OnPivotLeft(InputAction.CallbackContext context)
    {

        UnityEngine.Debug.Log($"[{gameObject.name}] OnPivotLeft - player: {player} ({(player != null ? player.GetType().Name : "NULL")})", this);
        if (context.performed)
            player.TryPlayAction(player.GetPivotLeft(), player.GetPivotStamina());
        
    }*/

    public void OnPivotLeft(InputAction.CallbackContext context)
    {

        UnityEngine.Debug.Log($"[{gameObject.name}] OnPivotLeft - player: {player} ({(player != null ? player.GetType().Name : "NULL")})", this);
        if (context.performed)
            if (player.TryPlayAction(player.GetPivotLeft(), player.GetPivotStamina())) { transform.Rotate(0, -90, 0); }


    }

    /*public void OnPivotRight(InputAction.CallbackContext context)
    {
        UnityEngine.Debug.Log($"[{gameObject.name}] OnPivotRight - player: {player} ({(player != null ? player.GetType().Name : "NULL")})", this);
        if (context.performed)
            player.TryPlayAction(player.GetPivotRight(), player.GetPivotStamina());
    } */

    public void OnPivotRight(InputAction.CallbackContext context)
    {
        UnityEngine.Debug.Log($"[{gameObject.name}] OnPivotRight - player: {player} ({(player != null ? player.GetType().Name : "NULL")})", this);
        if (context.performed)
           if( player.TryPlayAction(player.GetPivotRight(), player.GetPivotStamina())) { transform.Rotate(0, +90, 0); }
    }

    public void OnTaunt(InputAction.CallbackContext context)
    {
        UnityEngine.Debug.Log($"[{gameObject.name}] OnTaunt - player: {player} ({(player != null ? player.GetType().Name : "NULL")})", this);
        if (context.performed)
            player.TryPlayAction(player.GetTaunt(), player.GetTauntStamina());
    }

    public void OnBlockLeft(InputAction.CallbackContext context)
    {
        UnityEngine.Debug.Log($"[{gameObject.name}] OnBlockLeft - player: {player} ({(player != null ? player.GetType().Name : "NULL")})", this);
        if (context.performed)
            player.TryPlayAction(player.GetBlockLeft(), player.GetBlockStamina());
    }


    public void OnBlockRight(InputAction.CallbackContext context)
    {
        UnityEngine.Debug.Log($"[{gameObject.name}] OnBlockRight - player: {player} ({(player != null ? player.GetType().Name : "NULL")})", this);
        if (context.performed)
            player.TryPlayAction(player.GetBlockRight(), player.GetBlockStamina());
    }

    public void OnBlockCenter(InputAction.CallbackContext context)
    {
        UnityEngine.Debug.Log($"[{gameObject.name}] OnBlockCenter - player: {player} ({(player != null ? player.GetType().Name : "NULL")})", this);
        if (context.performed)
            player.TryPlayAction(player.GetBlockCenter(), player.GetBlockStamina());
    }

    public void OnJabLeft(InputAction.CallbackContext context)
    {
        UnityEngine.Debug.Log($"[{gameObject.name}] OnJabLeft - player: {player} ({(player != null ? player.GetType().Name : "NULL")})", this);
        if (context.performed)
            player.TryPlayAction(player.GetJabLeft(), player.GetJabStamina());
    }

    public void OnJabRight(InputAction.CallbackContext context)
    {
        UnityEngine.Debug.Log($"[{gameObject.name}] OnJabRight - player: {player} ({(player != null ? player.GetType().Name : "NULL")})", this);
        if (context.performed)
            player.TryPlayAction(player.GetJabRight(), player.GetJabStamina());
    }

    public void OnUppercutLeft(InputAction.CallbackContext context)
    {
        UnityEngine.Debug.Log($"[{gameObject.name}] OnUppercutLeft - player: {player} ({(player != null ? player.GetType().Name : "NULL")})", this);
        if (context.performed)
            player.TryPlayAction(player.GetUppercutLeft(), player.GetUppercutStamina());
    }

    public void OnUppercutRight(InputAction.CallbackContext context)
    {
        UnityEngine.Debug.Log($"[{gameObject.name}] OnUppercutRight - player: {player} ({(player != null ? player.GetType().Name : "NULL")})", this);
        if (context.performed)
            player.TryPlayAction(player.GetUppercutRight(), player.GetUppercutStamina());
    }

    public void OnCrossLeft(InputAction.CallbackContext context)
    {
        UnityEngine.Debug.Log($"[{gameObject.name}] OnCrossLeft - player: {player} ({(player != null ? player.GetType().Name : "NULL")})", this);
        if (context.performed)
            player.TryPlayAction(player.GetCrossLeft(), player.GetCrossStamina());
    }

    public void OnCrossRight(UnityEngine.InputSystem.InputAction.CallbackContext context) 
    {
        if (context.performed)
            player.TryPlayAction(player.GetCrossRight(), player.GetCrossStamina()); 
    }

    /*public void OnCrossRight(InputAction.CallbackContext context)
    {
        Debug.Log("CALLED OnCrossRight!!!", this);
        Debug.Log($"[{gameObject.name}] OnCrossRight - player: {player} ({(player != null ? player.GetType().Name : "NULL")})", this);
        if (context.performed)
            player.TryPlayAction(player.GetCrossRight(), player.GetCrossStamina());
    }*/

    protected void HandleMovement()
    {
        UnityEngine.Debug.Log($"[{gameObject.name}] HandleMovement - player: {player} ({(player != null ? player.GetType().Name : "NULL")})", this);
        float magnitude = moveInput.magnitude;
        if (magnitude < 0.1f) return;

        float angle = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;

        if (angle >= 315 || angle <= 45)
            player.TryPlayAction(player.GetShortStepForward(), player.GetShortStepStamina());
        if (angle > 45 && angle <= 135)
            player.TryPlayAction(player.GetShortStepRight(), player.GetShortStepStamina());
        if (angle > 135 && angle <= 225)
            player.TryPlayAction(player.GetShortStepBack(), player.GetShortStepStamina());
        if (angle > 225 && angle <= 315)
            player.TryPlayAction(player.GetShortStepLeft(), player.GetShortStepStamina());
    }
}