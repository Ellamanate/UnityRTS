using UnityEngine;
using System.Linq;

public class NearestEnemyFinder : EnemyCollector
{
    public IDamageable FindNearest()
    {
        if (EnemysInRange.Count == 1)
            return EnemysInRange.ElementAt<Unit>(0);

        Unit _potentialTarget = null;
        float _minDistance = Mathf.Infinity;

        foreach (Unit _enemy in EnemysInRange)
        {
            float _distance = Vector3.Distance(transform.position, _enemy.GameObject.transform.position);

            if (_distance < _minDistance)
            {
                _minDistance = _distance;
                _potentialTarget = _enemy;
            }
        }

        return _potentialTarget;
    }
}
