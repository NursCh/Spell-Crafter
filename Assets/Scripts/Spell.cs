using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Spell
{
    public string aspect1;
    public string aspect2;
    public string name;
    public Spell(string name)
    {
        this.name = name;
    }
}

[System.Serializable]
public class SpellsList
{
    public List<Spell> spells;
}

public class SpellsLoader : MonoBehaviour
{
    public SpellsList spellsList;
    public void LoadSpells()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "spells.json");

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            spellsList = JsonUtility.FromJson<SpellsList>(dataAsJson);
        }
        else
        {
            Debug.LogError("Cannot find combination file!");
        }
    }

    public string GetSpellResult(string aspect1, string aspect2)
    {
        foreach (var combo in spellsList.spells)
        {
            if ((combo.aspect1 == aspect1 && combo.aspect2 == aspect2) ||
                (combo.aspect1 == aspect2 && combo.aspect2 == aspect1))
            {
                if (!string.IsNullOrEmpty(combo.name))
                {
                    return combo.name;
                }
            }
        }
        return null;
    }
}