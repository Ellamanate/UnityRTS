using System.Collections;
using UnityEngine;
using UnitManagement;


public abstract class GameEntity : MonoBehaviour, IDamageable, ISelectable
{
    [SerializeField] private ArmorType _armorType;
    [SerializeField] private Collider _hitBox;
    [SerializeField] private Sprite _icon;
    [SerializeField] private int _maxHP;
    [SerializeField] private int _currentHP;
    private GameObject _gameObject;

    public virtual int MaxHP { get => _maxHP; set { _maxHP = OnChangeMaxHP(value); Events.OnDamageableHPChange.Publish(this); WhenChangeHP(); } }
    public virtual int CurrentHP { get => _currentHP; set { _currentHP = OnChangeCurrentHP(value); Events.OnDamageableHPChange.Publish(this); WhenChangeHP(); } }
    public ArmorType ArmorType { get => _armorType; set => _armorType = value; }
    public Collider HitBox => _hitBox;
    public GameObject GameObject => _gameObject;
    public virtual Sprite Icon { get => _icon; set => _icon = value; }

    public void ApplyDamage(DamageType _damageType, int _applyedDamage)
    {
        CurrentHP -= _damageType.DecreaseDamage(ArmorType, _applyedDamage);

        if (CurrentHP <= 0)
            Destroy();
    }

    public virtual void Destroy() => Destroy(gameObject);

    public virtual void WhenStart() { }

    public virtual void WhenEnable() { }

    public virtual void WhenDisable() { }

    public virtual void WhenChangeHP() { }

    private void Start()
    {
        CurrentHP = MaxHP;
        WhenStart();
    }

    private void OnEnable()
    {
        _gameObject = gameObject;
        WorldUI.Instance.RegistrEntity(this);
        WhenEnable();
    }

    private void OnDisable()
    {
        Events.OnUnitDestroy.Publish(this);
        _gameObject = null;
        WhenDisable();
    }

    private int OnChangeCurrentHP(int value) => value > MaxHP ? MaxHP : value;

    private int OnChangeMaxHP(int value)
    {
        if (_currentHP > value)
            _currentHP = value;

        return value;
    }
}