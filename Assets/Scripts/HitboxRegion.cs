using UnityEngine;

public enum BodyRegion
{
    Chin, TempleLeft, TempleRight, Nose, Liver, Chest, Head, ArmLeft, ArmRight
}

public class HitboxRegion : MonoBehaviour
{
    public BodyRegion region;
    [Range(0f, 5f)] public float damageMultiplier = 1f;
}
