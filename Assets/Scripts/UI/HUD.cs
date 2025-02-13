using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public static HUD instance;

    [Header("Narration")]
    [SerializeField] private GameObject narrationUI;
    [SerializeField] private TMP_Text narrationText;

    [Header("Actions")]
    [SerializeField] private GameObject actionPromptUI;
    [SerializeField] private TMP_Text actionPromptText;

    [Header("Fight")]
    [SerializeField] private GameObject fightPromptUI;
    [SerializeField] private List<MoveButton> moveButtons;
    [SerializeField] private TMP_Text pp;
    [SerializeField] private TMP_Text type;

    [Header("Pookemon Select")]
    [SerializeField] private GameObject pookiemonSelectUI;
    [SerializeField] private List<PookiemonSelectionButton> selectionButtons;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        HideAll();
    }

    public void Back()
    {
        ShowActionPrompt($"What will {BattleSystem.instance.CurrentPlayer.Pookiemon.PookiemonData.pookiemonName.ToUpper()} do?");
    }

    private void HideAll()
    {
        narrationUI.SetActive(false);
        actionPromptUI.SetActive(false);
        fightPromptUI.SetActive(false);
        pookiemonSelectUI.SetActive(false);
    }

    public void ShowNarration(string text, bool prepUI = false)
    {
        if (prepUI)
            HideAll();
        narrationUI.SetActive(true);
        narrationText.text = text;
    }

    public void ShowActionPrompt(string text)
    {
        HideAll();
        actionPromptUI.SetActive(true);
        actionPromptText.text = text;
    }

    public void ShowFightPrompt(Pookiemon pookie)
    {
        HideAll();
        fightPromptUI.SetActive(true);

        for (int i = 0; i < moveButtons.Count; ++i)
        {
            moveButtons[i].Init(pookie.Moves[i]);
        }

        pp.text = "";
        type.text = "";
    }

    public void SetMovePPandType(int _pp, string _type)
    {
        pp.text = $"PP: {_pp}";
        type.text = _type.ToUpper();
    }

    public void ShowPookiemonSelect(List<Pookiemon> pookies)
    {
        for (int i = 0; i < selectionButtons.Count; i++)
        {
            selectionButtons[i].Init(pookies[i]);
        }
        HideAll();
        pookiemonSelectUI.SetActive(true);
        // do more stuff to init the selection buttons
    }
}
