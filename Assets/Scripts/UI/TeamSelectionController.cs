using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamSelectionController : MonoBehaviour
{
    private static int maxTeamSize = 6;

    [Header("Player Selection")]
    [SerializeField] private AllPookiemonSO allPookiemon;
    [SerializeField] private TeamSelectionButton p1Button;
    [SerializeField] private TeamSelectionButton p2Button;

    private List<Pookiemon> p1Selection;
    private List<Pookiemon> p2Selection;

    [Header("UI")]
    [SerializeField] private Image headerImage;
    [SerializeField] private Transform gridLayoutGroup;
    [SerializeField] private GameObject selectionButtonPrefab;
    [SerializeField] private TeamSelectionDisplay teamDisplay;

    [Header("Battle System")]
    [SerializeField] private GameObject selectionScreen;
    [SerializeField] private GameObject battleSystem;
    [SerializeField] private GameObject battleHUD;

    private TeamSelectionButton currButton;
    public Player CurrPlayer { get { return currButton.MyPlayer; } }

    private void Awake()
    {
        allPookiemon.Init();
        CreateSelectionGrid();

        selectionScreen.SetActive(true);
        battleSystem.SetActive(false);
        battleHUD.SetActive(false);

        p1Selection = new List<Pookiemon>();
        p2Selection = new List<Pookiemon>();

        OnActivePlayerSelected(p1Button);

        p1Button.TeamButton.onClick.AddListener(() => OnActivePlayerSelected(p1Button));
        p1Button.ReadyButton.onClick.AddListener(() => OnReadyButtonClicked(p1Button));
        p1Button.InitButton();
        p2Button.TeamButton.onClick.AddListener(() => OnActivePlayerSelected(p2Button));
        p2Button.ReadyButton.onClick.AddListener(() => OnReadyButtonClicked(p2Button));
        p2Button.InitButton();
    }

    private void CreateSelectionGrid()
    {
        foreach (Pookiemon pookie in allPookiemon.pookiemons)
        {
            PookiemonTeamSelectionButton selectionButton = Instantiate(selectionButtonPrefab, gridLayoutGroup).GetComponent<PookiemonTeamSelectionButton>();
            selectionButton.Init(this, pookie, p1Button.MyPlayer.playerColor, p2Button.MyPlayer.playerColor);
        }
    }

    private void OnActivePlayerSelected(TeamSelectionButton activeTeam)
    {
        if (currButton != null)
            currButton.SetReadyButtonInteractable(false);

        currButton = activeTeam;
        currButton.SetReadyButtonInteractable(true);

        headerImage.color = currButton.MyPlayer.playerColor;
        if (currButton.MyPlayer.playerNumber == PlayerNumber.P1)
        {
            teamDisplay.UpdateDisplay(p1Selection, currButton.MyPlayer.playerColor);
        }
        else
        {
            teamDisplay.UpdateDisplay(p2Selection, currButton.MyPlayer.playerColor);
        }
    }

    private void OnReadyButtonClicked(TeamSelectionButton activeTeam)
    {
        activeTeam.ToggleReady();

        if (p1Button.teamReady && p2Button.teamReady)
        {
            p1Button.MyPlayer.SetTeam(GetPookiemonGOs(p1Selection));
            p2Button.MyPlayer.SetTeam(GetPookiemonGOs(p2Selection));

            selectionScreen.SetActive(false);
            battleSystem.SetActive(true);
            battleHUD.SetActive(true);
        }
    }

    private List<GameObject> GetPookiemonGOs(List<Pookiemon> pookies)
    {
        List<GameObject> gos = new List<GameObject>();
        
        foreach (Pookiemon p in pookies)
        {
            gos.Add(allPookiemon.PookiemonsDict[p]);
        }   
        
        return gos;
    }

    public bool OnPookiemonSelectionButtonClick(Pookiemon pookie)
    {
        List<Pookiemon> selection = currButton.MyPlayer.playerNumber == PlayerNumber.P1 ? p1Selection : p2Selection;
        return TrySelectRemovePookiemon(pookie, selection);
    }

    public bool TrySelectRemovePookiemon(Pookiemon pookie, List<Pookiemon> currSelection)
    {
        bool success = false;
        // Check if the Pookiemon is already in their selection, if so, remove the pookiemon
        if (currSelection.Contains(pookie))
        {
            currSelection.Remove(pookie);
            success = true;
        }
        // else if the team is not full, add the pookiemon
        else if (currSelection.Count < maxTeamSize)
        {
            currSelection.Add(pookie);   
            success = true;
        }

        currButton.ReadyButton.interactable = currSelection.Count == maxTeamSize;

        teamDisplay.UpdateDisplay(currSelection);
        return success;
    }
}

[Serializable]
public class TeamSelectionButton
{
    [SerializeField] private Button playerButton;
    public Button TeamButton { get { return playerButton; } }

    [SerializeField] private Button readyButton;
    public Button ReadyButton { get { return readyButton; } }

    [SerializeField] private TMP_Text playerButtonText;
    [SerializeField] private TMP_Text readyButtonText;

    [SerializeField] private Player myPlayer;
    public Player MyPlayer { get { return myPlayer; } }

    [HideInInspector] public bool teamReady;

    public void InitButton()
    {
        playerButtonText.text = $"{myPlayer.playerNumber}: {myPlayer.playerName}";
        readyButtonText.text = "Ready";
        readyButton.interactable = false;
        teamReady = false;
    }

    public void SetReadyButtonInteractable(bool isInteractable)
    {
        readyButton.interactable = isInteractable;
    }

    public void ToggleReady()
    {
        teamReady = !teamReady;
        readyButtonText.text = teamReady ? "Cancel" : "Ready";
    }
}

[Serializable]
public class TeamSelectionDisplay
{
    [SerializeField] private List<Image> displayImages;
    [SerializeField] private Outline outline;

    public void UpdateDisplay(List<Pookiemon> currentSelection, Color color)
    {
        outline.effectColor = color;
        UpdateDisplay(currentSelection);
    }

    public void UpdateDisplay(List<Pookiemon> currentSelection)
    {
        for (int i = 0; i < displayImages.Count; i++)
        {
            // If there is a pookiemon to be added to the images, add it
            if (i < currentSelection.Count)
            {
                displayImages[i].enabled = true;
                displayImages[i].sprite = currentSelection[i].PookiemonData.sprite;
            }
            // Otherwise, disable the image component
            else
            {
                displayImages[i].enabled = false;
            }
        }
    }
}