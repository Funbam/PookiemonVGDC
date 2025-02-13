using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Pookiemon List", menuName = "ScriptableObjects/Pookiemon List")]

public class AllPookiemonSO : ScriptableObject
{
    public string itemsPath = "";
    public List<GameObject> pookiemonGOs = new List<GameObject>();
    public List<Pookiemon> pookiemons = new List<Pookiemon>();

    private Dictionary<Pookiemon, GameObject> pookiemonsDict = new Dictionary<Pookiemon, GameObject>();
    public Dictionary<Pookiemon, GameObject> PookiemonsDict { get { return pookiemonsDict; } }
    
    public void Init()
    {
        pookiemonsDict = new Dictionary<Pookiemon, GameObject>();

        for (int i = 0; i < pookiemons.Count; i++)
        {
            pookiemonsDict[pookiemons[i]] = pookiemonGOs[i];
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Get all Pookiemons at itemsPath")]
    private void GetAllItemsAtPath()
    {
        pookiemonGOs = new List<GameObject>();
        pookiemons = new List<Pookiemon>();
        string[] allItemPaths = Directory.GetFiles(itemsPath, "*.prefab", searchOption: SearchOption.TopDirectoryOnly);

        foreach (var path in allItemPaths)
        {
            GameObject item = (GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
            pookiemonGOs.Add(item);

            if (item.TryGetComponent<Pookiemon>(out Pookiemon pookie))
            {
                pookiemons.Add(pookie);
            }
        }
        AssetDatabase.SaveAssets();
    }
#endif
}
