using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private TimingManager _theTimingManager;
    private CameraController _theCamController;
    private StatusManager _theStatusManager;
    
    private bool _canMove = true;
    private bool _isFalling = false;
    private Rigidbody _rigid;
    private Vector3 _originPos;

    // 이동
    [SerializeField] private float moveSpeed = 3f;
    private Vector3 _dir = new();
    public Vector3 destPos = new();
    
    // 회전
    [SerializeField] private float spinSpeed = 270f;
    private Vector3 _rotDir = new();
    private Quaternion _destRot = new();
    [SerializeField] private Transform fakeCube;
    [SerializeField] private Transform realCube;
    
    // 들썩거림
    [SerializeField] private float recoilPosY = 0.25f;
    [SerializeField] private float recoilSpeed = 1.5f;

    // 터치패드
    [SerializeField] private TouchpadManager theTouchpadManager;
    
    // Goal에 도착했을 때 플래그 
    public static bool SCanPressKey = true;
    
    private void Start()
    {
        _theTimingManager = FindObjectOfType<TimingManager>();
        _theCamController = FindObjectOfType<CameraController>();
        _theStatusManager = FindObjectOfType<StatusManager>();
        
        _rigid = GetComponentInChildren<Rigidbody>();
        _originPos = transform.position;
    }

    public void Initialized()
    {
        transform.position = Vector3.zero;
        destPos = Vector3.zero;
        realCube.localPosition = Vector3.zero;
        _canMove = true;
        SCanPressKey = true;
        _isFalling = false;
        _rigid.useGravity = false;
        _rigid.isKinematic = true;
    }
    
    private void Update()
    {
        if (GameManager.Instance.isStartGame == false) return;
        
        CheckFalling();
        
        if (theTouchpadManager.isTouched)
        {
            if (_canMove == false || SCanPressKey == false || _isFalling) return;

            CalcNextPos();

            if (_theTimingManager.CheckTiming())
            {
                CalcNextRot();
                StartAction();
            }
        }
    }

    void CalcNextPos()
    {
        // Debug.Log("CalcNextPos");
        
        // Direction Setting
        // TODO 두 축을 동시에 누르면 큐브가 삐뚤어지는 버그있음
        //_dir.Set(Input.GetAxisRaw("Vertical"), 0, Input.GetAxisRaw("Horizontal"));
        _dir = theTouchpadManager.dir;
        
        // Calc Dest Pos
        destPos = transform.position + new Vector3(-_dir.x, 0, _dir.z);
    }

    void CalcNextRot()
    {
        // Rotation Setting
        _rotDir = new Vector3(-_dir.z, 0f, -_dir.x);
        // Debug.Log(_rotDir);
        fakeCube.RotateAround(transform.position, _rotDir, spinSpeed);
        // fakeCube.Rotate(_rotDir, spinSpeed);
        _destRot = fakeCube.rotation;
    }
    
    void StartAction()
    {
        // Debug.Log("StartAction");
        StartCoroutine(CoMove());
        StartCoroutine(CoSpin());
        StartCoroutine(CoRecoil());
        StartCoroutine(_theCamController.ZoomCam());
    }

    IEnumerator CoMove()
    {
        _canMove = false;
        while (Vector3.SqrMagnitude(transform.position - destPos) >= 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = destPos;
        _canMove = true;
    }

    IEnumerator CoSpin()
    {
        while (Quaternion.Angle(realCube.rotation, _destRot) > 0.5f)
        {
            realCube.rotation = Quaternion.RotateTowards(realCube.rotation, _destRot, spinSpeed * Time.deltaTime);
            yield return null;
        }

        realCube.rotation = _destRot;
    }

    IEnumerator CoRecoil()
    {
        while (realCube.position.y < recoilPosY)
        {
            realCube.position += new Vector3(0, recoilSpeed * Time.deltaTime, 0);
            yield return null;
        }

        while (realCube.position.y > 0)
        {
            realCube.position -= new Vector3(0, recoilSpeed * Time.deltaTime, 0);
            yield return null;
        }
        
        realCube.localPosition = Vector3.zero;
    }

    void CheckFalling()
    {
        if (_isFalling || _canMove == false) return;
        
        Debug.DrawRay(transform.position, Vector3.down * 1.1f, Color.green);
        if (!Physics.Raycast(transform.position, Vector3.down, out var hitInfo,1.1f))
        {
            Falling();
        }
    }

    void Falling()
    {
        _isFalling = true;
        _rigid.useGravity = true;
        _rigid.isKinematic = false;
    }

    public void ResetFalling()
    {
        // 체력 감소
        _theStatusManager.DecreaseHp(1);
        
        // Falling 사운드 재생
        AudioManager.Instance.PlaySFX("Falling");

        if (_theStatusManager.IsDead())
            return;
        
        _isFalling = false;
        _rigid.useGravity = false;
        _rigid.isKinematic = true;

        transform.position = _originPos;
        realCube.localPosition = Vector3.zero;
    }
}
