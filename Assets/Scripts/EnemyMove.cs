using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    public NavMeshAgent NavMeshAgent;
    public CharacterController Controller;
    public bool RC;
    private Camera _camera;
    private Vector3 _targetPos;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (RC & Input.GetMouseButtonDown(1))
        {
            RightClick();
        }
        else if (!RC)
        {
            NavMeshAgent.Move(new Vector3(0,0,0));
        }
        //Controller.Move((_targetPos - transform.position).normalized * Time.deltaTime * 5);
    }

    private void RightClick()
    {
        Ray _ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(_ray, out RaycastHit _hitInfo))
        {
            Collider _collider = _hitInfo.collider;
            if (_collider.tag != "Player" & _collider.tag != "Enemy")
            {
                _targetPos = _hitInfo.point;
                NavMeshAgent.SetDestination(_hitInfo.point);
            }
        }
    }
}
