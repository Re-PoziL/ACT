using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform target;
    
    [SerializeField, Range(0,3)] private float distance =2f;

    [SerializeField, Range(0,1)] private float mouseSensitivity = 0.1f;

    [SerializeField, Range(0, 10)] private float lerpTime = 10f;
    [SerializeField, Range(0, 10)] private float rotationSmoothTime = 0.12f;

    [SerializeField] private Vector2 _camDistanceMinMax;
    [SerializeField] private float _camLerpTime = 50f;
    [SerializeField] private LayerMask _colliderLayerMask;
    [SerializeField,Range(0,3)]private float _camCheckDistance;

    

    //[SerializeField] private AnimationCurve armLengthCurve;

    public float pitch { get; private set; }
    public float yaw { get; private set; }

    private Vector3 currentVelocity;
    private Vector3 currentRotation;

    public InputSystem _inputSystem;
    private Vector3 _camDirection;
    private float _camDistance;
    private Transform _playerCamera;

    private void Awake()
    {
        _playerCamera = Camera.main.transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        SetCursor();
        //一开始把transform设为0，0，-0.1，这样才是正后方
        _camDirection = transform.localPosition.normalized;
    }

    private static void SetCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }


    // Update is called once per frame
    void Update()
    {
    }

    private void LateUpdate()
    {
        UpdateRotation();
        UpdatePosition();
        CheckCameraOcclusionAndCollision(_playerCamera);
    }


    private void UpdatePosition()
    {

        //获得从摄像机z轴到目标的向量
        Vector3 pos = target.position - transform.forward * distance;
        //pos.z = armLengthCurve.Evaluate(pitch)* -1;
        transform.position = Vector3.Lerp(transform.position, pos, lerpTime * Time.deltaTime);


    }

    void UpdateRotation()
    {
        //pitch是绕x轴旋转,和lookDelta.y是反向的
        pitch -= _inputSystem.lookDelta.y * mouseSensitivity;
        //yaw是绕y轴旋转
        yaw += _inputSystem.lookDelta.x * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, -80, 90);
        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref currentVelocity, rotationSmoothTime);
        transform.rotation = Quaternion.Euler(currentRotation);
    }

    /// <summary>
    /// 检查摄像机遮挡和碰撞
    /// </summary>
    private void CheckCameraOcclusionAndCollision(Transform camera)
    {
        //因为transform一直在变，所以desiredCamPosition也在变，里面是不变的，只是一个指向摄像机正后方的向量，距离由_camCheckDistance来控制
        Vector3 desiredCamPosition = transform.TransformPoint(_camDirection * _camCheckDistance);
        if(Physics.Linecast(transform.position,desiredCamPosition,out RaycastHit hitInfo, _colliderLayerMask))
        {
            _camDistance = Mathf.Clamp(hitInfo.distance, _camDistanceMinMax.x, _camDistanceMinMax.y);
        }
        else
        {
            _camDistance = _camDistanceMinMax.y;
        }

        camera.transform.localPosition = Vector3.Lerp(camera.transform.localPosition, _camDirection * _camDistance, _camLerpTime * Time.deltaTime);

    }


}
