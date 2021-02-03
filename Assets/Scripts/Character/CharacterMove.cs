using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class CharacterMove : MonoBehaviour
{
    public float Speed;
    public CharacterController Controller;
    public NavMeshAgent NavMeshAgent;
    public NavMeshObstacle NavMeshObstacle;
    public List<Transform> _patrolPoints = new List<Transform>();
    [HideInInspector] public Vector3 AvoidanceVector;
    [HideInInspector] public Vector3 _targetPos;
    public bool IsMove { get { return _isMove; } }
    public UnityEvent PathComplete { get { return _pathComplete; } }
    public bool Block
    {
        get { return _block; }
        set
        {
            if (value == true & NavMeshAgent.enabled == true)
            {
                NavMeshAgent.ResetPath();
                _targetPos = transform.position;
            }
        }
    }

    private bool _block = false;
    private bool _isMove = false;
    private bool _isCalculatingPath = false;
    private Vector3 _gravity = new Vector3(0, -10, 0);
    private Vector3 _movement;
    private UnityEvent _pathComplete = new UnityEvent();

    private void Start()
    {
        NavMeshAgent.speed = Speed;
        _targetPos = transform.position;
        StartCoroutine(CalculatePath(0.25f));
    }

    private void Update()
    {
        if (NavMeshAgent.enabled == true)
        {
            Controller.Move(NavMeshAgent.velocity * Time.deltaTime + _gravity);

            if (Vector3.Distance(VectorXZ(transform.position), VectorXZ(NavMeshAgent.pathEndPosition)) <= (NavMeshAgent.velocity * Time.deltaTime).magnitude)
            {
                PathComplete.Invoke();
                _isMove = false;
            }
            
            if (_isMove == false & _isCalculatingPath == false)
            {
                NavMeshAgent.enabled = false;
                NavMeshObstacle.enabled = true;
            }
        }
    } 

    public void ResetPath()
    {
        _isMove = false;
        _targetPos = transform.position;
        if (NavMeshAgent.enabled == true)
        {
            NavMeshAgent.ResetPath();
        }
    }

    public void MoveToPoint(Vector3 _point)
    {
        if (_block == false)
        {
            _targetPos = _point;
            _isMove = true;
        }
    }

    private IEnumerator CalculatePath(float _delay)
    {
        while (true)
        {
            if (_isMove == true)
            {
                _isCalculatingPath = true;
                if (NavMeshAgent.enabled == false)
                {
                    NavMeshObstacle.enabled = false;
                    yield return new WaitForEndOfFrame();
                    NavMeshAgent.enabled = true;
                }

                NavMeshAgent.SetDestination(_targetPos);
                _isCalculatingPath = false;
            }
            yield return new WaitForSeconds(_delay);
        }
    }

    private Vector3 VectorXZ(Vector3 _vector)
    {
        return new Vector3(_vector.x, 0, _vector.z);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
