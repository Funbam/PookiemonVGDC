using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// for when a pookiemon uses a move (both hits and misses)
public class BattleMoveAction : BattleAction
{
    private bool isHit;
    public bool IsHit { get { return isHit; } }
    private Move move;
    public Move MOVE { get { return move; } }

    public virtual void SetAction(Player _active, Player _target, Move _move, bool _isHit)
    {
        activePlayer = _active;
        opposingPlayer = _target;
        move = _move;
        isHit = _isHit;
    }

    public override void ApplyAction()
    {
        if (activePlayer.Pookiemon.cantMove)
        {
            narrationLine = $"{activePlayer.Pookiemon.PookiemonData.pookiemonName} can't move!";
            return;
        }

        if (isHit)
        {
            string extra = move.UseMove(opposingPlayer.Pookiemon);
            opposingPlayer.HealthUi.SetHealth(opposingPlayer.Pookiemon.CurrentHealth);
            narrationLine = $"{activePlayer.Pookiemon.PookiemonData.pookiemonName} used {move.moveName} on {opposingPlayer.Pookiemon.PookiemonData.pookiemonName}. " + extra;
            
        }
        else
        {
            narrationLine = $"{activePlayer.Pookiemon.PookiemonData.pookiemonName} missed.";
        }
    }
}
