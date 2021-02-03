using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public string PlayersTag;
    public List<Damageable> Units = new List<Damageable>();
    public Texture _image;
    public List<Damageable> SelectedUnits = new List<Damageable>();

    private Camera _camera;
    private bool _selecting = false;
    private int _layerMask;
    private int _defaultMask;
    private int _hitBoxMask;
    private int _agentlayer;
    private int _hitBoxlayer;
    private int _screenHeight;
    private Vector2 _startSelectPoint;

    private void Awake()
    {
        Static.GameLogic = this;
        Static.PlayersTag = PlayersTag;
    }

    private void Start()
    {
        Events.OnDamageableDestroy.Subscribe(OnDamageableDestroy);
        _screenHeight = Screen.height;
        _camera = Camera.main;
        _layerMask = LayerMask.GetMask(new string[] { "HitBox", "Default" });
        _defaultMask = LayerMask.GetMask("Default");
        _hitBoxMask = LayerMask.GetMask("HitBox");
        _agentlayer = LayerMask.NameToLayer("Agent");
        _hitBoxlayer = LayerMask.NameToLayer("HitBox");
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RightClick();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            LeftClick();
        }
    }

    public void RegistrUnit(Damageable _unit)
    {
        if (_unit.tag == PlayersTag)
        {
            Units.Add(_unit);
        }
    }

    public void OnDamageableDestroy(Damageable _destroyed)
    {
        if (_destroyed.tag == PlayersTag)
        {
            Units.Remove(_destroyed);
        }
        else
        {
            SelectedUnits.Remove(_destroyed);
            Static.UI.HighlightThis(SelectedUnits);
        }
    }

    private void LeftClick()
    {
        Ray _ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(_ray, out RaycastHit _hitInfo, 100, _layerMask))
        {
            Collider _collider = _hitInfo.collider;
            Rigidbody _attachedRigidbody = _collider.attachedRigidbody;

            if (_collider.gameObject.layer == _hitBoxlayer & _attachedRigidbody != null)
            {
                SelectedUnits = new List<Damageable> { _attachedRigidbody.GetComponent<Damageable>() };
                Static.UI.HighlightThis(SelectedUnits);
                _selecting = false;
                return;
            }
        }
        _selecting = true;
        SelectedUnits.Clear();
        Static.UI.HighlightThis(SelectedUnits);
    }

    private void RightClick()
    {
        if (SelectedUnits.Count != 0)
        {
            Ray _ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out RaycastHit _hitInfo, 100, _layerMask))
            {
                Collider _collider = _hitInfo.collider;
                Rigidbody _attachedBody = _collider.attachedRigidbody;

                if (_collider.gameObject.layer != _hitBoxlayer)
                {
                    for (int i = 0; i < SelectedUnits.Count; i++)
                    {
                        if (SelectedUnits[i].tag == PlayersTag & SelectedUnits[i].gameObject.layer == _agentlayer)
                        {
                            SelectedUnits[i].GetComponent<Character>().MoveToPoint(_hitInfo.point, false);
                        }
                    }
                }
                else if (_attachedBody.gameObject != gameObject)
                {
                    for (int i = 0; i < SelectedUnits.Count; i++)
                    {
                        if (SelectedUnits[i].tag == PlayersTag & SelectedUnits[i].gameObject.layer == _agentlayer)
                        {
                            if (Static.AllianceSystem.GetEnemyTags(PlayersTag).Contains(_attachedBody.tag))
                            {
                                SelectedUnits[i].GetComponent<Character>().Attack(_attachedBody.transform);
                            }
                            else
                            {
                                SelectedUnits[i].GetComponent<Character>().MoveToPoint(_attachedBody.transform.position, true);
                            }
                        }
                    }
                }
            }
        }
    }

    private void OnGUI()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _selecting = true;
            _startSelectPoint = Input.mousePosition;
        }
        if (Vector3.Distance(_startSelectPoint, Input.mousePosition) > 15)
        {
            if (Input.GetMouseButtonUp(0) & _selecting == true)
            {
                _selecting = false;
                SelectUnits();
                SortSelected();
                Static.UI.HighlightThis(SelectedUnits);
            }
            if (Input.GetMouseButton(0))
            {
                Rect _rect = PointsToRect(_startSelectPoint, Input.mousePosition);
                GUI.Box(_rect, "");
                //GUI.DrawTexture(_rect, _image);
            }
        }
    }

    private void SelectUnits()
    {
        Rect _rect = PointsToRect(_startSelectPoint, Input.mousePosition);
        Ray _ray = new Ray();
        RaycastHit _hitInfo = new RaycastHit();
        Vector3[] _screenPoints = { new Vector3(_rect.center.x, _screenHeight - _rect.center.y),
                                    new Vector3(_rect.xMax, _screenHeight - _rect.yMax) };
        for (int i = 0; i < _screenPoints.Length; i++)
        {
            _ray = _camera.ScreenPointToRay(_screenPoints[i]);
            if (Physics.Raycast(_ray, out _hitInfo, 100, _defaultMask))
            {
                _screenPoints[i] = _hitInfo.point;
            }
            else
            {
                return;
            }
        }

        float _radius = Vector3.Distance(_screenPoints[0], _screenPoints[1]);
        Collider[] _colliders = Physics.OverlapSphere(_screenPoints[0], _radius, _hitBoxMask);

        if (_colliders.Length != 0)
        {
            
            for (int i = 0; i < _colliders.Length; i++)
            {
                Vector3[] _points = { _colliders[i].bounds.max, _colliders[i].bounds.min, _colliders[i].transform.position };
                for (int j = 0; j < _points.Length; j++)
                {
                    if (_rect.Contains(_camera.WorldToScreenPoint(_points[j]), true))
                    {
                        SelectedUnits.Add(_colliders[i].attachedRigidbody.GetComponent<Damageable>());
                        break;
                    }
                }
            }
        }
    }

    private void SortSelected()
    {
        if (SelectedUnits.Count > 1)
        {
            List<Damageable> _playerUnit = new List<Damageable>();
            for (int i = 0; i < SelectedUnits.Count; i++)
            {
                if (SelectedUnits[i].tag == PlayersTag)
                {
                    _playerUnit.Add(SelectedUnits[i]);
                }
            }
            if (_playerUnit.Count == 0)
            {
                for (int i = 0; i < SelectedUnits.Count; i++)
                {
                    if (SelectedUnits[i].tag != PlayersTag)
                    {
                        _playerUnit.Add(SelectedUnits[0]);
                        break;
                    }
                }
            }
            SelectedUnits = _playerUnit;
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