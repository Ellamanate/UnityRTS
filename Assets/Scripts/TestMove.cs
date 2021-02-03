using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestMove : MonoBehaviour
{
    }/*public CharacterController Controller;
    public float Speed;
    public float _minWaypointToch;
    public bool RC;
    public Animator _animator;
    public NavMeshAgent _navMeshAgent;
    public FindEnemy _findEnemy;
    public GameObject Obstacle;
    public Vector3 ObstacleSize;
    public Transform _target;
    public List<string> _enemyTags = new List<string>();
    public List<Transform> _enemysInAttackRange = new List<Transform>();
    public bool _moveOrder = false;
    public bool _targetCollision = false;
    public bool _hasPath = false;
    public int _pathIndex = 0;
    public bool _isCalculatingPath = false;
    public Vector3[] _corners;
    public Vector3 _wayPoint;
    public Vector3 _targetPos;
    private Camera _camera;
    private NavMeshPath _path;
    private IEnumerator _action;
    private Vector3 _gravity = new Vector3(0, -10, 0);
    private CharacterAnimator _characterAnimator;
    private CharacterAttack _characterAttack;
    public bool _isAttack = false;

    private void Start()
    {
        _characterAnimator = new CharacterAnimator();
        _characterAnimator.Init(this, _animator);

        _characterAttack = new CharacterAttack();
        _characterAttack.Init(this, _characterAnimator);

        _findEnemy._enemyTags = _enemyTags;
        _camera = Camera.main;
        _path = new NavMeshPath();
        Obstacle = Instantiate(Obstacle);
        Obstacle.SetActive(false);
        Obstacle.GetComponent<NavMeshObstacle>().size = ObstacleSize;
        StartCoroutine(SetTarget(3.5f, 3.5f, 0.5f));
        //StartCoroutine(Follow(0.25f));
        StartCoroutine(ChaseEnemy(0.25f));
        StartCoroutine(MoveToTarget(0.25f));
    }

    private void Update()
    {
        _characterAnimator.AnimationControl();

        if (_moveOrder == false & _target != null & _targetCollision == true & _hasPath == false)
        {

            _characterAttack.SimpleAttack();

            if (_isCalculatingPath == false & _navMeshAgent.enabled == true)
            {
                _navMeshAgent.enabled = false;
                Obstacle.SetActive(true);
                Obstacle.transform.position = transform.position;
            }
        }

        if (RC & Input.GetMouseButtonDown(1))
        {
            RightClick();
        }

        Movement();
    }

    private void Movement()
    {
        if (_hasPath == true)
        {
            Vector3 _difference = _wayPoint - transform.position;
            _difference.Normalize();
            _difference.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_difference), Time.deltaTime);

            Vector3 _movement = (_wayPoint - transform.position).normalized * Speed;
            float _distance = Vector2.Distance(ToVector2(transform.position), ToVector2(_wayPoint));
            if ((_distance > _minWaypointToch & _wayPoint != _targetPos) |
                (_distance > (_movement * Time.deltaTime).magnitude) & _wayPoint == _targetPos)
            {
                Debug.DrawLine(transform.position, transform.position + _movement, Color.blue);
                Controller.Move(_movement * Time.deltaTime + _gravity);
            }
            else
            {
                if (_wayPoint != _targetPos)
                {
                    if (_pathIndex + 1 < _corners.Length)
                    {
                        _wayPoint = _corners[_pathIndex + 1];
                        _pathIndex++;
                    }
                    else
                    {
                        _wayPoint = _targetPos;
                    }
                }
                else
                {
                    _hasPath = false;
                    _moveOrder = false;
                }
                Controller.Move(_gravity);
            }
        }
        else
        {
            Controller.Move(_gravity);
        }
    }

    private void RightClick()
    {
        Ray _ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(_ray, out RaycastHit _hitInfo))
        {
            Collider _collider = _hitInfo.collider;
            if (_collider.tag != "Player")
            {
                if (_collider.tag != "Enemy")
                {
                    _targetPos = _hitInfo.point;
                    ChangeTarget(null);
                    _characterAnimator.StopAnimation();
                    _moveOrder = true;
                }
            }
        }
    }

    private IEnumerator MoveToTarget(float _delay)
    {
        while (true)
        {
            if (_moveOrder == true)
            {
                if (_action != null)
                {
                    StopCoroutine(_action);
                }
                _action = CalculatePath(_targetPos);
                StartCoroutine(_action);
            }
            yield return new WaitForSeconds(_delay);
        }
    }

    private IEnumerator ChaseEnemy(float _delay)
    {
        while (true)
        {
            if (_moveOrder == false & _target != null & _targetCollision == false & _characterAnimator.CheckAnimation("Fight") == false)
            {
                _targetPos = _target.position - (_target.position - transform.position).normalized;
                if (_action != null)
                {
                    StopCoroutine(_action);
                }
                _action = CalculatePath(_targetPos);
                StartCoroutine(_action);
                yield return new WaitUntil(() => _isCalculatingPath == true);
            }
            yield return new WaitForSeconds(_delay);
        }
    }

    private IEnumerator CalculatePath(Vector3 _pos)
    {
        _isCalculatingPath = true;
        Obstacle.SetActive(false);
        _navMeshAgent.enabled = true;
        
        _navMeshAgent.ResetPath();
        yield return new WaitForEndOfFrame();
        _navMeshAgent.CalculatePath(_pos, _path);
        yield return new WaitUntil(() => _path.corners != null);
        _corners = _path.corners;
        if (_corners.Length == 0)
        {
            _wayPoint = _pos;
            _pathIndex = 0;
        }
        else if (Vector3.Distance(transform.position, _corners[0]) < _minWaypointToch)
        {
            _wayPoint = _corners[1];
            _pathIndex = 1;
        }
        else
        {
            _wayPoint = _corners[0];
            _pathIndex = 0;
        }
        _hasPath = true;
        _isCalculatingPath = false;
    }

    private IEnumerator Follow(float _delay)
    {
        while (true)
        {
            if (_moveOrder == true)
            {
                _navMeshAgent.CalculatePath(_targetPos, new NavMeshPath());
                yield return new WaitUntil(() => _navMeshAgent.pathPending == false);
                yield return new WaitForSeconds(0.1f);
                _corners = _navMeshAgent.path.corners;
                _wayPoint = _corners[0];
                _hasPath = true;
            }
            yield return new WaitForSeconds(_delay);
        }
    }

    private IEnumerator SetTarget(float _timer, float _changeTargetDelay, float _delay)
    {
        yield return new WaitForSeconds(0.01f);
        ChangeTarget(_findEnemy.FindNearest());
        Transform _findedTarget;
        yield return new WaitForSeconds(_delay);
        while (true)
        {
            if (_moveOrder == false)
            {
                _findedTarget = _findEnemy.FindNearest();
                if (_target != _findedTarget & _timer >= _changeTargetDelay)
                {
                    _timer = 0;
                    if (Random.Range(1, 5) >= 2)
                    {
                        ChangeTarget(_findedTarget);
                    }
                }
                _targetCollision = false;
                foreach (Transform _enemy in _enemysInAttackRange)
                {
                    if (_enemy == _target)
                    {
                        _targetCollision = true;
                        ResetPath();
                        break;
                    }
                }
                _timer += _delay;
            }
            yield return new WaitForSeconds(_delay);
        }
    }

    private void ResetPath()
    {
        _hasPath = false;
        _corners = new Vector3[0];
        _wayPoint = transform.position;
        _targetPos = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            Transform _parent = other.attachedRigidbody.transform;
            string _collisionTag = _parent.tag;
            foreach (string _enemyTag in _enemyTags)
            {
                if (_enemyTag == _collisionTag)
                {
                    _enemysInAttackRange.Add(_parent);
                }
            }
            if (_parent == _target & _hasPath == true)
            {
                _targetCollision = true;
                ResetPath();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            Transform _parent = other.attachedRigidbody.transform;
            string _collisionTag = _parent.tag;
            foreach (string _enemyTag in _enemyTags)
            {
                if (_enemyTag == _collisionTag)
                {
                    _enemysInAttackRange.Remove(_parent);
                }
            }
            if (_parent == _target)
            {
                _targetCollision = false;
                ResetPath();
            }
        }
    }

    private void ChangeTarget(Transform _newTarget)
    {
        _target = _newTarget;
        _characterAttack.SetTarget(_newTarget);
    }

    private Vector2 ToVector2(Vector3 _vector)
    {
        return new Vector2(_vector.x, _vector.z);
    }

    private void OnDestroy()
    {
        _findEnemy.OnDead(this);
    }
}


/*public class CharacterAttack
{
    private Transform _target;
    private Transform _transform;
    private CharacterAnimator _characterAnimator;
    private TestMove _character;

    public void Init(TestMove _initCharacter, CharacterAnimator _initAnimator)
    {
        _characterAnimator = _initAnimator;
        _character = _initCharacter;
        _transform = _initCharacter.transform;
    }

    public void SimpleAttack()
    {
        Vector3 _difference = _target.position - _transform.position;
        _difference.Normalize();
        _difference.y = 0;
        _transform.rotation = Quaternion.Slerp(_transform.rotation, Quaternion.LookRotation(_difference), Time.deltaTime * 5);
        _characterAnimator.Attack();
    }

    public void SetTarget(Transform _newTarget)
    {
        _target = _newTarget;
    }
}

public class CharacterAnimator
{
    private Animator _animator;
    private TestMove _character;

    public void Init(TestMove _initCharacter, Animator _initAnimator)
    {
        _animator = _initAnimator;
        _character = _initCharacter;
    }

    public void AnimationControl()
    {
        if (_character._hasPath == false & _animator.GetBool("Walk") == true)
        {
            _animator.SetBool("Walk", false);
        }
        else if (_character._hasPath & _animator.GetBool("Walk") == false)
        {
            _animator.SetBool("Walk", true);
        }
    }

    public void Attack()
    {
        if (CheckAnimation("Fight") == false)
        {
            _animator.SetBool("Fight", true);
        }
    }

    public void FightAnimationEnd()
    {
        _animator.SetBool("Walk", false);
        _animator.SetBool("Fight", false);
    }

    public bool CheckAnimation(string _name)
    {
        return _animator.GetCurrentAnimatorStateInfo(0).IsName(_name);
    }

    public void StopAnimation()
    {
        _animator.SetBool("Fight", false);
    }
}*/
   