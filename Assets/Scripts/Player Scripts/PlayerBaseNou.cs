using UnityEngine;
using System.Collections.Generic;

public abstract class PlayerBaseNou : MonoBehaviour
{
    [Header("Core Stats")]
    public int maxHealth = 1000;
    public int currentHealth;

    public float maxStamina = 100f;
    public float currentStamina;
    protected float staminaRegenRate;
    protected float staminaRegenCooldown = 0f;

    [Header("Combat")]
    public int baseDamage;
    public float defense;
    public float critChance;
    public float critMultiplier = 1.5f;

    protected Animator animator;
    protected bool isPerformingAction = false;
    protected string pendingStepTrigger = null;
    protected string lastAttackType;

    // Regiuni de guard
    private static readonly HashSet<string> GuardRegions = new()
    {
        "LeftShoulder", "RightShoulder",
        "LeftArm", "RightArm",
        "LeftForeArm", "RightForeArm",
        "LeftHand", "RightHand"
    };

    // Doar lovituri din maini contează
    private static readonly HashSet<string> ValidHitSources = new()
    {
        "LeftHand", "RightHand"
    };

    public virtual void Awake()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        animator = GetComponent<Animator>();

    }

    protected virtual void Update()
    {
        if (CanRegenerateStamina())
            RegenerateStamina();
        else
            staminaRegenCooldown -= Time.deltaTime;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (isPerformingAction && stateInfo.normalizedTime >= 1f && !animator.IsInTransition(0))
        {
            isPerformingAction = false;

            if (!string.IsNullOrEmpty(pendingStepTrigger))
            {
                animator.SetTrigger(pendingStepTrigger);
                pendingStepTrigger = null;
            }
        }
    }

    protected virtual bool CanRegenerateStamina()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        return staminaRegenCooldown <= 0f && state.IsName("Idle");
    }

    protected void RegenerateStamina()
    {
        currentStamina = Mathf.Min(maxStamina, currentStamina + staminaRegenRate * Time.deltaTime);
    }

    public void ApplyStaminaCost(float amount)
    {
        currentStamina = Mathf.Max(0f, currentStamina - amount);
    }

    public void TriggerStaminaBreak()
    {
        currentStamina *= 0.2f;
        staminaRegenCooldown = 2f;
    }

    // NOU: Logica centralizată pentru hit detection și animații
    public void ReceiveHit(
        PlayerBaseNou attacker,
        string hitSourceBone, // ex: "LeftHand", "RightHand"
        string hitTargetBone, // ex: "Head", "Spine1", "LeftArm" etc.
        string hitType,       // ex: "JabLeft", "JabRight", "UppercutLeft", "UppercutRight", "CrossLeft", "CrossRight"
        bool isCrit)
    {
        if (!ValidHitSources.Contains(hitSourceBone))
            return;

        if (GuardRegions.Contains(hitTargetBone))
            return;

        string animTrigger = null;
        float multiplier = 1f;

        // Declară tipurile de player aici!
        bool attackerIsLight = attacker is LightweightPlayer;
        bool attackerIsHeavy = attacker is HeavyweightPlayer;
        bool receiverIsLight = this is LightweightPlayer;
        bool receiverIsHeavy = this is HeavyweightPlayer;

        switch (hitTargetBone)
        {
            case "Head":
                switch (hitType)
                {
                    case "JabLeft":
                    case "JabRight":
                        if (attackerIsLight && receiverIsHeavy)
                            animTrigger = GetRecieveaSmallPunchtotheFace();
                        else if (attackerIsLight && receiverIsLight)
                            animTrigger = GetRecieveaBigPunchtotheFace();
                        else if (attackerIsHeavy && receiverIsHeavy)
                            animTrigger = GetRecieveaSmallPunchtotheFace();
                        else if (attackerIsHeavy && receiverIsLight)
                            animTrigger = GetRecieveaBigPunchtotheFace();
                        multiplier = 1f;
                        break;
                    case "UppercutLeft":
                    case "UppercutRight":
                        if (attackerIsLight && receiverIsHeavy)
                            animTrigger = GetRecieveaSmallUppercut();
                        else if (attackerIsLight && receiverIsLight)
                            animTrigger = GetRecieveaBigUppercut();
                        else if (attackerIsHeavy && receiverIsHeavy)
                            animTrigger = GetRecieveaSmallUppercut();
                        else if (attackerIsHeavy && receiverIsLight)
                            animTrigger = GetRecieveaBigUppercut();
                        multiplier = 2f;
                        break;
                    case "CrossRight":
                        if (attackerIsLight && receiverIsHeavy)
                            animTrigger = GetRecieveaSmallRightCrosstotheFace();
                        else if (attackerIsLight && receiverIsLight)
                            animTrigger = GetRecieveaBigRightCrosstotheFace();
                        else if (attackerIsHeavy && receiverIsHeavy)
                            animTrigger = GetRecieveaSmallRightCrosstotheFace();
                        else if (attackerIsHeavy && receiverIsLight)
                            animTrigger = GetRecieveaBigRightCrosstotheFace();
                        multiplier = 4f;
                        break;
                }
                break;
            case "Spine1":
                switch (hitType)
                {
                    case "CrossLeft":
                        animTrigger = GetLivershotKnockdown();
                        multiplier = 3f;
                        break;
                }
                break;
            default:
                return;
        }

        if (animTrigger == null)
            return;

        PlayHitReaction(animTrigger);
        TakeHit(attacker, multiplier, isCrit);
    }

    public virtual void TakeHit(PlayerBaseNou attacker, float regionMultiplier, bool isCrit)
    {
        float damage;
        if (isCrit)
        {
            damage = attacker.baseDamage * attacker.critMultiplier;
        }
        else
        {
            float reduction = 1f - defense;
            damage = attacker.baseDamage * reduction * regionMultiplier;
        }

        currentHealth -= Mathf.RoundToInt(damage);
        currentHealth = Mathf.Max(0, currentHealth);

        if (currentHealth == 0)
        {
            OnDefeated();
        }
    }

    protected virtual void OnDefeated()
    {
        // Poți adăuga aici animație de KO sau UI de Game Over
    }

    public void PlayHitReaction(string triggerName)
    {
        if (!string.IsNullOrEmpty(triggerName) && animator != null)
            animator.SetTrigger(triggerName);
    }

    public string GetCurrentAttackType()
    {
        return lastAttackType;
    }

    public bool TryPlayAction(string triggerName, float staminaCost)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (isPerformingAction || currentStamina < staminaCost) return false;
        if (!stateInfo.IsName("Idle")) return false;

        animator.SetTrigger(triggerName);
        ApplyStaminaCost(staminaCost);
        isPerformingAction = true;
        lastAttackType = triggerName;
        return true;
    }

    // Anim trigger/costuri abstracte
    public abstract string GetPivotLeft();
    public abstract string GetPivotRight();
    public abstract string GetBlockLeft();
    public abstract string GetBlockRight();
    public abstract string GetBlockCenter();

    public abstract string GetJabLeft();
    public abstract string GetJabRight();
    public abstract string GetUppercutLeft();
    public abstract string GetUppercutRight();
    public abstract string GetCrossLeft();
    public abstract string GetCrossRight();

    public abstract string GetShortStepForward();
    public abstract string GetShortStepBack();
    public abstract string GetShortStepLeft();
    public abstract string GetShortStepRight();

    public abstract string GetMediumStepForward();
    public abstract string GetMediumStepLeft();
    public abstract string GetMediumStepRight();

    public abstract string GetTaunt();

    public abstract string GetRecieveaBigRightCrosstotheFace();
    public abstract string GetRecieveaSmallRightCrosstotheFace();
    public abstract string GetRecieveaBigUppercut();
    public abstract string GetRecieveaSmallUppercut();
    public abstract string GetRecieveaBigPunchtotheFace();
    public abstract string GetRecieveaSmallPunchtotheFace();
    public abstract string GetLivershotKnockdown();

    public abstract float GetJabStamina();
    public abstract float GetUppercutStamina();
    public abstract float GetCrossStamina();
    public abstract float GetBlockStamina();
    public abstract float GetShortStepStamina();
    public abstract float GetMediumStepStamina();
    public abstract float GetPivotStamina();
    public abstract float GetTauntStamina();
}