using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float defaultMoveSpeed;
    [SerializeField] private float runningSpeed;
    private float moveSpeed;
    private bool isWalking;
    private bool isRunning;

    private Animator animator;

    public Cinemachine.CinemachineVirtualCamera virtualCamera;

    private void Awake()
    {
        virtualCamera = FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector2 inputVector = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            inputVector.y = +1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector.y = -1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputVector.x = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector.x = +1;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Jump");
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            animator.SetTrigger("Attack");
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
            moveSpeed = runningSpeed;
        }
        else
        {
            isRunning = false;
            moveSpeed = defaultMoveSpeed; // Reset to default walking speed
        }

        // Get the camera's forward direction, without the vertical component
        Vector3 cameraForward = virtualCamera.transform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        // Normalize input vector
        inputVector = inputVector.normalized;

        Vector3 moveDir = Quaternion.LookRotation(cameraForward) * new Vector3(inputVector.x, 0f, inputVector.y);
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        isWalking = moveDir != Vector3.zero;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);

    }

    public bool IsWalking()
    {
        return isWalking;
    }

    public bool IsRunning()
    {
        return isRunning;
    }

}
