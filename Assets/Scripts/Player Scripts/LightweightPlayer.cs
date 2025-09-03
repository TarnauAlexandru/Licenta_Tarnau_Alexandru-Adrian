public class LightweightPlayer : PlayerBaseNou
{
    public override void Awake()
    {
        base.Awake();
        baseDamage = 80;
        defense = 0.4f;
        critChance = 0.2f;
        staminaRegenRate = 10f;

    }

    public override string GetPivotLeft() => "Pivot Left";
    public override string GetPivotRight() => "Pivot Right";
    public override string GetBlockLeft() => "Block Left";
    public override string GetBlockRight() => "Block Right";
    public override string GetBlockCenter() => "Block Center";
    public override string GetJabLeft() => "Jab Left";
    public override string GetJabRight() => "Jab Right";
    public override string GetUppercutLeft() => "Uppercut Left";
    public override string GetUppercutRight() => "Uppercut Right";
    public override string GetCrossLeft() => "Cross Left";
    public override string GetCrossRight() => "Cross Right";
    public override string GetStepForward() => "Step Forward";
    public override string GetStepBack() => "Step Backward";
    public override string GetStepLeft() => "Left Side Step";
    public override string GetStepRight() => "Right Side Step";
    public override string GetTaunt() => "Taunt";
    public override string GetRecieveaBigRightCrosstotheFace() => "Recieve a Big Right Cross to the Face";
    public override string GetRecieveaSmallRightCrosstotheFace() => "Recieve a Small Right Cross to the Face";
    public override string GetRecieveaBigUppercut() => "Recieve a Big Uppercut";
    public override string GetRecieveaSmallUppercut() => "Recieve a Small Uppercut";
    public override string GetRecieveaBigPunchtotheFace() => "Recieve a Big Punch to the Face";
    public override string GetRecieveaSmallPunchtotheFace() => "Recieve a Small Punch to the Face";
    public override string GetLivershotKnockdown() => "Livershot Knockdown";
    public override float GetJabStamina() => 10f;
    public override float GetUppercutStamina() => 11f;
    public override float GetCrossStamina() => 12f;
    public override float GetBlockStamina() => 5f;
    public override float GetPivotStamina() => 5f;
    public override float GetStepStamina() => 0f;
    public override float GetTauntStamina() => 5f;
}