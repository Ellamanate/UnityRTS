using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Canvas Canvas;
    public Image HealthBarPrefab;
    public GameObject HighliterPrefab;
    public UnitsColor UnitsColor;
    public Transform HilightersPool;
    
    public Transform[] _highliters;
    private List<UnitUI> UnitsUI = new List<UnitUI>();
    private Camera _camera;
    private Vector3 _offset;
    private ObjectPool _objectPool = new ObjectPool();

    private void Awake()
    {
        Static.UI = this;
        Events.OnDamageableDestroy.Subscribe(OnDamageableDestroy);
        _camera = Camera.main;
        _offset = new Vector3(0, 0.2f, 0);
        _objectPool.Prefab = HighliterPrefab;
        _objectPool.Pool = HilightersPool;
    }

    private void LateUpdate()
    {
        for (int i = 0; i < UnitsUI.Count; i++)
        {
            UnitsUI[i].UIContainer.position = UnitsUI[i].TargetTransform.position;
        }
    }

    public UnitUI RegistrUnit(Damageable _unit)
    {
        GameObject _container = new GameObject();
        UnitUI _unitUI = new UnitUI();
        _unitUI.Init(_unit, _container.transform);
        _container.transform.SetParent(Canvas.transform);
        UnitsUI.Add(_unitUI);
        return _unitUI;
    }

    public void OnDamageableDestroy(Damageable _destroyed)
    {
        _objectPool.BackToPool(_destroyed.UnitUI.Highlighter);
        _destroyed.UnitUI.Highlighter = null;
        Destroy(_destroyed.UnitUI.UIContainer.gameObject);
        UnitsUI.Remove(_destroyed.UnitUI);
    }

    public void SetHealthBar(Damageable _target, Vector3 _offset)
    {
        Image _healthBarImage = Instantiate(HealthBarPrefab);
        _healthBarImage.transform.SetParent(_target.UnitUI.UIContainer);
        _healthBarImage.transform.localPosition = _offset;
        _target.UnitUI.HealthBar = _healthBarImage;
    }

    public void HighlightThis(List<Damageable> _selected)
    {
        if (_selected.Count != 0)
        {
            _highliters = _objectPool.GetObjects(_selected.Count);
            for (int i = 0; i < _selected.Count; i++)
            {
                Transform _selectedTransform = _selected[i].HitBox.transform;
                _highliters[i].SetParent(_selected[i].UnitUI.UIContainer);
                _highliters[i].localPosition = new Vector3(0, 0, 0);
                _highliters[i].localScale = _selectedTransform.lossyScale;
                _highliters[i].gameObject.GetComponent<RawImage>().color = GetColor(_selectedTransform.tag);
                _selected[i].UnitUI.Highlighter = _highliters[i];
            }
        }
        else
        {
            for (int i = 0; i < UnitsUI.Count; i++)
            {
                UnitsUI[i].Highlighter = null;
            }
            _objectPool.BackToPoolAll();
        }
    }

    private Color GetColor(string _tag)
    {
        Color _color;
        if (_tag == Static.PlayersTag)
        {
            _color = UnitsColor.PlayerColor;
        }
        else if (Static.AllianceSystem.GetEnemyTags(Static.PlayersTag).Contains(_tag))
        {
            _color = UnitsColor.EnemyColor;
        }
        else
        {
            _color = UnitsColor.NeutralColor;
        }
        return _color;
    }
}

public class UnitUI
{
    public Damageable TargetDamageable;
    public Transform TargetTransform;
    public Transform UIContainer;
    public Transform Highlighter;
    public Image HealthBar;

    public void Init(Damageable _unit, Transform _container)
    {
        UIContainer = _container.transform;
        TargetDamageable = _unit;
        TargetTransform = _unit.transform;
    }
}

public class ObjectPool
{
    public Transform Pool;
    public GameObject Prefab;
    public List<Transform> Objects = new List<Transform>();

    public Transform[] GetObjects(int _count)
    {
        if (Objects.Count < _count)
        {
            CreateObjects(_count - Objects.Count);
        }

        Transform[] _objects = new Transform[_count];
        for (int i = 0; i < Objects.Count; i++)
        {
            if (i < _objects.Length)
            {
                _objects[i] = Objects[i];
            }
            else
            {
                Objects[i].SetParent(Pool);
            }
        }
        return _objects;
    }

    public void BackToPool(Transform _target)
    {
        if (Objects.Contains(_target))
        { 
            _target.SetParent(Pool);
        }
    }

    public void BackToPoolAll()
    {
        for (int i = 0; i < Objects.Count; i++)
        {
            Objects[i].SetParent(Pool);
        }
    }

    private void CreateObjects(int _count)
    {
        for (int i = 0; i < _count; i++)
        {
            GameObject _newObject = GameObject.Instantiate(Prefab);
            Objects.Add(_newObject.transform);
        }
    }
}

[System.Serializable]
public class UnitsColor
{
    public Color PlayerColor;
    public Color EnemyColor;
    public Color NeutralColor;
}


