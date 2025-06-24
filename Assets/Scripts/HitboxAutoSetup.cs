using System.Diagnostics;
using UnityEngine;

public class HitboxAutoSetup : MonoBehaviour
{
    [Header("Collider Settings")]
    public float colliderRadius = 0.1f;

    private void Start()
    {
        // === HEAD ZONE ===
        CreateHitbox("Head", "Hitbox_Chin", new Vector3(0f, -0.08f, 0.07f), BodyRegion.Chin, 1.2f);
        CreateHitbox("Head", "Hitbox_Temple_L", new Vector3(-0.07f, 0f, 0f), BodyRegion.TempleLeft, 1.5f);
        CreateHitbox("Head", "Hitbox_Temple_R", new Vector3(0.07f, 0f, 0f), BodyRegion.TempleRight, 1.5f);
        CreateHitbox("Head", "Hitbox_Nose", new Vector3(0f, 0f, 0.1f), BodyRegion.Nose, 1.1f);

        // === BODY ZONE ===
        CreateHitbox("Spine1", "Hitbox_Liver", new Vector3(0.08f, -0.05f, 0f), BodyRegion.Liver, 2.0f);

        // === ARMS (Low Damage Guard) ===
        CreateHitbox("LeftLowerArm", "Hitbox_LeftArm", Vector3.zero, BodyRegion.ArmLeft, 0.1f);
        CreateHitbox("RightLowerArm", "Hitbox_RightArm", Vector3.zero, BodyRegion.ArmRight , 0.1f);
    }

    private void CreateHitbox(string boneName, string name, Vector3 localOffset, BodyRegion region, float damageMult)
    {
        Transform bone = FindDeepChild(transform, boneName);
        if (bone == null)
        {
            UnityEngine.Debug.LogWarning($"Bone '{boneName}' not found.");
            return;
        }

        GameObject hitbox = new GameObject(name);
        hitbox.transform.SetParent(bone);
        hitbox.transform.localPosition = localOffset;
        hitbox.transform.localRotation = Quaternion.identity;

        var collider = hitbox.AddComponent<SphereCollider>();
        collider.radius = colliderRadius;
        collider.isTrigger = true;

        var regionComponent = hitbox.AddComponent<HitboxRegion>();
        regionComponent.region = region;
        regionComponent.damageMultiplier = damageMult;
    }

    private Transform FindDeepChild(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name) return child;
            var result = FindDeepChild(child, name);
            if (result != null) return result;
        }
        return null;
    }
}
