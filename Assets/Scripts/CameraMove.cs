using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform Player;
    private Vector3 StartPos;

    private void Awake()
    {
        StartPos = transform.position;
    }
    
    private void Update()
    {
        transform.position = Player.position + StartPos;
    }
}
