using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class WorldUIContainer
{
    public Transform TargetTransform;
    [HideInInspector] public Transform ContainerTransform;
    [HideInInspector] public Transform Highlighter;
    [HideInInspector] public Image HealthBar;
}