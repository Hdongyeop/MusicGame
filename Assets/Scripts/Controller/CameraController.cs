using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float followSpeed;
    [SerializeField] private float zoomDistance = -1.25f;

    private Vector3 _playerDistance = new();
    private float _hitDistance;
    
    private void Start()
    {
        _playerDistance = transform.position - target.position;
    }

    private void Update()
    {
        Vector3 tDestPos = target.position + _playerDistance + (transform.forward * _hitDistance);
        transform.position = Vector3.Lerp(transform.position, tDestPos, followSpeed * Time.deltaTime);
    }

    public IEnumerator ZoomCam()
    {
        _hitDistance = zoomDistance;

        yield return new WaitForSeconds(.15f);

        _hitDistance = 0f;
    }
}
