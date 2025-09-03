
using UnityEngine;
using UnityEngine.Events;

public class HUDControllerMicroBar : MonoBehaviour
{
    [Header("Către bare (0–1) - UnityEvents (opțional)")]
    public UnityEvent<float> OnP1Health01;
    public UnityEvent<float> OnP2Health01;
    public UnityEvent<float> OnP1Stamina01;
    public UnityEvent<float> OnP2Stamina01;

    [Header("Legături directe (recomandat)")]
    [SerializeField] private MicroBarReceiver p1HealthBar;
    [SerializeField] private MicroBarReceiver p2HealthBar;
    [SerializeField] private MicroBarReceiver p1StaminaBar;
    [SerializeField] private MicroBarReceiver p2StaminaBar;

    private PlayerBaseNou p1, p2;

    public void Bind(PlayerBaseNou player1, PlayerBaseNou player2)
    {
        if (p1 != null) { p1.OnHealth01 -= HandleP1Health; p1.OnStamina01 -= HandleP1Stamina; }
        if (p2 != null) { p2.OnHealth01 -= HandleP2Health; p2.OnStamina01 -= HandleP2Stamina; }

        p1 = player1; p2 = player2;

        if (p1 != null)
        {
            p1.OnHealth01 += HandleP1Health;
            p1.OnStamina01 += HandleP1Stamina;
            HandleP1Health(p1.maxHealth > 0 ? (float)p1.currentHealth / p1.maxHealth : 0f);
            HandleP1Stamina(p1.maxStamina > 0 ? p1.currentStamina / p1.maxStamina : 0f);
        }
        if (p2 != null)
        {
            p2.OnHealth01 += HandleP2Health;
            p2.OnStamina01 += HandleP2Stamina;
            HandleP2Health(p2.maxHealth > 0 ? (float)p2.currentHealth / p2.maxHealth : 0f);
            HandleP2Stamina(p2.maxStamina > 0 ? p2.currentStamina / p2.maxStamina : 0f);
        }
    }

    void HandleP1Health(float v) { v = Mathf.Clamp01(v); OnP1Health01?.Invoke(v); if (p1HealthBar) p1HealthBar.Set01(v); }
    void HandleP2Health(float v) { v = Mathf.Clamp01(v); OnP2Health01?.Invoke(v); if (p2HealthBar) p2HealthBar.Set01(v); }
    void HandleP1Stamina(float v) { v = Mathf.Clamp01(v); OnP1Stamina01?.Invoke(v); if (p1StaminaBar) p1StaminaBar.Set01(v); }
    void HandleP2Stamina(float v) { v = Mathf.Clamp01(v); OnP2Stamina01?.Invoke(v); if (p2StaminaBar) p2StaminaBar.Set01(v); }

    [ContextMenu("TEST P1 = 60%")] void __t1() => HandleP1Health(0.6f);
    [ContextMenu("TEST P2 = 25%")] void __t2() => HandleP2Health(0.25f);
}
