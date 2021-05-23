using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class WorldUIContainer
{
    public Highlighter Highlighter;
    public Image HealthBar;
    private Transform _containerTransform;
    private Transform _targetTransform;

    public Transform TargetTransform => _targetTransform;
    public Transform ContainerTransform => _containerTransform;

    public void SinchronizePosition() => _containerTransform.position = _targetTransform.position;

    public void DropHighlighter() => Highlighter = null;

    public WorldUIContainer(Transform targetTransform, Transform containerTransform)
    {
        _containerTransform = containerTransform;
        _targetTransform = targetTransform;
        Highlighter = null;
        HealthBar = null;
    }
}
