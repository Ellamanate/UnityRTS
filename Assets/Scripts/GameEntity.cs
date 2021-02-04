using System.Collections;
using UnityEngine;


public abstract class GameEntity : MonoBehaviour, IDamageable, ISelectable
{
    public virtual int MaxHP { get => _maxHP; set { _maxHP = OnChangeMaxHP(value); Events.OnDamageableHPChange.Publish(this); WhenChangeHP(); } }
    public virtual int CurrentHP { get => _currentHP; set { _currentHP = OnChangeCurrentHP(value); Events.OnDamageableHPChange.Publish(this); WhenChangeHP(); } }
    public Armor ArmorType { get => _armorType; set => _armorType = value; }
    public Collider HitBox { get => _hitBox; }
    public GameObject GameObject { get => _gameObject; }
    public WorldUIContainer WorldUIContainer { get => _worldUIContainer; }
    public virtual Sprite Icon { get => _icon; set => _icon = value; }

    [SerializeField] private Armor _armorType;
    [SerializeField] private Collider _hitBox;
    [SerializeField] private Sprite _icon;
    [SerializeField] private int _maxHP;
    [SerializeField] private int _currentHP;
    [SerializeField] private WorldUIContainer _worldUIContainer;
    private GameObject _gameObject;

    public void ApplyDamage(DamageType _damageType, int _applyedDamage)
    {
        CurrentHP -= _damageType.DecreaseDamage(ArmorType, _applyedDamage);

        if (CurrentHP <= 0)
            Destroy();
    }

    public virtual void Destroy() { Destroy(gameObject); }

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
        WorldUI.Instance.RegistrUnit(WorldUIContainer);
        WhenEnable();
    }

    private void OnDisable()
    {
        Events.OnUnitDestroy.Publish(this);
        _gameObject = null;
        WhenDisable();
    }

    private int OnChangeCurrentHP(int _value)
    {
        if (_value > MaxHP)
            return MaxHP;
        else
            return _value;
    }

    private int OnChangeMaxHP(int _value)
    {
        if (_currentHP > _value)
            _currentHP = _value;

        return _value;
    }
}