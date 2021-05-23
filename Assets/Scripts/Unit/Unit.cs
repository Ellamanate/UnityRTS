using System.Collections;
using UnityEngine;
using UnitManagement;


public abstract class Unit : GameEntity, IManageable
{
    public Vector3 HealthBarStartPos { get => _healthBarStartPos; }

    [SerializeField] private Vector3 _healthBarStartPos;

    protected Camera mainCamera;

    public override void WhenStart()
    {
        mainCamera = Camera.main;
    }

    public override void WhenEnable()
    {
        SetHealthBar();
    }

    public void SetHealthBar()
    {
        WorldUI.Instance.CreateHealthBar(this, HealthBarStartPos);
    }

    public virtual void Move(Vector3 point) { }
    public virtual void Move(IDamageable damageable) { }
    public virtual void Attack(Vector3 point) { }
    public virtual void Attack(IDamageable damageable) { }
    public virtual void Collect(ItemPrefab item) { }
    public virtual void Stop() { }
    public virtual void HoldPosition() { }
}