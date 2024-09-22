using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Combination
{
    public string aspect1;
    public string aspect2;
    public string result;
}

[System.Serializable]
public class CombinationList
{
    public List<Combination> combinations;
}

public class CombinationLoader : MonoBehaviour
{
    public CombinationList combinationList;
    public void LoadCombinations()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "combinations.json");

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            combinationList = JsonUtility.FromJson<CombinationList>(dataAsJson);
        }
        else
        {
            Debug.LogError("Cannot find combination file!");
        }
    }

    public string GetCombinationResult(string aspect1, string aspect2)
    {
        foreach (var combo in combinationList.combinations)
        {
            if ((combo.aspect1 == aspect1 && combo.aspect2 == aspect2) ||
                (combo.aspect1 == aspect2 && combo.aspect2 == aspect1))
            {
                if (!string.IsNullOrEmpty(combo.result))
                {
                    return combo.result;
                }
            }
        }
        return null;
    }
}