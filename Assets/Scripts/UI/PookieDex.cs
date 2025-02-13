using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class PookieDex : MonoBehaviour
{
    public static PookieDex instance;
    
    [SerializeField] private SpriteLibraryAsset typeIconLibrary;
    [SerializeField] private Image pookiemonImage;
    [SerializeField] private TMP_Text pookiemonName;
    [SerializeField] private TMP_Text pookiemonTitle;
    [SerializeField] private TMP_Text pookiemonHeight;
    [SerializeField] private TMP_Text pookiemonWeight;
    [SerializeField] private TMP_Text pookiemonHabitat;

    [SerializeField] private Image primaryType;
    [SerializeField] private Image secondaryType;

    [SerializeField] private TMP_Text description;
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        ClearDex();
    }

    private void ClearDex()
    {
        pookiemonImage.enabled = false;
        primaryType.enabled = false;
        secondaryType.enabled = false;

        pookiemonName.text = "";
        pookiemonTitle.text = "";
        pookiemonHeight.text = "";
        pookiemonWeight.text = "";
        pookiemonHabitat.text = "";
        description.text = "";
    }

    public void UpdateDexEntry(Pookiemon pookie)
    {
        audioSource.PlayOneShot(pookie.PookiemonData.pookiemonCrySFX);

        pookiemonImage.enabled = true;
        primaryType.enabled = true;
        secondaryType.enabled = false;

        pookiemonImage.sprite = pookie.PookiemonData.sprite;
        primaryType.sprite = typeIconLibrary.GetSprite("Types", pookie.PookiemonData.type1.ToString());
        if (pookie.PookiemonData.type2 != Types.NULL)
        {
            secondaryType.enabled = true;
            secondaryType.sprite = typeIconLibrary.GetSprite("Types", pookie.PookiemonData.type2.ToString());
        }

        pookiemonName.text = $"{pookie.PookiemonData.pookiemonName}";
        pookiemonTitle.text = $"{pookie.PookiemonData.pookiemonTitle} Pookiemon";
        pookiemonHeight.text = $"{pookie.PookiemonData.height:#.0}m";
        pookiemonWeight.text = $"{pookie.PookiemonData.weight:#.0}kg";
        pookiemonHabitat.text = $"{pookie.PookiemonData.GetHabitatString()}";
        description.text = $"{pookie.PookiemonData.description}";
    }
}
