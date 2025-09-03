using UnityEngine;

public class MenuBoxerBase : MonoBehaviour
{
    [Header("Info vizual")]
    public string boxerName;
    public int damage;
    public int stamina;
    public int crit;
    public int damageReduction;

    public GameObject menuPreviewPrefab;

    protected Animator animator;

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetWinner()
    {
        animator.SetTrigger("Victory");
    }

    public void SetLoser() 
    {
        animator.SetTrigger("Defeat");
    }

    public virtual string GetTauntAnimation() => "Taunt";
    public virtual string GetIdleAnimation() => "Idle";
}