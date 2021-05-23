using UnityEngine;


public class NearestEnemyFinder : EnemyCollector
{
    public IDamageable FindNearest()
    {
        if (_enemysInRange.Count == 1)
            return _enemysInRange[0];

        Unit _potentialTarget = null;
        float _minDistance = Mathf.Infinity;

        foreach (Unit _enemy in _enemysInRange)
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
