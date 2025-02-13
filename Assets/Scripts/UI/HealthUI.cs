using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    private const string levelLabel = "Lv.";

    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private Slider healthbar;
    [SerializeField] private TMP_Text healthText;

    public void Init(Pookiemon pookie)
    {
        nameText.text = pookie.PookiemonData.pookiemonName;
        levelText.text = $"{levelLabel}{Pookiemon.LEVEL}";

        healthbar.maxValue = pookie.GetStat(Stats.HP);
        healthbar.minValue = 0;
        healthbar.value = Mathf.Clamp(pookie.CurrentHealth, healthbar.minValue, healthbar.maxValue);

        healthText.text = $"{healthbar.value}/{healthbar.maxValue}";
    }

    public void SetHealth(int currentHealth)
    {
        healthbar.value = Mathf.Clamp(currentHealth, healthbar.minValue, healthbar.maxValue);
        healthText.text = $"{Mathf.Clamp(currentHealth, healthbar.minValue, healthbar.maxValue)}/{healthbar.maxValue}";
    }
}
