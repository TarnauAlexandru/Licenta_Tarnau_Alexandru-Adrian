using System;
using System.Collections;
using UnityEngine;

public class AIPlayerController : MonoBehaviour
{
    public PlayerBaseNou player;
    public Transform targetPlayer;
    private float thinkTime = 1.0f;
    private float attackRange = 2.2f;
    private float blockChance = 0.3f;
    private bool isThinking = false;
    private bool canAttack = true;

    private void Start()
    {
        player = GetComponent<PlayerBaseNou>();

        // Initialize Rigidbody constraints (same as PlayerController)
        var rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }

        StartCoroutine(AIThinkLoop());
    }

    IEnumerator AIThinkLoop()
    {
        while (targetPlayer == null)
            yield return null; // Wait until targetPlayer is set
        while (true)
        {
            if (!isThinking)
            {
                isThinking = true;
                AIThink();
                yield return new WaitForSeconds(thinkTime);
                isThinking = false;
            }
            yield return null;
        }
    }

    private void AIThink()
    {
        if (!canAttack) return;

        float dist = Vector3.Distance(player.transform.position, targetPlayer.position);

        if (dist > attackRange)
        {
            if (UnityEngine.Random.value < 0.5f)
            {
                player.TryPlayAction(player.GetShortStepForward(), player.GetShortStepStamina());
            }
            else
            {
                // Pivot logic with rotation
                if (UnityEngine.Random.value < 0.5f)
                {
                    if (player.TryPlayAction(player.GetPivotLeft(), player.GetPivotStamina()))
                    {
                        transform.Rotate(0, -90, 0);
                    }
                }
                else
                {
                    if (player.TryPlayAction(player.GetPivotRight(), player.GetPivotStamina()))
                    {
                        transform.Rotate(0, 90, 0);
                    }
                }
            }
            return;
        }

        float r = UnityEngine.Random.value;

        if (r < 0.35f)
        {
            string jab = (UnityEngine.Random.value < 0.5f) ? player.GetJabLeft() : player.GetJabRight();
            player.TryPlayAction(jab, player.GetJabStamina());

            if (UnityEngine.Random.value < 0.25f)
            {
                StartCoroutine(ComboDelay(jab == player.GetJabLeft() ? player.GetCrossLeft() : player.GetCrossRight(), 0.3f, player.GetCrossStamina()));
            }
        }
        else if (r < 0.55f)
        {
            string up = (UnityEngine.Random.value < 0.5f) ? player.GetUppercutLeft() : player.GetUppercutRight();
            player.TryPlayAction(up, player.GetUppercutStamina());
        }
        else if (r < 0.65f)
        {
            float b = UnityEngine.Random.value;
            if (b < 0.33f) player.TryPlayAction(player.GetBlockLeft(), player.GetBlockStamina());
            else if (b < 0.66f) player.TryPlayAction(player.GetBlockCenter(), player.GetBlockStamina());
            else player.TryPlayAction(player.GetBlockRight(), player.GetBlockStamina());
        }
        else if (r < 0.8f)
        {
            player.TryPlayAction(UnityEngine.Random.value < 0.5f ? player.GetCrossLeft() : player.GetCrossRight(), player.GetCrossStamina());
        }
        else
        {
            // Pivot logic with rotation
            if (UnityEngine.Random.value < 0.5f)
            {
                if (player.TryPlayAction(player.GetPivotLeft(), player.GetPivotStamina()))
                {
                    transform.Rotate(0, -90, 0);
                }
            }
            else
            {
                if (player.TryPlayAction(player.GetPivotRight(), player.GetPivotStamina()))
                {
                    transform.Rotate(0, 90, 0);
                }
            }
        }
    }

    IEnumerator ComboDelay(string nextAttack, float delay, float staminaCost)
    {
        canAttack = false;
        yield return new WaitForSeconds(delay);
        player.TryPlayAction(nextAttack, staminaCost);
        canAttack = true;
    }

    // You can add more methods for hit/block reactions if you want more "human" AI
}