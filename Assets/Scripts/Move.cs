using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Move : MonoBehaviour
{
    public string moveName;
    [SerializeField] [Range(0,100)] int accuracy;
    [SerializeField] protected Pookiemon user;
    public Types type;
    public int movePP;
    protected int currentPP;
    public int priority;

    protected Sequence seq;

    private void Awake()
    {
        currentPP = movePP;
    }

    // applys the move
    public virtual string UseMove(Pookiemon target) 
    {
        currentPP--;
        return "";
    }

    // checks if the move is a hit or miss
    public bool AttemptMove()
    {
        if (currentPP - 1 <= 0)
        {
            return false;
        }

        int roll = UnityEngine.Random.Range(0,100);
        return (roll * user.GetAccuracy() < accuracy);
    }
}
