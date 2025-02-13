using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PookiemonSelectionButton : MonoBehaviour
{
    private const string levelLabel = "Lv.";

    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private Slider healthbar;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Image image;
    [SerializeField] private GameObject statusEffect;
    [SerializeField] private TMP_Text statusText;

    [SerializeField] private Button myButton;
    private Pookiemon pookiemon;

    private void Awake()
    {
        myButton.onClick.AddListener(OnClick);
    }

    public void Init(Pookiemon pookie)
    {
        pookiemon = pookie;
        nameText.text = pookie.PookiemonData.pookiemonName;
        levelText.text = $"{levelLabel}{Pookiemon.LEVEL}";

        healthbar.maxValue = pookie.GetStat(Stats.HP);
        healthbar.minValue = 0;
        healthbar.value = pookie.CurrentHealth;

        healthText.text = $"{healthbar.value}/{healthbar.maxValue}";

        image.sprite = pookie.PookiemonData.sprite;
        statusEffect.SetActive(false);
        myButton.interactable = true;

        if (pookiemon.IsDead)
        {
            myButton.interactable = false;
            statusEffect.SetActive(true);
            statusText.text = "FAINT";
        }
    }

    private void OnClick()
    {
        BattleSystem.instance.OnPookiemonSwitched(pookiemon);
    }
}
