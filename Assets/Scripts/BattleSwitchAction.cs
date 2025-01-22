using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSwitchAction : BattleAction
{
    Pookiemon switchin;
    public virtual void SetAction(Player _active, Player _target, Pookiemon _switchin)
    {
        activePlayer = _active;
        opposingPlayer = _target;
        switchin = _switchin;
    }
    public override void ApplyAction()
    {
        activePlayer.SwitchPookie(switchin);
    }
}
