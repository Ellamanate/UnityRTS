using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllianceSystem : MonoBehaviour
{
    public List<Alliance> Alliances = new List<Alliance>();

    private void Awake()
    {
        Static.AllianceSystem = this;
    }

    public List<string> GetEnemyTags(string _selfTag)
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
    public string Tag;
    public List<string> EnemyTags = new List<string>();
}
