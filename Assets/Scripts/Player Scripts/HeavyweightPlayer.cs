public class HeavyweightPlayer : PlayerBaseNou
{
    public override void Awake()
    {
        base.Awake();
        baseDamage = 100;
        defense = 0.5f;
        critChance = 0.1f;
        staminaRegenRate = 7.5f;
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
    public override string GetShortStepForward() => "Short Step Forward";
    public override string GetShortStepBack() => "Short Step Backward";
    public override string GetShortStepLeft() => "Short Left Side Step";
    public override string GetShortStepRight() => "Short Right Side Step";
    public override string GetMediumStepForward() => "Medium Step Forward";
    public override string GetMediumStepLeft() => "Medium Left Sidestep";
    public override string GetMediumStepRight() => "Medium Right Sidestep";
    public override string GetTaunt() => "Taunt";
    public override string GetRecieveaBigRightCrosstotheFace() => "Recieve a Big Right Cross to the Face";
    public override string GetRecieveaSmallRightCrosstotheFace() => "Recieve a Small Right Cross to the Face";
    public override string GetRecieveaBigUppercut() => "Recieve a Big Uppercut";
    public override string GetRecieveaSmallUppercut() => "Recieve a Small Uppercut";
    public override string GetRecieveaBigPunchtotheFace() => "Recieve a Big Punch to the Face";
    public override string GetRecieveaSmallPunchtotheFace() => "Recieve a Small Punch to the Face";
    public override string GetLivershotKnockdown() => "Livershot Knockdown";
    public override float GetJabStamina() => 15f;
    public override float GetUppercutStamina() => 16f;
    public override float GetCrossStamina() => 17f;
    public override float GetBlockStamina() => 7f;
    public override float GetPivotStamina() => 7f;
    public override float GetShortStepStamina() => 1f;
    public override float GetMediumStepStamina() => 3f;
    public override float GetTauntStamina() => 1f;
}