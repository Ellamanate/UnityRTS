using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public string PlayersTag => _playersTag;

    [SerializeField] private string _playersTag;

    public void CreateCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}