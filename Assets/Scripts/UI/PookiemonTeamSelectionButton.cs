using UnityEngine;
using UnityEngine.UI;

public class PookiemonTeamSelectionButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image pookiemonImage;
    [SerializeField] private Image mainFrame;
    [SerializeField] private Image secondaryFrame;

    [SerializeField] private Color unselectedColor = Color.grey;
    private Color p1Color, p2Color;

    private TeamSelectionController selectionController;
    private Pookiemon pookie;
    private bool p1Selected;
    private bool p2Selected;

    private void Awake()
    {
        mainFrame.color = unselectedColor;
        secondaryFrame.color = unselectedColor;
        button.onClick.AddListener(OnClick);

        p1Selected = false;
        p2Selected = false;
    }
    private void OnClick()
    {
        bool succesSelection = selectionController.OnPookiemonSelectionButtonClick(pookie);

        PookieDex.instance.UpdateDexEntry(pookie);

        if (succesSelection)
            SelectButton(selectionController.CurrPlayer.playerNumber, 
            selectionController.CurrPlayer.playerNumber == PlayerNumber.P1 ? !p1Selected : !p2Selected);
    }

    private void SelectButton(PlayerNumber playerNum, bool isSelecting)
    {
        if (playerNum == PlayerNumber.P1)
            p1Selected = isSelecting;
        else if (playerNum == PlayerNumber.P2)
            p2Selected = isSelecting;

        // Logic to control the color of the frame based on who is selecting
        // Priority to P1 for the main color of the frame

        // Both players selected
        if (p1Selected && p2Selected)
        {
            mainFrame.color = p1Color;
            secondaryFrame.color = p2Color;
        }
        // Only P1 selected
        else if (p1Selected)
        {
            mainFrame.color = p1Color;
            secondaryFrame.color = p1Color;
        }
        else if (p2Selected)
        {
            mainFrame.color = p2Color;
            secondaryFrame.color = p2Color;
        }
        else
        {
            mainFrame.color = unselectedColor;
            secondaryFrame.color = unselectedColor;
        }
    }

    public void Init(TeamSelectionController _selectionController, Pookiemon _pookie, Color _p1Color, Color _p2Color)
    {
        selectionController = _selectionController;
        pookie = _pookie;
        p1Color = _p1Color;
        p2Color = _p2Color;

        pookiemonImage.sprite = pookie.PookiemonData.sprite;
    }

}
    
