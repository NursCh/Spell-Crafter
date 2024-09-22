

using System;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    public GameObject prefab1; 
    public GameObject prefab2; 
    public GameObject prefab3; 
    public GameObject prefab4; 
    public List<GameObject> BaseAspects;
    [System.NonSerialized]
    public List<GameObject> GeneratedAspects;
    [System.NonSerialized]
    public List<GameObject> SelectedAspects;
    [System.NonSerialized]
    public List<Spell> Spells;
    public uint NumberOfGeneratedAspects = 9;

    public GameObject CreateObjectAspect(GameObject prefab)
    {
        GameObject newAspect = Instantiate(prefab);
        return newAspect;
    }
    public void GenerateBaseAspects()
    {
        BaseAspects = new List<GameObject>();
        
        BaseAspects.Add(CreateObjectAspect(prefab1));
        BaseAspects.Add(CreateObjectAspect(prefab2));
        BaseAspects.Add(CreateObjectAspect(prefab3));
        BaseAspects.Add(CreateObjectAspect(prefab4));
        Spells = new List<Spell>();
        Spells.Add(new Spell("kill"));
        Spells.Add(new Spell("heal"));
        Spells.Add(new Spell("create"));
        Spells.Add(new Spell("remove"));
    }
    
    public List<GameObject> GenerateAspects(List<GameObject> aspects, Customer customer)
    {
        List<GameObject> newAspects = new List<GameObject>();
        HashSet<GameObject> set = new HashSet<GameObject>();
        int difficulty = customer.Difficulty;
        while (newAspects.Count < NumberOfGeneratedAspects)
        {
            int numberOfAspects = aspects.Count;
            int aspectIndex = 0;
            aspectIndex = UnityEngine.Random.Range(0, numberOfAspects);
            if (newAspects.Count > 4 && set.Count < 2)
            {
                while (set.Contains(aspects[aspectIndex]))
                {
                    aspectIndex = UnityEngine.Random.Range(0, numberOfAspects);
                }
                newAspects.Add(aspects[aspectIndex]);
                int random = UnityEngine.Random.Range(0, numberOfAspects);
                while (aspectIndex != random)
                {
                    random = UnityEngine.Random.Range(0, numberOfAspects);
                }
                newAspects.Add(aspects[random]);
                set.Add(aspects[aspectIndex]);
            }
            else
            {
                switch (difficulty)
                {
                    case 0:
                        newAspects.Add(aspects[aspectIndex]);
                        set.Add(aspects[aspectIndex]);
                        break;
                    case 1:
                        if (set.Count > 2)
                        {
                            while (!set.Contains(aspects[aspectIndex]))
                            {
                                aspectIndex = UnityEngine.Random.Range(0, numberOfAspects);
                            }
                            newAspects.Add(aspects[aspectIndex]);
                            set.Add(aspects[aspectIndex]);
                        }
                        else
                        {
                            newAspects.Add(aspects[aspectIndex]);
                            set.Add(aspects[aspectIndex]);
                        }
                        break;
                    case 2:
                        if (set.Count > 2)
                        {
                            while (!set.Contains(aspects[aspectIndex]))
                            {
                                aspectIndex = UnityEngine.Random.Range(0, numberOfAspects);
                            }
                            newAspects.Add(aspects[aspectIndex]);
                            set.Add(aspects[aspectIndex]);
                        }
                        else
                        {
                            newAspects.Add(aspects[aspectIndex]);
                            set.Add(aspects[aspectIndex]);
                        }
                        break;
                }
            }
        }
        return newAspects;
    }
}

