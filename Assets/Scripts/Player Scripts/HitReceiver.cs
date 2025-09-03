using UnityEngine;

public class HitReceiver : MonoBehaviour
{
    private PlayerBaseNou victimBase;
    Animator animator;

    void Awake()
    {
        victimBase = GetComponentInParent<PlayerBaseNou>();
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        AttackerMarker marker = other.GetComponent<AttackerMarker>();
        if (marker == null || marker.attackerBase == null) return;

        if (marker.attackerBase == victimBase) return;

        PlayerBaseNou attacker = marker.attackerBase;
        string hitType = attacker.GetCurrentAttackType(); 
        bool ISCrit = attacker.GetIsCrit();

        if (attacker.IsPunching())
        {
            switch (gameObject.layer)
            {

                case 6: 
                    Debug.Log($"{victimBase.name} A BLOCAT lovitura {hitType} de la {attacker.name} cu layer block");
                    if (victimBase.IsBlocking())
                        attacker.SetBlockedAttack(); 
                    break;
                case 7: 
                    victimBase.ReceiveHit(attacker, "Torso", hitType, ISCrit);
                    Debug.Log($"{victimBase.name} a fost lovit la TORSO de {attacker.name} cu {hitType}");
                    break;

                case 8: 
                    victimBase.ReceiveHit(attacker, "Head", hitType, ISCrit);
                    Debug.Log($"{victimBase.name} a fost lovit la CAP de {attacker.name} cu {hitType}");
                    break;

                default:
                    Debug.Log("Lovit într-un layer necunoscut!");
                    break;
            }
        }
    }
}
