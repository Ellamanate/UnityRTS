using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class CharacterMove : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _angularSpeed = 2000;
    [SerializeField] private float _stoppingDistance = 0.1f;
    [SerializeField] private CharacterController _controller;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private NavMeshObstacle _navMeshObstacle;
    [SerializeField] private bool _block = false;
    [SerializeField] private bool _isMove = false;
    [SerializeField] private bool _isCalculatingPath = false;
    [SerializeField] private Vector3 _targetPos;
    private Vector3 _gravity = new Vector3(0, -10, 0);
    private IEnumerator _repath;
    private const float _repathTime = 0.25f;

    public UnityAction PathComplete;
    public bool IsMove => _isMove;
    public float Speed { get => _speed; set { if (value >= 0) _speed = value; } }
    public bool Block { get => _block; set => _block = OnBlock(value); }
    public Vector3 TargetPos => _targetPos;

    public void ResetPath()
    {
        _isMove = false;
        _targetPos = transform.position;

        if (_navMeshAgent.enabled)
        {
            _navMeshAgent.ResetPath();
            _navMeshAgent.enabled = false;
            _navMeshObstacle.enabled = true;
        }
    }

    public void MoveToPoint(Vector3 point)
    {
        if (!_block)
        {
            _targetPos = point;

            if (!_isCalculatingPath)
            {
                _isCalculatingPath = true;
                StartCoroutine(CalculatePath());
            }
        }
    }

    private void OnEnable()
    {
        StartCoroutine(_repath);
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
        _repath = Repath(_repathTime);
    }

    private void Update()
    {
        if (_navMeshAgent.enabled)
        {
            _controller.Move(_navMeshAgent.velocity * Time.deltaTime + _gravity);

            if (_isMove & !_isCalculatingPath & _navMeshAgent.velocity.magnitude <= _stoppingDistance)
            {
                _isMove = false;
                _navMeshAgent.enabled = false;
                _navMeshObstacle.enabled = true;

                if (PathComplete != null)
                    PathComplete.Invoke();
            }
        }
    } 

    private bool OnBlock(bool block)
    {
        if (block & _navMeshAgent.enabled)
        {
            _navMeshAgent.velocity = new Vector3(0, 0, 0);
            _navMeshAgent.ResetPath();
        }

        return block;
    }

    private IEnumerator Repath(float delay)
    {
        while (true)
        {
            yield return CalculatePath();
            yield return new WaitForSeconds(delay);
        }
    }

    private IEnumerator CalculatePath()
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

        yield return null;
    }
}
