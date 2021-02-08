using System.Collections.Generic;
using UnityEngine;


public class UnitOrdering
{
    private string[] _skillHotKeys;

    public UnitOrdering(string[] _newSkillHotKeys)
    {
        _skillHotKeys = _newSkillHotKeys;
    }

    public void GiveOrder(IReadOnlyList<ISelectable> _selectedUnits, Order _order)
    {
        if (Raycaster.Instance.OrderMouseRaycast(out RaycastHit _hitInfo, out Rigidbody _attachedRigidbody))
        {
            if (_attachedRigidbody == null)
            {
                if (_selectedUnits.Count == 1)
                {
                    if (TypeChecker<Character>.CheckSelectable(_selectedUnits[0], out Character _character))
                        _order.PointSelected(_character, _hitInfo.point);
                }
                else
                {
                    Vector3[] _points = CalculateGroupPoints(_hitInfo.point, _selectedUnits);

                    for (int i = 0; i < _points.Length; i++)
                        if (TypeChecker<Character>.CheckSelectable(_selectedUnits[i], out Character _character))
                            _order.PointSelected(_character, _points[i]);
                }
            }
            else
            {
                for (int i = 0; i < _selectedUnits.Count; i++)
                {
                    if (TypeChecker<ItemPrefab>.CheckGameObject(_attachedRigidbody.gameObject, out ItemPrefab _itemPrefab))
                    {
                        if (TypeChecker<Character>.CheckSelectable(_selectedUnits[i], out Character _character))
                        {
                            _order.ItemSelected(_character, _itemPrefab, out bool _isBreak);

                            if (_isBreak)
                                break;
                        }
                    }
                    else if (TypeChecker<IDamageable>.CheckGameObject(_attachedRigidbody.gameObject, out IDamageable _damageable))
                    {
                        if (TypeChecker<Character>.CheckSelectable(_selectedUnits[i], out Character _character))
                            _order.CharacterSelected(_character, _damageable);
                    }
                }
            }
        }
    }

    public void IsPressSkillHotkey(IReadOnlyList<ISelectable> _selectedUnits)
    {
        if (_selectedUnits.Count != 0)
            for (int i = 0; i < _skillHotKeys.Length; i++)
                if (Input.GetKeyDown(_skillHotKeys[i]))
                    for (int j = 0; j < _selectedUnits.Count; j++)
                        if (_selectedUnits[j].GameObject.GetComponent<SkillCaster>() != null)
                            _selectedUnits[j].GameObject.GetComponent<SkillCaster>().SkillManager.TryCastSkill(i);
    }

    private Vector3[] CalculateGroupPoints(Vector3 _targetPoint, IReadOnlyList<ISelectable> _selectedUnits)
    {
        ///// 
        // Здесь получение максимального размера юнита в группе

        float _maxUnitSize = 2;
        /////

        Vector3[] _newPoints = new Vector3[_selectedUnits.Count];

        for (int i = 0; i < _selectedUnits.Count; i++)
        {
            float _angle = (360 / _selectedUnits.Count) * i * Mathf.PI / 180;
            _newPoints[i] = _targetPoint + RotateXZ(Vector3.forward * _maxUnitSize, _angle);
        }
        
        return _newPoints;
    }

    private Vector3 RotateXZ(Vector3 _vector, float _angle)
    {
        return new Vector3(_vector.x * Mathf.Cos(_angle) + _vector.z * Mathf.Sin(_angle), 0,
                           _vector.z * Mathf.Cos(_angle) - _vector.x * Mathf.Sin(_angle));
    }
}