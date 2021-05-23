using System;
using UnityEngine;

public static class Extensions
{
    public static Vector3[] CalculateGroupPoints(this Vector3 _targetPoint, float maxUnitSize, int selectedCount)
    {
        Vector3[] _newPoints = new Vector3[selectedCount];

        for (int i = 0; i < selectedCount; i++)
        {
            float _angle = (360 / selectedCount) * i * Mathf.PI / 180;
            _newPoints[i] = _targetPoint + Vector3.forward.RotateXZ(_angle) * maxUnitSize;
        }

        return _newPoints;
    }

    public static Vector3 RotateXZ(this Vector3 _vector, float _angle)
    {
        return new Vector3(_vector.x * Mathf.Cos(_angle) + _vector.z * Mathf.Sin(_angle), 0,
                           _vector.z * Mathf.Cos(_angle) - _vector.x * Mathf.Sin(_angle));
    }
}