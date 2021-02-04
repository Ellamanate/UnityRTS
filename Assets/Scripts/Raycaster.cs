using System.Collections;
using UnityEngine;


public class Raycaster : Singleton<Raycaster>
{
    public int SelectMask { get => _selectMask; }
    public int OrderMask { get => _orderMask; }
    public int DefaultMask { get => _defaultMask; }
    public int HitBoxlayer { get => _hitBoxlayer; }
    public int AgentLayer { get => _agentLayer; }

    private int _selectMask;
    private int _orderMask;
    private int _defaultMask;
    private int _hitBoxMask;
    private int _hitBoxlayer;
    private int _agentLayer;
    private Camera _defaultCamera;
    [SerializeField] private float _defaultRange = 100;

    private void Start()
    {
        _defaultCamera = Camera.main;
        _selectMask = LayerMask.GetMask("HitBox");
        _orderMask = LayerMask.GetMask(new string[] { "HitBox", "Default" });
        _defaultMask = LayerMask.GetMask("Default");
        _hitBoxlayer = LayerMask.NameToLayer("HitBox");
        _agentLayer = LayerMask.NameToLayer("Agent");
    }

    public bool SelectMouseRaycast(out RaycastHit _hitInfo, out Rigidbody _attachedRigidbody)
    {
        return MouseRaycast(out _hitInfo, _selectMask, out _attachedRigidbody) & _attachedRigidbody != null;
    }

    public bool SelectMouseRaycast(out Rigidbody _attachedRigidbody)
    {
        return MouseRaycast(out RaycastHit _hitInfo, _selectMask, out _attachedRigidbody) & _attachedRigidbody != null;
    }

    public bool OrderMouseRaycast(out RaycastHit _hitInfo, out Rigidbody _attachedRigidbody)
    {
        return MouseRaycast(out _hitInfo, _orderMask, out _attachedRigidbody);
    }

    public bool DefaultMouseRaycast(out RaycastHit _hitInfo)
    {
        return MouseRaycast(out _hitInfo, _defaultMask);
    }

    public bool MouseRaycast(out RaycastHit _hitInfo, int _mask)
    {
        return Raycast(Input.mousePosition, out _hitInfo, _mask);
    }

    public bool MouseRaycast(out RaycastHit _hitInfo, int _mask, out Rigidbody _attachedRigidbody)
    {
        bool _isHit = Raycast(Input.mousePosition, out _hitInfo, _mask);

        if (_isHit)
            _attachedRigidbody = _hitInfo.collider.attachedRigidbody;
        else
            _attachedRigidbody = null;

        return _isHit;
    }

    public bool PointRaycast(Vector3 _position, out RaycastHit _hitInfo)
    {
        return Raycast(_position, out _hitInfo, _defaultMask);
    }

    public bool Raycast(Vector3 _position, out RaycastHit _hitInfo, int _mask)
    {
        Ray _ray = _defaultCamera.ScreenPointToRay(_position);
        return Physics.Raycast(_ray, out _hitInfo, _defaultRange, _mask);
    }

    public bool Raycast(Vector3 _position, out RaycastHit _hitInfo, int _mask, Camera _camera)
    {
        Ray _ray = _camera.ScreenPointToRay(_position);
        return Physics.Raycast(_ray, out _hitInfo, _defaultRange, _mask);
    }
}