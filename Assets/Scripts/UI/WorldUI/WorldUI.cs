using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class WorldUI : Singleton<WorldUI>
{
    public Canvas Canvas;
    public Image HealthBarPrefab;
    public GameObject HighliterPrefab;
    public UnitsColor UnitsColor;
    public Transform HilightersPool;
    
    public Transform[] _highliters;
    private List<WorldUIContainer> UnitsUI = new List<WorldUIContainer>();
    private ObjectPool _objectPool;

    private void Awake()
    {
        _objectPool = new ObjectPool(HighliterPrefab, HilightersPool);
    }

    private void OnEnable()
    {
        Events.OnUnitDestroy.Subscribe(OnUnitDestroy);
        Events.OnSelectedChange.Subscribe(HighlightThis);
        Events.OnDamageableHPChange.Subscribe(UpdateHealthBar);
    }

    private void OnDisable()
    {
        Events.OnUnitDestroy.UnSubscribe(OnUnitDestroy);
        Events.OnSelectedChange.UnSubscribe(HighlightThis);
        Events.OnDamageableHPChange.UnSubscribe(UpdateHealthBar);
    }

    private void LateUpdate()
    {
        for (int i = 0; i < UnitsUI.Count; i++)
            UnitsUI[i].ContainerTransform.position = UnitsUI[i].TargetTransform.position;
    }

    public void RegistrUnit(WorldUIContainer _unitUI)
    {
        GameObject _container = new GameObject();
        _unitUI.ContainerTransform = _container.transform;
        _container.transform.SetParent(Canvas.transform);
        UnitsUI.Add(_unitUI);
    }

    public void OnUnitDestroy(IDamageable _destroyed)
    {
        HideDamageableUI(_destroyed);
    }

    public void HideDamageableUI(IDamageable _destroyed)
    {
        _objectPool.BackToPool(_destroyed.WorldUIContainer.Highlighter);
        _destroyed.WorldUIContainer.Highlighter = null;
        UnitsUI.Remove(_destroyed.WorldUIContainer);
        Destroy(_destroyed.WorldUIContainer.ContainerTransform.gameObject);
    }

    public void SetHealthBar(ISelectable _target, Vector3 _offset)
    {
        Image _healthBarImage = Instantiate(HealthBarPrefab);
        _healthBarImage.transform.SetParent(_target.WorldUIContainer.ContainerTransform);
        _healthBarImage.transform.localPosition = _offset;
        _target.WorldUIContainer.HealthBar = _healthBarImage;
    }

    public void UpdateHealthBar(IDamageable _damageable)
    {
        if (_damageable.WorldUIContainer.HealthBar != null)
            _damageable.WorldUIContainer.HealthBar.fillAmount = (float)_damageable.CurrentHP / (float)_damageable.MaxHP;
    }

    public void HighlightThis(IReadOnlyList<ISelectable> _selected)
    {
        if (_selected.Count != 0)
        {
            _highliters = _objectPool.GetObjects(_selected.Count);

            for (int i = 0; i < _selected.Count; i++)
            {
                Transform _selectedTransform = _selected[i].HitBox.transform;
                Transform _highliter = _highliters[i];
                _highliter.SetParent(_selected[i].WorldUIContainer.ContainerTransform);
                _highliter.localPosition = new Vector3(0, 0.2f, 0);
                _highliter.localScale = _selectedTransform.lossyScale;
                _highliter.gameObject.GetComponent<RawImage>().color = GetColor(_selectedTransform.tag);
                _selected[i].WorldUIContainer.Highlighter = _highliter;
            }
        }
        else
        {
            _objectPool.BackToPoolAll();

            for (int i = 0; i < UnitsUI.Count; i++)
                UnitsUI[i].Highlighter = null;
        }
    }

    private Color GetColor(string _tag)
    {
        Color _color;

        if (_tag == GameManager.Instance.PlayersTag)
            _color = UnitsColor.PlayerColor;
        else if (AllianceSystem.Instance.GetEnemyTags(GameManager.Instance.PlayersTag).Contains(_tag))
            _color = UnitsColor.EnemyColor;
        else
            _color = UnitsColor.NeutralColor;

        return _color;
    }
}

[System.Serializable]
public struct UnitsColor
{
    public Color PlayerColor;
    public Color EnemyColor;
    public Color NeutralColor;
}


