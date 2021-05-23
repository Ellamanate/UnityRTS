using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnitManagement;

public class WorldUI : Singleton<WorldUI>
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Image _healthBarPrefab;
    [SerializeField] private Highlighter _highliterPrefab;
    [SerializeField] private UnitsColor _unitsColor;
    [SerializeField] private Transform _hilightersPool;
    private Highlighter[] _highliters;
    private ObjectPool<Highlighter> _objectPool;
    private Dictionary<GameObject, WorldUIContainer> _containers = new Dictionary<GameObject, WorldUIContainer>();

    private void Awake()
    {
        _objectPool = new ObjectPool<Highlighter>(_highliterPrefab, _hilightersPool);
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
        foreach (WorldUIContainer container in _containers.Values)
            container.SinchronizePosition();
    }

    public void RegistrEntity(IDamageable damageable)
    {
        if (!_containers.ContainsKey(damageable.GameObject)) 
        {
            GameObject containerObject = new GameObject();
            WorldUIContainer container = new WorldUIContainer(damageable.GameObject.transform, containerObject.transform);
            containerObject.transform.SetParent(_canvas.transform);

            _containers.Add(damageable.GameObject, container);
        }
    }

    public void OnUnitDestroy(IDamageable destroyed)
    {
        HideDamageableUI(destroyed);
    }

    public void HideDamageableUI(IDamageable destroyed)
    {
        if (_containers.TryGetValue(destroyed.GameObject, out WorldUIContainer container))
        {
            _objectPool.BackToPool(container.Highlighter);
            container.Highlighter = null;
            _containers.Remove(destroyed.GameObject);

            Destroy(container.ContainerTransform.gameObject);
        }
    }

    public void CreateHealthBar(IDamageable damageable, Vector3 offset)
    {
        if (_containers.TryGetValue(damageable.GameObject, out WorldUIContainer container))
        {
            Image healthBarImage = Instantiate(_healthBarPrefab);
            healthBarImage.transform.SetParent(container.ContainerTransform);
            healthBarImage.transform.localPosition = offset;
            container.HealthBar = healthBarImage;
        }
    }

    public void UpdateHealthBar(IDamageable damageable)
    {
        if (_containers.TryGetValue(damageable.GameObject, out WorldUIContainer container) && container.HealthBar != null)
            container.HealthBar.fillAmount = (float)damageable.CurrentHP / (float)damageable.MaxHP;
    }

    public void HighlightThis(IReadOnlyList<ISelectable> selected)
    {
        if (selected.Count != 0)
        {
            _highliters = _objectPool.GetObjects(selected.Count);

            for (int i = 0; i < selected.Count; i++)
            {
                if (_containers.TryGetValue(selected[i].GameObject, out WorldUIContainer container))
                {
                    Transform selectedTransform = selected[i].HitBox.transform;
                    _highliters[i].Init(container, selectedTransform);
                    _highliters[i].SetColor(GetColor(selectedTransform.tag));
                    container.Highlighter = _highliters[i];
                }
            }
        }
        else
        {
            _objectPool.BackToPoolAll();

            foreach (WorldUIContainer container in _containers.Values)
                container.DropHighlighter();
        }
    }

    private Color GetColor(string tag)
    {
        Color color;

        if (tag == GameManager.Instance.PlayersTag)
            color = _unitsColor.PlayerColor;
        else if (AllianceSystem.Instance.GetEnemyTags(GameManager.Instance.PlayersTag).Contains(tag))
            color = _unitsColor.EnemyColor;
        else
            color = _unitsColor.NeutralColor;

        return color;
    }
}

[System.Serializable]
public struct UnitsColor
{
    public Color PlayerColor;
    public Color EnemyColor;
    public Color NeutralColor;
}
