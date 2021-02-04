using System.Collections;
using UnityEngine;


public abstract class Unit : GameEntity
{
    public Vector3 HealthBarStartPos { get => _healthBarStartPos; }

    [SerializeField] private Vector3 _healthBarStartPos;

    protected Camera Camera;

    public override void WhenStart()
    {
        Camera = Camera.main;
    }

    public override void WhenEnable()
    {
        UnitSelector.Instance.RegistrUnit(this);
        SetHealthBar();
    }

    public void SetHealthBar()
    {
        WorldUI.Instance.SetHealthBar(this, HealthBarStartPos);
    }
}