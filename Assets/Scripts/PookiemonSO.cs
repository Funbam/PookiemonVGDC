using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pookiemon Data", menuName = "ScriptableObjects/Pookiemon Data")]
public class PookiemonSO : ScriptableObject
{
    [Header("Dex Info")]
    public Sprite sprite;
    public string pookiemonName;
    public string pookiemonTitle;
    public Types type1;
    public Types type2;
    public float height = 1.0f;
    public float weight = 1.0f;
    public Habitats habitat;
    [TextArea(5, 10)]
    public string description;

    [Header("Battle Info")]
    public StatMap baseStats;
    public AudioClip pookiemonCrySFX;

    public string GetHabitatString()
    {
        string retString = "";
        switch(habitat)
        {
            case Habitats.Grassland:
                retString = habitat.ToString();
                break;
            case Habitats.Forest:
                retString = habitat.ToString();
                break;
            case Habitats.WatersEdge:
                retString = "Water's Edge";
                break;
            case Habitats.Sea:
                retString = habitat.ToString();
                break;
            case Habitats.Cave:
                retString = habitat.ToString();
                break;
            case Habitats.Mountain:
                retString = habitat.ToString();
                break;
            case Habitats.RoughTerrain:
                retString = "Rough-Terrain";
                break;
            case Habitats.Urban:
                retString = habitat.ToString();
                break;
            case Habitats.Rare:
                retString = habitat.ToString();
                break;
        }
        return retString;
    }
}
