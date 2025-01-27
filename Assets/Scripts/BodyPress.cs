using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPress : Attack
{
    public override string UseMove(Pookiemon target)
    {
        target.TakeBodyPressDamage(user, this);
        currentPP--;
        return "";
    }
}
