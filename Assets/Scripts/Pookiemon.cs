using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatMap : SerializableDictionary<Stats, int> { }
public enum Stats
{
    HP,
    ATTACK,
    DEFENSE,
    SPATTACK,
    SPDEFENSE,
    SPEED
}

public enum Types
{
    Normal, Fire, Water, Grass, Electric, Ice, Fighting, Poison, Ground,
    Flying, Psychic, Bug, Rock, Ghost, Dragon, Dark, Steel, Fairy, NULL
}


public class Pookiemon : MonoBehaviour
{
    private static readonly float[,] typeChart = {
    // Normal   Fire    Water   Grass   Electric  Ice    Fighting Poison  Ground  Flying  Psychic Bug    Rock    Ghost   Dragon  Dark    Steel   Fairy
    { 1f,      1f,     1f,     1f,     1f,       1f,    1f,      1f,     1f,     1f,     1f,     1f,     0.5f,  0f,     1f,     1f,     0.5f,  1f }, // Normal
    { 1f,      0.5f,   0.5f,   2f,     1f,       2f,    1f,      1f,     1f,     1f,     1f,     2f,     0.5f,  1f,     0.5f,   1f,     2f,    1f }, // Fire
    { 1f,      2f,     0.5f,   0.5f,   1f,       1f,    1f,      1f,     2f,     1f,     1f,     1f,     2f,    1f,     0.5f,   1f,     1f,    1f }, // Water
    { 1f,      0.5f,   2f,     0.5f,   1f,       1f,    1f,      0.5f,   2f,     0.5f,   1f,     0.5f,   2f,    1f,     0.5f,   1f,     0.5f,  1f }, // Grass
    { 1f,      1f,     1f,     0.5f,   0.5f,     1f,    1f,      1f,     2f,     1f,     1f,     1f,     1f,    1f,     0.5f,   1f,     1f,    1f }, // Electric
    { 1f,      0.5f,   0.5f,   2f,     1f,       0.5f,  1f,      1f,     1f,     2f,     1f,     1f,     1f,    1f,     2f,     1f,     0.5f,  1f }, // Ice
    { 2f,      1f,     1f,     1f,     1f,       2f,    1f,      0.5f,   1f,     0.5f,   0.5f,   0.5f,   2f,    0f,     1f,     2f,     2f,    0.5f }, // Fighting
    { 1f,      1f,     1f,     2f,     1f,       1f,    1f,      0.5f,   0.5f,   1f,     1f,     1f,     0.5f,  0.5f,   1f,     1f,     0f,    2f }, // Poison
    { 1f,      2f,     1f,     0.5f,   0f,       1f,    1f,      2f,     1f,     0f,     1f,     0.5f,   2f,    1f,     1f,     1f,     2f,    1f }, // Ground
    { 1f,      1f,     1f,     2f,     1f,       1f,    2f,      1f,     1f,     1f,     1f,     2f,     0.5f,  1f,     1f,     1f,     0.5f,  1f }, // Flying
    { 1f,      1f,     1f,     1f,     1f,       1f,    2f,      2f,     1f,     1f,     0.5f,   1f,     1f,    1f,     1f,     0f,     0.5f,  1f }, // Psychic
    { 1f,      0.5f,   1f,     2f,     1f,       1f,    0.5f,    0.5f,   1f,     0.5f,   1f,     1f,     1f,    0.5f,   1f,     1f,     0.5f,  0.5f }, // Bug
    { 1f,      2f,     1f,     1f,     1f,       2f,    0.5f,    1f,     0.5f,   2f,     1f,     1f,     1f,    1f,     1f,     1f,     0.5f,  1f }, // Rock
    { 0f,      1f,     1f,     1f,     1f,       1f,    1f,      1f,     1f,     1f,     2f,     1f,     1f,    2f,     1f,     0.5f,   1f,    1f }, // Ghost
    { 1f,      1f,     1f,     1f,     1f,       1f,    1f,      1f,     1f,     1f,     1f,     1f,     1f,    1f,     2f,     1f,     0.5f,  0f }, // Dragon
    { 1f,      1f,     1f,     1f,     1f,       1f,    0.5f,    1f,     1f,     1f,     2f,     1f,     1f,    2f,     1f,     0.5f,   1f,    0.5f }, // Dark
    { 1f,      0.5f,   0.5f,   1f,     1f,       2f,    1f,      1f,     1f,     1f,     1f,     1f,     2f,    1f,     1f,     1f,     0.5f,  2f }, // Steel
    { 1f,      0.5f,   1f,     1f,     1f,       1f,    2f,      0.5f,   1f,     1f,     1f,     1f,     1f,    1f,     2f,     2f,     0.5f,  1f }  // Fairy
};


