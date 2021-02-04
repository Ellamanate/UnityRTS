using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class Avoidance : MonoBehaviour
{
    public CharacterMove CharacterMove;
    private Vector3 _avoidanceVector;
    private Camera _camera;
    private int _layerMask;
    private float _lineRange;
    private float _avoidanceForce;
    private Collider[] _colliders;
    private Vector3 _offset;

    private void Start()
    {
        _lineRange = 1.5f;
        _avoidanceForce = 2f;
        _offset = new Vector3(0, 2.5f, 0);
        _camera = Camera.main;
        string[] _layers = new string[2];
        _layers[0] = "CollisionSensor";
        _layers[1] = "Wall";
        _layerMask = LayerMask.GetMask(_layers);
        StartCoroutine(CalculateAvoidance(0.25f));
    }

    private IEnumerator CalculateAvoidance(float _delay)
    {
        while (true)
        {
            if (CharacterMove.IsMove)
            {
                _colliders = Physics.OverlapSphere(transform.position, 10, _layerMask);
                _avoidanceVector = new Vector3(0, 0, 0);
                for (int j = 90; j > -91; j -= 30)
                {
                    float _angle = j * Mathf.PI / 180;
                    Vector3 _line = RotateXZ(CharacterMove.NavMeshAgent.velocity, _angle);
                    Vector3 _movementPoint = transform.position + _line.normalized * _lineRange + _offset;
                    //Debug.DrawLine(transform.position + new Vector3(0, 2.5f, 0), _movementPoint, Color.yellow);
                    for (int i = 0; i < _colliders.Length; i++)
                    {
                        if (_colliders[i].attachedRigidbody != null)
                        {
                            if (_colliders[i].attachedRigidbody.gameObject != CharacterMove.gameObject)
                            {
                                LineAvoidance(i, _movementPoint);
                                //break;
                            }
                        }
                        else
                        {
                            LineAvoidance(i, _movementPoint);
                            //break;
                        }
                    }
                }
                _avoidanceVector.y = 0;
                //Debug.DrawLine(transform.position, transform.position - _avoidanceVector.normalized * _avoidanceForce, Color.red);
                CharacterMove.AvoidanceVector = - _avoidanceVector.normalized * _avoidanceForce;
            }
            yield return new WaitForSeconds(_delay);
        }
    }

    private void LineAvoidance(int i, Vector3 _movementPoint)
    {
        Vector3 _resultVector = _colliders[i].transform.position - _movementPoint;
        float _distance = _resultVector.magnitude;
        if (_colliders[i].bounds.Contains(_movementPoint))
        {
            _avoidanceVector += _resultVector.normalized / _distance;
        }
    }

    private Vector3 RotateXZ(Vector3 _vector, float _angle)
    {
        return new Vector3(_vector.x * Mathf.Cos(_angle) + _vector.z * Mathf.Sin(_angle),
            0, _vector.z * Mathf.Cos(_angle) - _vector.x * Mathf.Sin(_angle));
    }
}*/
