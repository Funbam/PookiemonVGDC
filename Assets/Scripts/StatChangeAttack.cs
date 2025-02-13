using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
    [Serializable]
public class EffectData
{
    public Stats statToMod;
    public int increment;
    public float chance;
    public EffectData(Stats s, int i, float c)
        {
        statToMod = s;
        increment = i;
        chance = c;
        }
}
public class StatChangeAttack : Attack
{
    [SerializeField] List<EffectData> effectsToApply;
        [SerializeField] int accuracyChange;
  public override string ExtraEffects(Pookiemon target, int damageDealt)
    {
        
        string effectChanges = "";
        foreach (EffectData effect in effectsToApply) {
            if (UnityEngine.Random.Range(0f, 1f) < effect.chance)
            {
               target.ApplyStatChange(effect.statToMod, effect.increment);
                string msg = effect.increment > 0 ? "increased!" : "decreased! ";
                effectChanges += target.PookiemonData.pookiemonName + "'s " + effect.statToMod.ToString() + " has " + msg;
            }
                }
        if (accuracyChange != 0) {
            target.ApplyAccuracyChange(accuracyChange);
            string msg = accuracyChange > 0 ? "increased! " : "decreased! ";
            effectChanges += target.PookiemonData.pookiemonName + "'s " + " accuracy has " + msg;
        }

        return effectChanges;
   
    }
}
