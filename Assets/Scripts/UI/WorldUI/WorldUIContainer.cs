using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class WorldUIContainer
{
    public Transform ContainerTransform;
    public Transform Highlighter;
    public Image HealthBar;
    public Transform TargetTransform { get => _targetTransform; }

    private Transform _targetTransform;

    public WorldUIContainer(Transform _initTargetTransform)
    {
        _targetTransform = _initTargetTransform;
    }
}