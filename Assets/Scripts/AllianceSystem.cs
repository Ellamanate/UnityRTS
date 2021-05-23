using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllianceSystem : Singleton<AllianceSystem>
{
    public IReadOnlyCollection<Alliance> Alliances => _alliances.AsReadOnly();
    [SerializeField] private List<Alliance> _alliances = new List<Alliance>();

    public IReadOnlyCollection<string> GetEnemyTags(string selfTag)
    {
        foreach (Alliance alliance in Alliances)
        {
            if (alliance.Tag == selfTag)
            {
                return alliance.EnemyTags;
            }
        }
        return null;
    }
}

[System.Serializable]
public class Alliance
{
    public IReadOnlyCollection<string> EnemyTags => _enemyTags.AsReadOnly();
    public string Tag;

    [SerializeField] private List<string> _enemyTags = new List<string>();
}
