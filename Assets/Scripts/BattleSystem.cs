using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { START, P1TURN, P2TURN, BATTLE, P1SWITCH, P2SWITCH}

public class BattleSystem : MonoBehaviour
{
    public static BattleSystem instance;

    [Header("Player Configurations")]
    [SerializeField] Player player1;
    [SerializeField] Player player2;
    private Player currentPlayer;
    public Player CurrentPlayer { get { return currentPlayer; } }

    [Header("UI")]
    [SerializeField] private HUD playerHUD;

    [Header("Timing Configurations")]
    // timing parameters
    [SerializeField] private float setUpBattleTime = 2f;
    [SerializeField] private float startBattleTime = 2f;

    private BattleState currentState;
    private Queue<BattleAction> battleNarration;

    public static event Action TurnStart;
    public static event Action TurnEnd;
    private AudioSource audioSource;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        currentState = BattleState.START;
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(SetupBattle());
    }

    private IEnumerator SetupBattle()
    {
        // spawn all pookiemon at their respective battle positions and turn them all off
        player1.InitPlayer();
        player2.InitPlayer();

        playerHUD.ShowNarration("Let the Pookiemon battle begin!", true);

        yield return new WaitForSeconds(setUpBattleTime);

        currentState = BattleState.START;
        NextPhase();
    }

    private void PlayerTurn()
    {
        playerHUD.ShowActionPrompt($"What will {currentPlayer.Pookiemon.pookiemonName.ToUpper()} do?");
    }

    // sort the move order queue and determine outcomes (calculate things that need to be caclulated) 
    private void SetUpBattle()
    {
        List<BattleAction> actions = new List<BattleAction>();
        actions.Add(player1.currentMove); actions.Add(player2.currentMove);
        actions.Sort();
        StartCoroutine(BeginBattle(actions));
    }

    private IEnumerator BeginBattle(List<BattleAction> actions)
    {
        TurnStart?.Invoke();
        actions[0].ApplyAction();
        playerHUD.ShowNarration(actions[0].NarrationLine, true);
        yield return new WaitForSeconds(setUpBattleTime);
        actions[1].ApplyAction();
        playerHUD.ShowNarration(actions[1].NarrationLine);
        TurnEnd?.Invoke();
        yield return new WaitForSeconds(setUpBattleTime);
        if (!CheckGameOver()) { NextPhase(); }
        yield return null;
    }

    private void NextPhase()
    {
        switch (currentState)
        {
            case BattleState.START:
                currentState = BattleState.P1TURN;
                currentPlayer = player1;
                PlayerTurn();
                break;
            case BattleState.P1TURN:
                currentState = BattleState.P2TURN;
                currentPlayer = player2;
                PlayerTurn();
                break;
            case BattleState.P2TURN:
                currentPlayer = null;
                currentState = BattleState.BATTLE;
                SetUpBattle();
                break;
            case BattleState.BATTLE:
                currentPlayer = player1;
                currentState = BattleState.P1SWITCH;
                if (player1.Pookiemon.IsDead)
                {
                    audioSource.PlayOneShot(player1.Pookiemon.PookiemonDieSFX);
                    OnPookiemonSelect();
                }
                else
                {
                    NextPhase();
                }
                break;
            case BattleState.P1SWITCH:
                currentPlayer = player2;
                currentState = BattleState.P2SWITCH;
                if (player2.Pookiemon.IsDead)
                {
                    audioSource.PlayOneShot(player2.Pookiemon.PookiemonDieSFX);
                    OnPookiemonSelect();
                }
                else
                {
                    NextPhase();
                }
                break;
            case BattleState.P2SWITCH:
                currentState = BattleState.P1TURN;
                currentPlayer = player1;
                PlayerTurn();
                break;
        }
    }

    // called after the battle is over
    private bool CheckGameOver()
    {
        bool teamOneDead = CheckTeamIsDead(player1.Team);
        bool teamTwoDead = CheckTeamIsDead(player2.Team);

        if (teamOneDead && teamTwoDead) // tie state
        {
            playerHUD.ShowNarration("All Pookiemon have fainted. It's a tie!");
            return true;
        }
        else if (teamOneDead)
        {
            playerHUD.ShowNarration("Player 1 defeated. Player 2 wins!");
            return true;
        }
        else if (teamTwoDead)
        {
            playerHUD.ShowNarration("Player 2 defeated. Player 1 wins!");
            return true;
        }
        return false;
    }

    // checks a players team to see if it is completely knocked out
    private bool CheckTeamIsDead(List<Pookiemon> playerTeam)
    {
        foreach (Pookiemon p in playerTeam)
        {
            if (!p.IsDead)
                return false;
        }

        return true;
    }

    #region Action Prompt Buttons
    public void OnRun()
    {
        playerHUD.ShowActionPrompt("Can't run, bozo!");
    }

    public void OnBag()
    {
        playerHUD.ShowActionPrompt("You a broke af!");
    }

    public void OnFight()
    {
        if (currentState != BattleState.P1TURN && currentState != BattleState.P2TURN)
            return;

        playerHUD.ShowFightPrompt(currentPlayer.Pookiemon);
    }

    public void OnPookiemonSelect()
    {
        if (currentState == BattleState.START || currentState == BattleState.BATTLE)
            return;

        playerHUD.ShowPookiemonSelect(currentPlayer.Team);
        // queue up the pookiemon select action
    }

    #endregion

    public void OnMoveSelected(Move move)
    {
        if (currentState != BattleState.P1TURN && currentState != BattleState.P2TURN)
            return;

        BattleMoveAction action = new BattleMoveAction();
        bool hit = move.AttemptMove();
        action.SetAction(currentPlayer, currentPlayer == player1 ? player2 : player1, move, hit);
        currentPlayer.currentMove = action;
        NextPhase();
    }

    //Called by the switch in button
    public void OnPookiemonSwitched(Pookiemon p)
    {
        if (currentState != BattleState.P1TURN && currentState != BattleState.P2TURN)
            return;

        BattleSwitchAction action = new BattleSwitchAction();
        action.SetAction(currentPlayer, currentPlayer == player1 ? player2 : player1, p);
        currentPlayer.currentMove = action;
        NextPhase();
    }
}