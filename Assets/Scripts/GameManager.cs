using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public string PlayersTag { get => _playersTag; }

    [SerializeField] private string _playersTag;

    public void CreateCoroutine(IEnumerator _coroutine)
    {
        StartCoroutine(_coroutine);
    }
}