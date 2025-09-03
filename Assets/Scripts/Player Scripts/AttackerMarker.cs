using UnityEngine;

public class AttackerMarker : MonoBehaviour
{
    public PlayerBaseNou attackerBase;  // Boxerul care lovește

    void Awake()
    {
        if (!attackerBase)
            attackerBase = GetComponentInParent<PlayerBaseNou>();
    }
}
