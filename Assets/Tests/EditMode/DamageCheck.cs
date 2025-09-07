using NUnit.Framework;
using UnityEngine;

public class DamageCheck
{
    [Test]
    public void TakeHit_RegionMultiplier_Heavyweight_AppliesCorrectly()
    {
        var attacker = new HeavyweightTest();
        attacker.Test();
        var defender = new HeavyweightTest();
        defender.Test();
        defender.currentHealth = defender.maxHealth;

        for (int regionMultiplier = 1; regionMultiplier <= 4; regionMultiplier++)
        {
            defender.currentHealth = defender.maxHealth;
            defender.TakeHit(attacker, regionMultiplier, false);

            float expectedReduction = 1f - defender.defense; // 0.5
            float expectedDamage = attacker.baseDamage * expectedReduction * regionMultiplier; // 100 * 0.5 * regionMultiplier
            int expectedHealth = defender.maxHealth - Mathf.RoundToInt(expectedDamage);

            Assert.AreEqual(expectedHealth, defender.currentHealth, $"Heavyweight failed for regionMultiplier={regionMultiplier}");
        }
    }

    [Test]
    public void TakeHit_RegionMultiplier_Lightweight_AppliesCorrectly()
    {
        var attacker = new LightweightTest();
        attacker.Test();
        var defender = new LightweightTest();
        defender.Test();
        defender.currentHealth = defender.maxHealth;

        for (int regionMultiplier = 1; regionMultiplier <= 4; regionMultiplier++)
        {
            defender.currentHealth = defender.maxHealth;
            defender.TakeHit(attacker, regionMultiplier, false);

            float expectedReduction = 1f - defender.defense; // 0.6
            float expectedDamage = attacker.baseDamage * expectedReduction * regionMultiplier; // 80 * 0.6 * regionMultiplier
            int expectedHealth = defender.maxHealth - Mathf.RoundToInt(expectedDamage);

            Assert.AreEqual(expectedHealth, defender.currentHealth, $"Lightweight failed for regionMultiplier={regionMultiplier}");
        } 
    }

    [Test]

    public void TakeHit_HeavyweightAttacks_LightweightDefends()
    {
        var attacker = new HeavyweightTest();
        attacker.Test();
        var defender = new LightweightTest();
        defender.Test();
        defender.currentHealth = defender.maxHealth;

        for (int regionMultiplier = 1; regionMultiplier <= 4; regionMultiplier++)
        {
            defender.currentHealth = defender.maxHealth;
            defender.TakeHit(attacker, regionMultiplier, false);

            float expectedReduction = 1f - defender.defense; // Lightweight defense
            float expectedDamage = attacker.baseDamage * expectedReduction * regionMultiplier;
            int expectedHealth = defender.maxHealth - Mathf.RoundToInt(expectedDamage);

            Assert.AreEqual(expectedHealth, defender.currentHealth, $"Heavyweight->Lightweight failed for regionMultiplier={regionMultiplier}");
        }
    }

    [Test]
    public void TakeHit_LightweightAttacks_HeavyweightDefends()
    {
        var attacker = new LightweightTest();
        attacker.Test();
        var defender = new HeavyweightTest();
        defender.Test();
        defender.currentHealth = defender.maxHealth;

        for (int regionMultiplier = 1; regionMultiplier <= 4; regionMultiplier++)
        {
            defender.currentHealth = defender.maxHealth;
            defender.TakeHit(attacker, regionMultiplier, false);

            float expectedReduction = 1f - defender.defense; // Heavyweight defense
            float expectedDamage = attacker.baseDamage * expectedReduction * regionMultiplier;
            int expectedHealth = defender.maxHealth - Mathf.RoundToInt(expectedDamage);

            Assert.AreEqual(expectedHealth, defender.currentHealth, $"Lightweight->Heavyweight failed for regionMultiplier={regionMultiplier}");
        }
    }

    [Test]
    public void AverageDamagePerHit_HeavyweightVsLightweight_IsWithinExpectedRange()
    {
        var attacker = new HeavyweightTest();
        attacker.Test();
        var defender = new LightweightTest();
        defender.Test();

        defender.currentHealth = defender.maxHealth;
        defender.TakeHit(attacker, 1, false);
        float damageNormal = defender.maxHealth - defender.currentHealth;

        defender.currentHealth = defender.maxHealth;
        defender.TakeHit(attacker, 1, true);
        float damageCrit = defender.maxHealth - defender.currentHealth;

        float critChance = attacker.critChance;
        float averageDamage = (1 - critChance) * damageNormal + critChance * damageCrit;

        Assert.Greater(averageDamage, 55, "Average damage is not greater than 55");
        Assert.Less(averageDamage, 65, "Average damage is not less than 65");
    }

    [Test]
    public void AverageDamagePerHit_LightweightVsHeavyweight_IsWithinExpectedRange()
    {
        var attacker = new LightweightTest();
        attacker.Test();
        var defender = new HeavyweightTest();
        defender.Test();

        defender.currentHealth = defender.maxHealth;
        defender.TakeHit(attacker, 1, false);
        float damageNormal = defender.maxHealth - defender.currentHealth;

        defender.currentHealth = defender.maxHealth;
        defender.TakeHit(attacker, 1, true);
        float damageCrit = defender.maxHealth - defender.currentHealth;

        float critChance = attacker.critChance;
        float averageDamage = (1 - critChance) * damageNormal + critChance * damageCrit;

        Assert.Greater(averageDamage, 55, "Average damage is not greater than 50");
        Assert.Less(averageDamage, 65, "Average damage is not less than 60");
    }

    [Test]
    public void AverageDamagePerHit_HeavyweightVsHeavyweight_IsWithinExpectedRange()
    {
        var attacker = new HeavyweightTest();
        attacker.Test();
        var defender = new HeavyweightTest();
        defender.Test();

        defender.currentHealth = defender.maxHealth;
        defender.TakeHit(attacker, 1, false);
        float damageNormal = defender.maxHealth - defender.currentHealth;

        defender.currentHealth = defender.maxHealth;
        defender.TakeHit(attacker, 1, true);
        float damageCrit = defender.maxHealth - defender.currentHealth;

        float critChance = attacker.critChance;
        float averageDamage = (1 - critChance) * damageNormal + critChance * damageCrit;

        Assert.Greater(averageDamage, 55, "Average damage is not greater than 50");
        Assert.Less(averageDamage, 65, "Average damage is not less than 60");
    }

    [Test]
    public void AverageDamagePerHit_LightweightVsLightweight_IsWithinExpectedRange()
    {
        var attacker = new LightweightTest();
        attacker.Test();
        var defender = new LightweightTest();
        defender.Test();

        defender.currentHealth = defender.maxHealth;
        defender.TakeHit(attacker, 1, false);
        float damageNormal = defender.maxHealth - defender.currentHealth;

        defender.currentHealth = defender.maxHealth;
        defender.TakeHit(attacker, 1, true);
        float damageCrit = defender.maxHealth - defender.currentHealth;

        float critChance = attacker.critChance;
        float averageDamage = (1 - critChance) * damageNormal + critChance * damageCrit;

        Assert.Greater(averageDamage, 55, "Average damage is not greater than 50");
        Assert.Less(averageDamage, 65, "Average damage is not less than 60");
    }
}
