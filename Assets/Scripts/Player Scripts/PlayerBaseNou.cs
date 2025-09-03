using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEditor;
using UnityEditor.Experimental;
using System;
using UnityEngine;
using Unity.IO.LowLevel.Unsafe;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Analytics;


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
    public bool isCrit = false;

    protected Animator animator;
    protected bool isPerformingAction = false;
    protected string pendingStepTrigger = null;
    protected string lastAttackType;
    public int spawnindex;
    public bool winnerstatus; 
    public PlayerBaseNou opponent;
    public bool opponentSet = false;

    protected string lastReceivedHitType;
    protected string lastReceivedRegion;
    static bool s_roundStarted = false;
    static float s_roundStartTime = 0f;


    public virtual void Awake()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        animator = GetComponent<Animator>();
        PushHealth();
        PushStamina();

        if (!s_roundStarted)
        {
            s_roundStarted = true;
            s_roundStartTime = Time.time;
            UnityAnalytics("ua_round_started", new Dictionary<string, object>{{ "scene", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name }});
        }

    }

    protected virtual void Update()
    {
        if (CanRegenerateStamina())
            RegenerateStamina();
        else
            staminaRegenCooldown -= Time.deltaTime;

    }

    protected virtual bool CanRegenerateStamina()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        return staminaRegenCooldown <= 0f && state.IsName("Idle");
    }

    protected void RegenerateStamina()
    {
        currentStamina = Mathf.Min(maxStamina, currentStamina + staminaRegenRate * Time.deltaTime);
        PushStamina();
    }

    public void SetSpawnIndex(int index)
    {
        spawnindex = index;
    }

    private void setOpponent(PlayerBaseNou oppo)
    {
        opponent = oppo;
    }



    public void ApplyStaminaCost(float amount)
    {
        currentStamina = Mathf.Max(0f, currentStamina - amount);
        UnityEngine.Debug.Log($"[{gameObject.name}] Applied stamina cost: {amount}. Current stamina: {currentStamina}", this);
        PushStamina();
    }

    public void TriggerStaminaBreak()
    {
        currentStamina *= 0.2f;
        staminaRegenCooldown = 2f;
        PushStamina();
    }

    public void ReceiveHit(PlayerBaseNou attacker, string hitTargetBone, string hitType, bool isCrit)
    {
        if(!opponentSet) { setOpponent(attacker); opponentSet = true; }
        string animTrigger = null;
        float multiplier = 1f;

        lastReceivedHitType = hitType; lastReceivedRegion = hitTargetBone;

        switch (hitTargetBone)
        {
            case "Head":
                switch (hitType)
                {
                    case "Jab Left":
                        if (!isCrit) animTrigger = GetRecieveaSmallPunchtotheFace(); else animTrigger = GetRecieveaBigPunchtotheFace();
                        multiplier = 1f; break;
                    case "Jab Right":
                        if (!isCrit) animTrigger = GetRecieveaSmallPunchtotheFace(); else animTrigger = GetRecieveaBigPunchtotheFace();
                        multiplier = 1f; break;
                    case "Uppercut Left": if (!isCrit) animTrigger = GetRecieveaSmallUppercut(); else animTrigger = GetRecieveaBigUppercut();
                        multiplier = 2f; break;
                    case "Uppercut Right": if (!isCrit) animTrigger = GetRecieveaSmallUppercut(); else animTrigger = GetRecieveaBigUppercut();
                        multiplier = 2f; break;
                    case "Cross Right":
                        if (!isCrit) animTrigger = GetRecieveaSmallRightCrosstotheFace(); else animTrigger = GetRecieveaBigRightCrosstotheFace();
                        multiplier = 4f;break;
                    default: break;
                }
                break;
                
            case "Torso":
                switch (hitType)
                {
                    case "Cross Left": animTrigger = GetLivershotKnockdown(); multiplier = 3f; break;
                    default: break;
                } break;

            case "Blocked":
                return;
            case "Hit":
                return;
            default:
                return;
                
        }
        if (animTrigger == null) return;
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
        PushHealth();
        Debug.Log($"[{gameObject.name}] A primit {damage} daune de la {attacker.gameObject.name}. Sănătate curentă: {currentHealth}/{maxHealth}", this);
        attacker.lastAttackType = "Hit";

        attacker.UnityAnalytics("ua_hit_landed", new Dictionary<string, object>{{ "move", attacker.lastAttackType },{ "region", lastReceivedRegion },{ "is_crit", isCrit },{ "damage", damage },{ "defender_name", gameObject.name },{ "defender_health_after", currentHealth }});
        UnityAnalytics("ua_hit_received", new Dictionary<string, object>{{ "received_from", attacker.gameObject.name },{ "move", lastReceivedHitType },{ "region", lastReceivedRegion },{ "is_crit", isCrit },{ "damage", damage },{ "health_after", currentHealth }
});
        if (currentHealth == 0)
        {
            OnDefeated();
        }
    }

    protected virtual void OnDefeated()
    {
        winnerstatus = false;
        opponent.winnerstatus = true;
        animator.SetTrigger("Defeated");

        MultiplayerGameResult.WinnerIndex = opponent.spawnindex;
        MultiplayerGameResult.LoserIndex = this.spawnindex;

        float roundDuration = s_roundStarted ? (Time.time - s_roundStartTime) : 0f;

        UnityAnalytics("ua_match_result", new Dictionary<string, object>{{ "winner_name", opponent.gameObject.name },{ "loser_name", gameObject.name },{ "round_time_s", roundDuration }});
        UnityAnalytics("ua_round_ended", new Dictionary<string, object>{{ "duration_s", roundDuration }});

        try { AnalyticsService.Instance.Flush(); } catch { }

        StartCoroutine(WaitAndLoadGameOver());
    }

    private System.Collections.IEnumerator WaitAndLoadGameOver()
    {
        yield return new WaitForSeconds(1.5f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
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

    public void SetBlockedAttack()
    {
        lastAttackType = "Blocked";
    }

    public bool SetIsCrit()
    {
        return UnityEngine.Random.value < critChance;
    }

    public bool GetIsCrit()
    {
        return isCrit;
    }

    public bool AITryPlayAction(string triggerName, float staminaCost)
    {
        Debug.Log($"[TryPlayAction] Se încearcă să pornească animația: {triggerName}");
        if (isPerformingAction || (currentStamina < staminaCost)) return false;

        SnapRotationToNearest90();
        animator.SetTrigger(triggerName);
        ApplyStaminaCost(staminaCost);
        lastAttackType = triggerName;
        isCrit = SetIsCrit();
        return true;
    } 

    public bool TryPlayAction(string triggerName, float staminaCost)
    {
        Debug.Log($"[TryPlayAction] Se încearcă să pornească animația: {triggerName}");
        if (!CanPlayAction(staminaCost)) { Debug.Log("TryPlayAction a returnat fals la CanPlayAction"); return false; }
        if (IsPunching()) { Debug.Log("TryPlayAction a returnat fals la if (IsPunching) "); return false; }
        if (IsBlocking()) { Debug.Log("TryPlayAction a returnat fals la if (IsBlocking) "); return false; }
        SnapRotationToNearest90();
        animator.SetTrigger(triggerName);
        ApplyStaminaCost(staminaCost);
        return true;
    }

    public bool TryPlayPivot(string triggerName, float staminaCost)
    {
        Debug.Log($"[TryPlayPivot] Se încearcă să pornească pivotul: {triggerName}");
        if(IsPunching()) { Debug.Log("TryPlayPivot a returnat fals la if (IsPunching) "); return false; }
        if(IsBlocking()) { Debug.Log("TryPlayPivot a returnat fals la if (IsBlocking) "); return false; }
        if (IsHit()) { Debug.Log("TryPlayPivot a returnat fals la if(IsHit()) "); return false; }
        if (!isIdle()) { Debug.Log("TryPlayPivot a returnat fals la if (!isIdle()) "); return false; }
        if (currentStamina < staminaCost)
        {
            Debug.Log("TryPlayPivot a returnat fals la if (currentStamina < staminaCost) ");
            return false;
        }
        SnapRotationToNearest90();
        animator.SetTrigger(triggerName);
        ApplyStaminaCost(staminaCost);
        return true;
    }

    public bool TryPlayJab(string triggerName, float staminaCost)
    {
        Debug.Log($"[TryPlayJab] Se încearcă să pornească jab-ul: {triggerName}");
        if (!CanPlayAction(staminaCost)) 
        { 
          Debug.Log("TryPlayJab a returnat fals la CanPlayAction");
          return false; 
        }
        if (IsHit()) { Debug.Log("TryPlayJab a returnat fals la if(IsHit()) "); return false; }
        if (IsPunching()) { Debug.Log("TryPlayJab a returnat fals la if (IsPunching) "); return false; }
        ApplyStaminaCost(staminaCost);
        lastAttackType = triggerName;
        isCrit = SetIsCrit();
        SnapRotationToNearest90();
        animator.SetTrigger(triggerName);

        UnityAnalytics("ua_attack_thrown", new Dictionary<string, object>{{ "move", triggerName },{ "is_crit_roll", isCrit },{ "stamina_after", currentStamina }});
        return true;
    }

    public bool TryPlayUppercut(string triggerName, float staminaCost)
    {
        Debug.Log($"[TryPlayUppercut] Se încearcă să pornească uppercut-ul: {triggerName}");
        if (!CanPlayAction(staminaCost)) { Debug.Log("TryPlayUppercut a returnat fals la CanPlayAction"); return false; }
        if (!canCombo()) { Debug.Log("TryPlayUppercut a returnat fals la if (!canCombo()) "); return false; }
        switch (triggerName)
        {
            case "Uppercut Left":
                if (animator.GetCurrentAnimatorStateInfo(1).IsName("Jab Left"))
                { Debug.Log("TryPlayUppercut a returnat fals la if(animator.GetCurrentAnimatorStateInfo(1).IsName(\"Jab Left\")) ");  return false; }
                break;
            case "Uppercut Right":
                if (animator.GetCurrentAnimatorStateInfo(1).IsName("Jab Right")) { Debug.Log("TryPlayUppercut a returnat fals la if(animator.GetCurrentAnimatorStateInfo(1).IsName(\"Jab Right\")) "); return false; }
                break;
            default:
                return false;
        }
        ApplyStaminaCost(staminaCost);
        lastAttackType = triggerName;
        isCrit = SetIsCrit();
        SnapRotationToNearest90();
        animator.SetTrigger(triggerName);
        UnityAnalytics("ua_attack_thrown", new Dictionary<string, object> { { "move", triggerName }, { "is_crit_roll", isCrit }, { "stamina_after", currentStamina } });
        return true;
    }

    public bool TryPlayCross(string triggerName, float staminaCost)
    {
        Debug.Log($"[TryPlayCross] Se încearcă să pornească cross-ul: {triggerName}");
        if (!CanPlayAction(staminaCost)) { Debug.Log("TryPlayCross a returnat fals la CanPlayAction"); return false; }
        if (!canCombo()) { Debug.Log("TryPlayCross a returnat fals la if (!canCombo()) "); return false; }
        switch (triggerName)
        {
            case "Cross Left":
                if (animator.GetCurrentAnimatorStateInfo(1).IsName("Jab Left")){ Debug.Log("TryPlayCross a returnat fals la if (!(animator.GetCurrentAnimatorStateInfo(1).IsName(\"Empty\") || animator.GetCurrentAnimatorStateInfo(1).IsName(\"Jab Right\")) "); return false; }
                break;
            case "Cross Right":
                if (animator.GetCurrentAnimatorStateInfo(1).IsName("Jab Right")) { Debug.Log("TryPlayCross a returnat fals la if (!(animator.GetCurrentAnimatorStateInfo(1).IsName(\"Empty\") || animator.GetCurrentAnimatorStateInfo(1).IsName(\"Jab Left\")) "); return false; }
                break;
            default:
                return false;
        }
        ApplyStaminaCost(staminaCost);
        lastAttackType = triggerName;
        isCrit = SetIsCrit();
        SnapRotationToNearest90();
        animator.SetTrigger(triggerName);
        UnityAnalytics("ua_attack_thrown", new Dictionary<string, object> { { "move", triggerName }, { "is_crit_roll", isCrit }, { "stamina_after", currentStamina } });
        return true;
    }

    private bool CanPlayAction(float staminaCost)
    {
        if (currentStamina < staminaCost) { Debug.Log("CanPlayAction a returnat fals la if (currentStamina < staminaCost) "); return false; }
        if(IsHit()) { Debug.Log("CanPlayAction a returnat fals la if(IsHit()) "); return false; }

        return true;
    }

    public bool isIdle()
    {
        AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateinfo.IsName("Idle")) return true;
        else return false;
    }

    public bool TryPlayMove(string triggerName, float staminaCost)
    {
        Debug.Log("TryPlayMove a intrat in functie");
        AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(0);
        Debug.Log($"[TryPlayMove] Se încearcă să pornească mișcarea: {triggerName}");
        if (!isIdle()) { Debug.Log("TryPlayMove a returnat fals la if (!stateinfo.IsName(\"Idle\")) "); return false; }
        if (currentStamina < staminaCost) return false;
        if (IsHit()) { Debug.Log("TryPlayMove a returnat fals la if(IsHit()) "); return false; }

        SnapRotationToNearest90();
        animator.SetTrigger(triggerName);

        ApplyStaminaCost(staminaCost);
        return true;
    }

    public bool IsHit()
    {
        AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(2); 
        AnimatorStateInfo knockdowninfo = animator.GetCurrentAnimatorStateInfo(3); 

        if (stateinfo.IsName("Receive a Small Uppercut") ||
            stateinfo.IsName("Receive a Small Punch to the Face") ||
            stateinfo.IsName("Receive a Small Right Cross to the Face") ||
            stateinfo.IsName("Receive a Big Right Cross to the Face") ||
            stateinfo.IsName("Receive a Big Punch to the Face") ||
            stateinfo.IsName("Receive a Big Uppercut") || knockdowninfo.IsName("Livershot Knockdown"))
        {
            return true;
        }
        else return false;
    }

    public bool IsBlocking()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(1);

        if (stateInfo.IsName("Block Right") ||
            stateInfo.IsName("Block Center") ||
            stateInfo.IsName("Block Left"))
        {
            return true;
        } else return false;
    }

    public bool IsPunching()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(1);
        // Punching states: return true
        if (stateInfo.IsName("Jab Left") ||
            stateInfo.IsName("Jab Right") ||
            stateInfo.IsName("Uppercut Left") ||
            stateInfo.IsName("Uppercut Right") ||
            stateInfo.IsName("Cross Left") ||
            stateInfo.IsName("Cross Right"))
        {
            return true;
        }
        else return false;
    }

    public bool canCombo()
        {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(1);
        if (stateInfo.IsName("Uppercut Left") ||
            stateInfo.IsName("Uppercut Right") ||
            stateInfo.IsName("Cross Left") ||
            stateInfo.IsName("Cross Right"))
        {
            return false;
        }
        else return true;
    }

    private void SnapRotationToNearest90()
    {
        float y = transform.eulerAngles.y;
        float snappedY = Mathf.Round(y / 90f) * 90f;
        transform.rotation = Quaternion.Euler(0f, snappedY, 0f);
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetCurrentStamina()
    {
        return currentStamina;
    }

    public event Action<float> OnHealth01;
    public event Action<float> OnStamina01;

    void PushHealth() => OnHealth01?.Invoke(maxHealth > 0 ? (float)currentHealth / maxHealth : 0f);
    void PushStamina() => OnStamina01?.Invoke(maxStamina > 0 ? currentStamina / maxStamina : 0f);

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

    public abstract string GetStepBack();
    public abstract string GetStepForward();
    public abstract string GetStepLeft();
    public abstract string GetStepRight();

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
    public abstract float GetStepStamina();
    public abstract float GetPivotStamina();
    public abstract float GetTauntStamina();

    // Trimite un Analytics CustomData în siguranță (nu aruncă excepții în Editor)
    protected void UnityAnalytics(string eventName, Dictionary<string, object> data = null)
    {
        try
        {
            if (UnityServices.State != ServicesInitializationState.Initialized) return;

            var ev = new CustomEvent(eventName);

            if (data != null)
            {
                // doar tipuri primitive: string/int/bool/float/double/longsx
                foreach (var kv in data)
                    ev.Add(kv.Key, kv.Value);
            }

            AnalyticsService.Instance.RecordEvent(ev);   // <-- așa e corect în 6.x
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Analytics send failed for {eventName}: {e.Message}");
        }
    }

}