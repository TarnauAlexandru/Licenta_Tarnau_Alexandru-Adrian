using Codice.Client.BaseCommands;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseTest : MonoBehaviour
{

    public int maxHealth = 1000;
    public int currentHealth;

    public float maxStamina = 100f;
    public float currentStamina;
    protected float staminaRegenRate;
    protected float staminaRegenCooldown = 0f;

    public Animator animator;
    public int baseDamage;
    public float defense;
    public float critChance;
    public float critMultiplier = 1.5f;
    public bool isCrit = false;
    public string lastattack = null;

    public bool JabRightNoCrit = false;
    public bool JabLeftNoCrit = false;
    public bool UppercutRightNoCrit = false;
    public bool UppercutLeftNoCrit = false;
    public bool CrossRightNoCrit = false;
    public bool CrossLeftNoCrit = false;
    public bool JabRightCrit = false;
    public bool JabLeftCrit = false;
    public bool UppercutRightCrit = false;
    public bool UppercutLeftCrit = false;
    public bool CrossRightCrit = false;
    public bool CrossLeftCrit = false;
    public bool trytest = false;

    public bool testpassed;

    public bool JabRightNoCritTest()
    {
        TryPlayPunch(GetJabRight());
        ReceiveHit(this, "Head", lastattack, false);
        if (IsSmallPunchToTheFace()) return true; else return false;
    }

    public bool JabLeftNoCritTest()
    {
        TryPlayPunch(GetJabLeft());
        ReceiveHit(this, "Head", lastattack, false);
        if (IsSmallPunchToTheFace()) return true; else return false;
    }

    public bool UppercutRightNoCritTest()
    {
        TryPlayPunch(GetUppercutRight());
        ReceiveHit(this, "Head", lastattack, false);
        if (IsSmallUppercut()) return true; else return false;
    }

    public bool UppercutLeftNoCritTest()
    {
        TryPlayPunch(GetUppercutLeft());
        ReceiveHit(this, "Head", lastattack, false);
        if (IsSmallUppercut()) return true; else return false;
    }

    public bool CrossRightNoCritTest()
    {
        TryPlayPunch(GetCrossRight());
        ReceiveHit(this, "Head", lastattack, false);
        if (IsSmallRightCross()) return true; else return false;
    }

    public bool CrossLeftNoCritTest()
    {
        TryPlayPunch(GetCrossLeft());
        ReceiveHit(this, "Torso", lastattack, false);
        if (IsLivershotKnockdown()) return true; else return false;
    }

    public bool JabRightCritTest()
    {
        TryPlayPunch(GetJabRight());
        ReceiveHit(this, "Head", lastattack, true);
        if (IsBigPunchToTheFace()) return true; else return false;
    }

    public bool JabLeftCritTest()
    {
        TryPlayPunch(GetJabLeft());
        ReceiveHit(this, "Head", lastattack, true);
        if (IsBigPunchToTheFace()) return true; else return false;
    }

    public bool UppercutRightCritTest()
    {
        TryPlayPunch(GetUppercutRight());
        ReceiveHit(this, "Head", lastattack, true);
        if (IsBigUppercut()) return true; else return false;
    }

    public bool UppercutLeftCritTest()
    {
        TryPlayPunch(GetUppercutLeft());
        ReceiveHit(this, "Head", lastattack, true);
        if (IsBigUppercut()) return true; else return false;
    }

    public bool CrossRightCritTest()
    {
        TryPlayPunch(GetCrossRight());
        ReceiveHit(this, "Head", lastattack, true);
        if (IsBigRightCross()) return true; else return false;
    }

    public bool CrossLeftCritTest()
    {
        TryPlayPunch(GetCrossLeft());
        ReceiveHit(this, "Torso", lastattack, true);
        if (IsLivershotKnockdown()) return true; else return false;
    }

    protected virtual void update()
    {

        /*if (JabRightNoCrit) testpassed = JabRightNoCritTest();
        if (JabLeftNoCrit) testpassed = JabLeftNoCritTest();
        if (UppercutRightNoCrit) testpassed = UppercutRightNoCritTest();
        if (UppercutLeftNoCrit) testpassed = UppercutLeftNoCritTest();
        if (CrossRightNoCrit) testpassed = CrossRightNoCritTest();
        if (CrossLeftNoCrit) testpassed = CrossLeftNoCritTest();
        if (JabRightCrit) testpassed = JabRightCritTest();
        if (JabLeftCrit) testpassed = JabLeftCritTest();
        if (UppercutRightCrit) testpassed = UppercutRightCritTest();
        if (UppercutLeftCrit) testpassed = UppercutLeftCritTest();
        if (CrossRightCrit) testpassed = CrossRightCritTest();
        if (CrossLeftCrit) testpassed = CrossLeftCritTest(); */


    }


    public PlayerBaseTest attacker;



    public virtual void Awake()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        animator = GetComponent<Animator>();
    }

    public virtual void Test()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        animator = GetComponent<Animator>();
    }

    public void TestAnimation(string Animtrigger)
    {
        animator.SetTrigger(Animtrigger);
    }

    public virtual void TakeHit(PlayerBaseTest attacker, float regionMultiplier, bool isCrit)
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

    public bool IsPunching()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(1);
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

    public bool IsBigUppercut()
    {
        AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(2);
        if(stateinfo.IsName("Receive a Big Uppercut"))
        {
            return true;
        }
        else return false;
    }

    public bool IsBigRightCross()
    {
        AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(2);
        if (stateinfo.IsName("Receive a Big Right Cross to the Face"))
        {
            return true;
        }
        else return false;
    }

    public bool IsBigPunchToTheFace()
    {
        AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(2);
        if (stateinfo.IsName("Receive a Big Punch to the Face"))
        {
            return true;
        }
        else return false;
    }

    public bool IsLivershotKnockdown()
    {
        AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(3);
        if (stateinfo.IsName("Livershot Knockdown"))
        {
            return true;
        }
        else return false;
    }

    public bool IsSmallUppercut()
    {
        AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(2);
        if (stateinfo.IsName("Receive a Small Uppercut"))
        {
            return true;
        }
        else return false;
    }

    public bool IsIdle()
    {
        AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateinfo.IsName("Idle"))
        {
            return true;
        }
        else return false;
    }

    public bool IsSmallRightCross()
    {
        AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(2);
        if (stateinfo.IsName("Receive a Small Right Cross to the Face"))
        {
            return true;
        }
        else return false;
    }

    public bool IsSmallPunchToTheFace()
    {
        AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(2);
        if (stateinfo.IsName("Receive a Small Punch to the Face"))
        {
            return true;
        }
        else return false;
    }

    public bool TryPlayPunch(string trigger)
    {
        if (IsPunching()) return false;
        if (IsHit()) return false;
        animator.SetTrigger(trigger);
        lastattack = trigger;
        return true;
    }

    public bool TryPlayHit(string trigger)
    {
        animator.SetTrigger(trigger);
        return true;
    }

    public bool ReceiveHit(PlayerBaseTest attacker, string hitTargetBone, string hitType, bool isCrit)
    {
        string animTrigger = null;

        switch (hitTargetBone)
        {
            case "Head":
                switch (hitType)
                {
                    case "Jab Left":
                        if (!isCrit) animTrigger = GetRecieveaSmallPunchtotheFace(); else animTrigger = GetRecieveaBigPunchtotheFace();
                        break;
                    case "Jab Right":
                        if (!isCrit) animTrigger = GetRecieveaSmallPunchtotheFace(); else animTrigger = GetRecieveaBigPunchtotheFace();
                        break;
                    case "Uppercut Left":
                        if (!isCrit) animTrigger = GetRecieveaSmallUppercut(); else animTrigger = GetRecieveaBigUppercut();
                        break;
                    case "Uppercut Right":
                        if (!isCrit) animTrigger = GetRecieveaSmallUppercut(); else animTrigger = GetRecieveaBigUppercut();
                        break;
                    case "Cross Right":
                        if (!isCrit) animTrigger = GetRecieveaSmallRightCrosstotheFace(); else animTrigger = GetRecieveaBigRightCrosstotheFace();
                        break;
                    default: break;
                }
                break;

            case "Torso":
                switch (hitType)
                {
                    case "Cross Left": animTrigger = GetLivershotKnockdown();break;
                    default: break;
                }
                break;
            default:
                return false;

        }
        if (animTrigger == null) return false;
        return TryPlayHit(animTrigger);

    }

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

}
 
