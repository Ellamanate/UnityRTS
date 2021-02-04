using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnitSelector : Singleton<UnitSelector>
{
    public IReadOnlyList<ISelectable> SelectedUnits { get => _selectedUnits.AsReadOnly(); }
    public bool CanControlSelected { get => _canControlSelected; }

    private List<ISelectable> _selectedUnits = new List<ISelectable>();
    private List<Unit> _units = new List<Unit>();
    private bool _selecting = false;
    private bool _canControlSelected = false;
    private int _screenHeight;
    private Camera _camera;
    private Vector2 _startSelectPoint;

    public void RegistrUnit(Unit _unit)
    {
        if (_unit.CompareTag(GameManager.Instance.PlayersTag))
            _units.Add(_unit);
    }

    public void OnUnitDestroy(IDamageable _destroyed)
    {
        if (_destroyed.GameObject.CompareTag(GameManager.Instance.PlayersTag))
            _units.Remove(_destroyed as Unit);

        if (_selectedUnits.Contains(_destroyed as Unit))
            _selectedUnits.Remove(_destroyed as Unit);
    }

    public void Select()
    {
        if (ScreenUI.Instance.IsMouseEscapeUI())
        {
            if (Raycaster.Instance.SelectMouseRaycast(out Rigidbody _attachedRigidbody))
            {
                if (TypeChecker<ISelectable>.CheckGameObject(_attachedRigidbody.gameObject, out ISelectable _selectable))
                {
                    _selectedUnits.Clear();
                    _selectedUnits.Add(_selectable);
                    Events.OnSelectedChange.Publish(SelectedUnits);
                    _selecting = false;

                    if (_selectable.GameObject.CompareTag(GameManager.Instance.PlayersTag))
                        _canControlSelected = true;
                    else
                        _canControlSelected = false;

                    return;
                }
            }

            _selecting = true;
            _selectedUnits.Clear();
            Events.OnSelectedChange.Publish(SelectedUnits);
        }
        else _selecting = false;
    }

    private void Start()
    {
        _screenHeight = Screen.height;
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        Events.OnUnitDestroy.Subscribe(OnUnitDestroy);
    }

    private void OnDisable()
    {
        Events.OnUnitDestroy.UnSubscribe(OnUnitDestroy);
    }

    private void OnGUI()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _selecting = true;
            _startSelectPoint = Input.mousePosition;
        }
        if (Vector3.Distance(_startSelectPoint, Input.mousePosition) > 15 & _selecting == true)
        {
            if (Input.GetMouseButtonUp(0))
            {
                _selecting = false;
                SelectGroup();
                SortSelected();
                Events.OnSelectedChange.Publish(SelectedUnits);
            }
            if (Input.GetMouseButton(0))
            {
                Rect _rect = PointsToRect(_startSelectPoint, Input.mousePosition);
                GUI.Box(_rect, "");
                //GUI.DrawTexture(_rect, _image);
            }
        }
    }

    private void SelectGroup()
    {
        Rect _rect = PointsToRect(_startSelectPoint, Input.mousePosition);
        Vector3[] _screenPoints = { new Vector3(_rect.center.x, _screenHeight - _rect.center.y),
                                    new Vector3(_rect.xMax, _screenHeight - _rect.yMax) };

        for (int i = 0; i < _screenPoints.Length; i++)
        {
            if (Raycaster.Instance.PointRaycast(_screenPoints[i], out RaycastHit _hitInfo))
                _screenPoints[i] = _hitInfo.point;
            else return;
        }

        float _radius = Vector3.Distance(_screenPoints[0], _screenPoints[1]);
        Collider[] _colliders = Physics.OverlapSphere(_screenPoints[0], _radius, Raycaster.Instance.SelectMask);

        if (_colliders.Length != 0)
        {
            for (int i = 0; i < _colliders.Length; i++)
            {
                if (_rect.Overlaps(PointsToRect(_camera.WorldToScreenPoint(_colliders[i].bounds.min),
                                                _camera.WorldToScreenPoint(_colliders[i].bounds.max)), true))
                    _selectedUnits.Add(_colliders[i].attachedRigidbody.GetComponent<ISelectable>());
            }
        }
    }

    private void SortSelected()
    {
        if (_selectedUnits.Count > 1)
        {
            List<ISelectable> _playerUnit = new List<ISelectable>();

            for (int i = 0; i < _selectedUnits.Count; i++)
            {
                if (TypeChecker<Unit>.CheckSelectable(_selectedUnits[i], out Unit _damageable))
                    if (_damageable.CompareTag(GameManager.Instance.PlayersTag))
                        _playerUnit.Add(_damageable);
            }

            if (_playerUnit.Count == 0)
            {
                for (int i = 0; i < _selectedUnits.Count; i++)
                {
                    if (TypeChecker<Unit>.CheckSelectable(_selectedUnits[i], out Unit _damageable))
                    {
                        if (!_damageable.CompareTag(GameManager.Instance.PlayersTag))
                        {
                            _selectedUnits.Clear();
                            _selectedUnits.Add(_damageable);
                            _canControlSelected = false;
                            return;
                        }
                    }
                }
            }

            _canControlSelected = true;
            _selectedUnits.Clear();
            _selectedUnits.AddRange(_playerUnit);
        }
    }

    private Rect PointsToRect(Vector3 _start, Vector3 _end)
    {
        float _width = _end.x - _start.x;
        float _height = _end.y - _start.y;
        Rect _rect = new Rect(_start.x, _screenHeight - _start.y, _width, -_height);

        return _rect;
    }
}