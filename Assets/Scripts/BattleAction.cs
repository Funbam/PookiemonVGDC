using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// just a base class
public class BattleAction : IComparable<BattleAction>
{
    protected Player activePlayer;
    protected Player opposingPlayer;
    protected string narrationLine;
    public string NarrationLine { get { return narrationLine; } }
    
    protected Sequence seq;

    public bool isAnimationDone()
    {
        return seq == null || !seq.IsActive();
    }



    public virtual void SetAction(Player _active, Player _target)
    {
        activePlayer = _active;
        opposingPlayer = _target;
    }

    public virtual void ApplyAction()
    {
        Debug.Log($"Override this. {activePlayer.playerName} did something to {opposingPlayer.playerName}.");
    }

    public int CompareTo(BattleAction other)
    {
        if (this.GetType() == typeof(BattleSwitchAction))
        {
            return -1;
        }

        else if (other.GetType() == typeof(BattleSwitchAction))
        {
            return 1;
        }

        else
        {
            if(((BattleMoveAction)this).MOVE.priority > ((BattleMoveAction)other).MOVE.priority)
            {
                return -1;
            }
            else if(((BattleMoveAction)this).MOVE.priority < ((BattleMoveAction)other).MOVE.priority)
            {
                return 1;
            }
            else
            {
                return activePlayer.Pookiemon.GetStat(Stats.SPEED) >= opposingPlayer.Pookiemon.GetStat(Stats.SPEED) ? -1 : -1;
            }
        }
    }
}
/*
// move happens "attack lands"
    move name and stats
// player switches out pookiemon
    - "player switchedo out their Pookiemon to ____"
    - ative player's pookiemon
// player pookiemon faints
    "____ fainted."
    - ative player's pookiemon
// move fails -- soley dialogue
    - move name

ALL
- current player vs "enemy player"
*/