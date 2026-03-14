using UnityEngine;
using System;
using System.Collections.Generic;
using GameLogic.GameState;

[Serializable]
public class ComplexAnimationPrefabEntry
{
    public string actionStateTypeName;   // stores the type
    public GameObject complexAnimationPrefab;
}

public class ComplexAnimationPrefabRegistry : MonoBehaviour
{
    [SerializeField]
    private List<ComplexAnimationPrefabEntry> entries = new();

    private Dictionary<Type, GameObject> lookup;

    void Awake()
    {
        lookup = new Dictionary<Type, GameObject>();

        foreach (var e in entries)
        {
            if (!ActionStateRegistry.Types.TryGetValue(e.actionStateTypeName, out Type targetType))
            {
                continue;
            }
            
            lookup[targetType] = e.complexAnimationPrefab;
        }
    }

    public GameObject GetComplexAnimationPrefab(Type type)
    {

        if (lookup.ContainsKey(type))
        {
            return lookup[type];
        }

        return null;
    }
}
