using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damageable : MonoBehaviour
{
    public int MaxHP;
    public int CurrentHP;
    public int Armor;
    public Collider HitBox;
    public Vector3 HealthBarStartPos;
    public UnitUI UnitUI;
    protected Camera Camera;

    protected virtual void Start()
    {
        Camera = Camera.main;
        Static.GameLogic.RegistrUnit(this);
        UnitUI = Static.UI.RegistrUnit(this);
        SetHealthBar();
    }

    protected virtual void Update()
    {

    }

    public virtual void SetHealthBar()
    {
        Static.UI.SetHealthBar(this, HealthBarStartPos);
        CurrentHP = MaxHP;
    }

    public virtual void ApplyDamage(int _applyedDamage)
    {
        CurrentHP -= (_applyedDamage - Armor);
        if (UnitUI.HealthBar != null)
        {
            ChangeHealthBar();
        }

        if (CurrentHP <= 0)
        {
            Events.OnDamageableDestroy.Publish(this);
            Destroy(gameObject);
        }
    }

    private void ChangeHealthBar()
    {
        UnitUI.HealthBar.fillAmount = (float)CurrentHP / (float)MaxHP;
    }
}
