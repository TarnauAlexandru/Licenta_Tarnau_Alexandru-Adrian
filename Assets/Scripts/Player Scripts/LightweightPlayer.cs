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

    public override string GetPivotLeft() => "Light_Pivot Left";
    public override string GetPivotRight() => "Light_Pivot Right";
    public override string GetBlockLeft() => "Light_Block Left";
    public override string GetBlockRight() => "Light_Block Right";
    public override string GetBlockCenter() => "Light_Block Center";
    public override string GetJabLeft() => "Light_Jab Left";
    public override string GetJabRight() => "Light_Jab Right";
    public override string GetUppercutLeft() => "Light_Uppercut Left";
    public override string GetUppercutRight() => "Light_Uppercut Right";
    public override string GetCrossLeft() => "Light_Cross Left";
    public override string GetCrossRight() => "Light_Cross Right";
    public override string GetShortStepForward() => "Light_Short Step Forward";
    public override string GetShortStepBack() => "Light_Short Step Backward";
    public override string GetShortStepLeft() => "Light_Short Left Side Step";
    public override string GetShortStepRight() => "Light_Short Right Side Step";
    public override string GetMediumStepForward() => "Light_Medium Step Forward";
    public override string GetMediumStepLeft() => "Light_Medium Left Sidestep";
    public override string GetMediumStepRight() => "Light_Medium Right Sidestep";
    public override string GetTaunt() => "Light_Taunt";
    public override string GetRecieveaBigRightCrosstotheFace() => "Light_Recieve a Big Right Cross to the Face";
    public override string GetRecieveaSmallRightCrosstotheFace() => "Light_Recieve a Small Right Cross to the Face";
    public override string GetRecieveaBigUppercut() => "Light_Recieve a Big Uppercut";
    public override string GetRecieveaSmallUppercut() => "Light_Recieve a Small Uppercut";
    public override string GetRecieveaBigPunchtotheFace() => "Light_Recieve a Big Punch to the Face";
    public override string GetRecieveaSmallPunchtotheFace() => "Light_Recieve a Small Punch to the Face";
    public override string GetLivershotKnockdown() => "Light_Livershot Knockdown";
    public override float GetJabStamina() => 10f;
    public override float GetUppercutStamina() => 11f;
    public override float GetCrossStamina() => 12f;
    public override float GetBlockStamina() => 5f;
    public override float GetPivotStamina() => 5f;
    public override float GetShortStepStamina() => 1f;
    public override float GetMediumStepStamina() => 2f;
    public override float GetTauntStamina() => 5f;
}