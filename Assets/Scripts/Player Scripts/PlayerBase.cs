using UnityEngine;

public abstract class PlayerBase : MonoBehaviour
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

    public virtual void TakeHit(PlayerBase attacker, float regionMultiplier, bool isCrit)
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

    public void PlayHitReaction(string triggerName)
    {
        if (!string.IsNullOrEmpty(triggerName) && animator != null)
            animator.SetTrigger(triggerName);
    }

    public string GetCurrentAttackType()
    {
        return lastAttackType;
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