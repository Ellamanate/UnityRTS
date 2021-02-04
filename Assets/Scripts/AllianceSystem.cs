using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllianceSystem : Singleton<AllianceSystem>
{
    public IReadOnlyCollection<Alliance> Alliances { get => _alliances.AsReadOnly(); }
    [SerializeField] private List<Alliance> _alliances = new List<Alliance>();

    public IReadOnlyCollection<string> GetEnemyTags(string _selfTag)
    {
        foreach (Alliance _alliance in Alliances)
        {
            if (_alliance.Tag == _selfTag)
            {
                return _alliance.EnemyTags;
            }
        }
        return null;
    }
}

[System.Serializable]
public class Alliance
{
    public IReadOnlyCollection<string> EnemyTags { get => _enemyTags.AsReadOnly(); }
    public string Tag;

    [SerializeField] private List<string> _enemyTags = new List<string>();
}