    public string pookiemonName = "";
    public Sprite sprite;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] StatMap baseStats;
    StatMap stats = new StatMap();
    StatMap statChanges = new StatMap();
    public static int LEVEL = 50;
    [SerializeField] Types type1;
    [SerializeField] Types type2;
    [SerializeField] List<Move> moves;
    public List<Move> Moves { get { return moves; } }
    [HideInInspector] public float accuracyStage = 0;
    public bool IsDead { get { return currentHealth <= 0; } }
    private int currentHealth;
    public int CurrentHealth { get { return currentHealth; } }

    [HideInInspector] public bool cantMove = false;

    [Header("Audio")]
    [SerializeField] private AudioClip pookiemonSpawnSFX;
    [SerializeField] private AudioClip pookiemonDieSFX;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public AudioClip PookiemonSpawnSFX => pookiemonSpawnSFX;
    public AudioClip PookiemonDieSFX => pookiemonDieSFX;

    public static float GetMultiplier(Types attackType, Types defend1, Types defender2)
    {
        // Base multiplier for the first type
        float multiplier = typeChart[(int)attackType, (int)defend1];

        // Apply second type multiplier if present
        if (defender2 != Types.NULL)
        {
            multiplier *= typeChart[(int)attackType, (int)defender2];
        }

        return multiplier;
    }

    public int GetStat(Stats stat)
    {
        int num = 2;
        int denom = 2;
        if (statChanges[stat] > 0)
        {
            num += 1;
        }
        else if (statChanges[stat] < 0)
        {
            denom += 1;
        }

        return (int)(stats[stat] * num/denom);
    }

    public float GetAccuracy()
    {
        int num = 3;
        int denom = 3;
        if(accuracyStage > 0)
        {
            num += 1;
        }
        else
        {
            denom += 1;
        }
        return num / denom;
    }


    public void Init()
    {
        spriteRenderer.sprite = sprite;
        foreach (Stats s in baseStats.Keys)
        {
            stats[s] = LEVEL * ((baseStats[s] / 50) + 5);
        }

        stats[Stats.HP] = LEVEL + 50 + baseStats[Stats.HP];

        currentHealth = stats[Stats.HP];
        for(int i = 0; i < 6; i++)
        {
            statChanges.Add(i, 0);
        }
        ResetChanges();
    }

    public void OnSwitch()
    {
        ResetChanges();
    }

    protected void ResetChanges()
    {
        for (int i = 0; i < 6; i++)
        {
            statChanges[i] = 0;
        }
        accuracyStage = 0;
    }

    public void ApplyStatChange(Stats stat, int stageRaise)
    {
        statChanges[stat] = Mathf.Clamp(statChanges[stat] + stageRaise, -6, 6);
    }

    public void ApplyAccuracyChange(int stageRaise)
    {
        accuracyStage = Mathf.Clamp(accuracyStage + stageRaise, -6, 6);
    }

    public int TakePhysicalDamage(Pookiemon attacker, Attack attack)
    {
        //Super Effectiveness
        float multiplier = GetMultiplier(attack.type, type1, type2);
        //Crit
        if(RollCrit(attack)) { multiplier *= 1.5f; }
        //STAB
        if(attack.type == type1 || attack.type == type2) { multiplier *= 1.5f; }
        int damage = (int)(multiplier * (2*LEVEL/5 +2) * attack.POWER * attacker.GetStat(Stats.ATTACK) / GetStat(Stats.DEFENSE) / 50);
        Debug.Log("Damage: " + damage);
        int healthLost = Mathf.Clamp(damage, currentHealth, damage);
        currentHealth -= damage;
        return healthLost;
    }

    public int TakeSpecialDamage(Pookiemon attacker, Attack attack)
    {
        //Super Effectiveness
        float multiplier = GetMultiplier(attack.type, type1, type2);
        //Crit
        if (RollCrit(attack)) { multiplier *= 1.5f; }
        //STAB
        if (attack.type == type1 || attack.type == type2) { multiplier *= 1.5f; }
        int damage = (int)(multiplier * (2 * LEVEL / 5 + 2) * attack.POWER * attacker.GetStat(Stats.SPATTACK) / GetStat(Stats.SPDEFENSE) / 50);
        Debug.Log("Damage: " + damage);
        int healthLost = Mathf.Clamp(damage, currentHealth, damage);
        currentHealth -= damage;
        return healthLost;
    }

    public int TakeBodyPressDamage(Pookiemon attacker, Attack attack)
    {
        //Super Effectiveness
        float multiplier = GetMultiplier(attack.type, type1, type2);
        //Crit
        if (RollCrit(attack)) { multiplier *= 1.5f; }
        //STAB
        if (attack.type == type1 || attack.type == type2) { multiplier *= 1.5f; }
        int damage = (int)(multiplier * (2 * LEVEL / 5 + 2) * attack.POWER * attacker.GetStat(Stats.DEFENSE) / GetStat(Stats.DEFENSE) / 50);
        Debug.Log("Damage: " + damage);
        int healthLost = Mathf.Clamp(damage, currentHealth, damage);
        currentHealth -= damage;
        return healthLost;
    }

    private bool RollCrit(Attack a)
    {
        int roll = UnityEngine.Random.Range(1, 100);
        return roll * a.critChance > 100;

    }
}
