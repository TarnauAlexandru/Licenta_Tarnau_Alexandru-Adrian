using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;


public class PlayerController : MonoBehaviour
{
    [HideInInspector] public PlayerBaseNou player;

    private PlayerInput playerInput;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        player = GetComponent<PlayerBaseNou>();
        if (player == null)
        {
            UnityEngine.Debug.LogError($"[{gameObject.name}] PlayerBase component not found!", this);
        }
        else
        {
            UnityEngine.Debug.Log($"[{gameObject.name}] PlayerBase component found: {player.GetType().Name}", this);
        }

        UnityEngine.Debug.Log($"PlayerController AWAKE on {gameObject.name}");

        playerInput.actions["Taunt"].performed += OnTaunt;
        playerInput.actions["BlockLeft"].performed += OnBlockLeft;
        playerInput.actions["BlockRight"].performed += OnBlockRight;
        playerInput.actions["BlockCenter"].performed += OnBlockCenter;
        playerInput.actions["JabLeft"].performed += OnJabLeft;
        playerInput.actions["JabRight"].performed += OnJabRight;
        playerInput.actions["UppercutLeft"].performed += OnUppercutLeft;
        playerInput.actions["UppercutRight"].performed += OnUppercutRight;
        playerInput.actions["CrossLeft"].performed += OnCrossLeft;
        playerInput.actions["CrossRight"].performed += OnCrossRight;

    }

    private void OnEnable()
    {
        if (player == null)
            player = GetComponent<PlayerBaseNou>();
    }

    /*public void pozitionCorrection()
    {
        Vector3 pos = transform.position;
        transform.position = new Vector3(pos.x, 1.75f, pos.z);

    } */


    public void OnTaunt(InputAction.CallbackContext context)
    {
        UnityEngine.Debug.Log($"[{gameObject.name}] OnTaunt - player: {player} ({(player != null ? player.GetType().Name : "NULL")})", this);
        if (context.performed)
            player.TryPlayAction(player.GetTaunt(), player.GetTauntStamina());
        //pozitionCorrection();
    }

    public void OnBlockLeft(InputAction.CallbackContext context)
    {
        UnityEngine.Debug.Log($"[{gameObject.name}] OnBlockLeft - player: {player} ({(player != null ? player.GetType().Name : "NULL")})", this);
        if (context.performed)
            player.TryPlayAction(player.GetBlockLeft(), player.GetBlockStamina());
        //pozitionCorrection();
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
        if (context.performed) player.TryPlayJab(player.GetJabLeft(), player.GetJabStamina());
    }

    public void OnJabRight(InputAction.CallbackContext context)
    {
        if (context.performed) player.TryPlayJab(player.GetJabRight(), player.GetJabStamina());
    }

    public void OnUppercutLeft(InputAction.CallbackContext context)
    {
        if (context.performed) player.TryPlayUppercut(player.GetUppercutLeft(), player.GetUppercutStamina());
    }

    public void OnUppercutRight(InputAction.CallbackContext context)
    {
        if (context.performed) player.TryPlayUppercut(player.GetUppercutRight(), player.GetUppercutStamina());
    }

    public void OnCrossLeft(InputAction.CallbackContext context)
    {
        if (context.performed) player.TryPlayCross(player.GetCrossLeft(), player.GetCrossStamina());
    }

    public void OnCrossRight(UnityEngine.InputSystem.InputAction.CallbackContext context) 
    {
        if (context.performed) player.TryPlayCross(player.GetCrossRight(), player.GetCrossStamina());

    }
}