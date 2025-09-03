using UnityEngine;

public class HandHitDetector : MonoBehaviour
{
    public string handName; // "RightHand" sau "LeftHand"
    public PlayerBaseNou owner; // setezi din inspector sau din script la start

    private void OnTriggerEnter(Collider other)
    {
        var opponent = other.GetComponentInParent<PlayerBaseNou>();
        if (opponent != null && owner != null)
        {
            opponent.ReceiveHit(
                attacker: owner,
                //hitSourceBone: handName,
                hitTargetBone: other.gameObject.name,
                hitType: owner.GetCurrentAttackType(),
                isCrit: false // sau logica ta de crit
            );
        }
    }
}