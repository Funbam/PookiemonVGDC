using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatBuff : Move
{
    [SerializeField] int stageAmount;
    [SerializeField] Stats stat;
    public override string UseMove(Pookiemon target)
    {
        return base.UseMove(target);
        user.ApplyStatChange(stat, stageAmount);
    }
}
