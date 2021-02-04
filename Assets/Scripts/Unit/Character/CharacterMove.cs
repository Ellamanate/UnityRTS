using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class CharacterMove : MonoBehaviour
{
    public UnityAction PathComplete;
    public bool IsMove { get => _isMove; }
    public float Speed { get => _speed; set { if (value >= 0) _speed = value; } }
    public bool Block { get => _block; set => _block = OnBlock(value); }
    public Vector3 TargetPos { get => _targetPos; }

    [SerializeField] private float _speed;
    [SerializeField] private float _angularSpeed = 2000;
    [SerializeField] private float _stoppingDistance = 0.1f;
    [SerializeField] private CharacterController _controller;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private NavMeshObstacle _navMeshObstacle;
    [SerializeField] private List<Transform> _patrolPoints = new List<Transform>();
    private bool _block = false;
    [SerializeField] private bool _isMove = false;
    [SerializeField] private bool _isCalculatingPath = false;
    private Vector3 _targetPos;
    private Vector3 _gravity = new Vector3(0, -10, 0);
    private IEnumerator _calculatePath;

    public void ResetPath()
    {
        _isMove = false;
        _targetPos = transform.position;

        if (_navMeshAgent.enabled)
            _navMeshAgent.ResetPath();
    }

    public void MoveToPoint(Vector3 _point)
    {
        if (!_block)
        {
            _targetPos = _point;
            _isCalculatingPath = true;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(_calculatePath);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Awake()
    {
        _navMeshAgent.speed = _speed;
        _navMeshAgent.angularSpeed = _angularSpeed;
        _navMeshAgent.stoppingDistance = _stoppingDistance;
        _targetPos = transform.position;
        _calculatePath = CalculatePath(0.25f);
    }

    private void Update()
    {
        if (_navMeshAgent.enabled)
        {
            _controller.Move(_navMeshAgent.velocity * Time.deltaTime + _gravity);

            //if (_navMeshAgent.remainingDistance <= (_navMeshAgent.velocity.normalized * _speed * Time.deltaTime).magnitude)
            if (_isMove & !_isCalculatingPath & _navMeshAgent.velocity.magnitude <= _stoppingDistance)
            {
                PathComplete.Invoke();
                _isMove = false;
                _navMeshAgent.enabled = false;
                _navMeshObstacle.enabled = true;
            }
        }
    } 

    private bool OnBlock(bool _block)
    {
        if (_block & _navMeshAgent.enabled)
        {
            _navMeshAgent.velocity = new Vector3(0, 0, 0);
            _navMeshAgent.ResetPath();
        }

        return _block;
    }

    private IEnumerator CalculatePath(float _delay)
    {
        while (true)
        {
            if (_isCalculatingPath)
            {
                if (!_navMeshAgent.enabled)
                {
                    _navMeshObstacle.enabled = false;
                    yield return new WaitForEndOfFrame();
                    _navMeshAgent.enabled = true;
                }

                _navMeshAgent.SetDestination(_targetPos);
                yield return new WaitForEndOfFrame();

                _isMove = true;
                _isCalculatingPath = false;
            }

            yield return new WaitForSeconds(_delay);
        }
    }

    private Vector3 VectorXZ(Vector3 _vector)
    {
        return new Vector3(_vector.x, 0, _vector.z);
    }
}
