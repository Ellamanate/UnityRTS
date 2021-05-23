using System.Collections.Generic;
using UnityEngine;


namespace UnitManagement
{
    public class UnitSelector : Singleton<UnitSelector>
    {
        private List<ISelectable> _selectedUnits = new List<ISelectable>();
        private bool _groupSelecting = false;
        private bool _canControlSelected = false;
        private int _screenHeight;
        private Camera _camera;
        private Vector2 _startSelectPoint;

        public IReadOnlyList<ISelectable> SelectedUnits => _selectedUnits.AsReadOnly();
        public bool CanControlSelected => _canControlSelected;

        public void OnUnitDestroy(IDamageable destroyed)
        {
            if (_selectedUnits.Contains(destroyed as Unit))
                _selectedUnits.Remove(destroyed as Unit);
        }

        public void Select()
        {
            if (ScreenUI.Instance.IsMouseEscapeUI())
            {
                _groupSelecting = true;
                _startSelectPoint = Input.mousePosition;

                _selectedUnits.Clear();

                if (Raycaster.Instance.SelectMouseRaycast(out Rigidbody attachedRigidbody))
                {
                    if (TypeChecker<ISelectable>.CheckGameObject(attachedRigidbody.gameObject, out ISelectable selectable))
                    {
                        _selectedUnits.Add(selectable);

                        _canControlSelected = IsPlayerUint(selectable);
                    }
                }

                Events.OnSelectedChange.Publish(SelectedUnits);
            }
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
            if (Input.GetMouseButtonUp(0))
            {
                if (IsDrawRect())
                {
                    SelectGroup();
                    SortSelected();
                    Events.OnSelectedChange.Publish(SelectedUnits);
                }

                _groupSelecting = false;
            }
            if (Input.GetMouseButton(0) && IsDrawRect())
            {
                Rect _rect = PointsToRect(_startSelectPoint, Input.mousePosition);
                GUI.Box(_rect, string.Empty);
                //GUI.DrawTexture(_rect, _image);
            }
        }

        private bool IsDrawRect() => Vector3.Distance(_startSelectPoint, Input.mousePosition) > 15 & _groupSelecting == true;
        private bool IsPlayerUint(ISelectable selectable) => selectable.GameObject.CompareTag(GameManager.Instance.PlayersTag) ? true : false;

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
                    if (_rect.Overlaps(PointsToRect(_camera.WorldToScreenPoint(_colliders[i].bounds.min), _camera.WorldToScreenPoint(_colliders[i].bounds.max)), true) &&
                        TypeChecker<ISelectable>.CheckCollider(_colliders[i], out ISelectable selectable))
                        _selectedUnits.Add(selectable);
                }
            }
        }

        private void SortSelected()
        {
            if (_selectedUnits.Count > 1)
            {
                List<ISelectable> _playerUnits = new List<ISelectable>();

                for (int i = 0; i < _selectedUnits.Count; i++)
                {
                    if (TypeChecker<Unit>.CheckSelectable(_selectedUnits[i], out Unit _damageable) && IsPlayerUint(_damageable))
                        _playerUnits.Add(_damageable);
                }

                if (_playerUnits.Count == 0)
                {
                    for (int i = 0; i < _selectedUnits.Count; i++)
                    {
                        if (TypeChecker<Unit>.CheckSelectable(_selectedUnits[i], out Unit _damageable) && IsPlayerUint(_damageable))
                        {
                            _selectedUnits.Clear();
                            _selectedUnits.Add(_damageable);
                            _canControlSelected = false;
                            return;
                        }
                    }
                }

                _canControlSelected = true;
                _selectedUnits.Clear();
                _selectedUnits.AddRange(_playerUnits);
            }
            else if (_selectedUnits.Count == 1)
            {
                _canControlSelected = IsPlayerUint(_selectedUnits[0]);
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
}