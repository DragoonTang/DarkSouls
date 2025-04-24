using System;
using UnityEngine;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    PlayerManager player;

    public float verticalMovement;
    public float horizontalMovement;
    Vector3 moveDirection;
    Vector3 targetRotationDirection;

    [SerializeField] float walkingSpeed = 2f;
    [SerializeField] float runningSpeed = 5f;
    [SerializeField] float rotationSpeed = 15;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }

    /// <summary>
    /// ��PlayerManagerÿ֡����
    /// </summary>
    public void HandleAllMovement()
    {
        // �������ƶ�
        HandleGroundedMovement();
        // ��ת
        HandleRotation();
    }

    void GetVerticalInputAndHorizontalInput()
    {
        // ��ȡ��ֱ��ˮƽ����ֵ
        verticalMovement = PlayerInputManager.instance.verticalInput;
        horizontalMovement = PlayerInputManager.instance.horizontalInput;
    }

    void HandleGroundedMovement()
    {
        GetVerticalInputAndHorizontalInput();

        // ��ǰ�ƶ��������X���Z�������ֵ�������ǰ�������ĳ˻�
        moveDirection = PlayerCamera.instance.transform.forward * verticalMovement + PlayerCamera.instance.transform.right * horizontalMovement;
        // ��һ��
        moveDirection.Normalize();
        // ��Ϊ����ֻ��Ҫ��X���Z�����ƶ�������Y���ֵ��Ϊ0
        moveDirection.y = 0;

        // �����ǰ���ڵ����ƶ���������ֵ����0.5Ϊ���ܣ�����Ϊ����
        if (PlayerInputManager.instance.moveAmount > 0)
            player.characterController.Move((PlayerInputManager.instance.moveAmount > .5f ? runningSpeed : walkingSpeed) * Time.deltaTime * moveDirection);
    }

    void HandleRotation()
    {
        targetRotationDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement + PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
        targetRotationDirection.Normalize();
        targetRotationDirection.y = 0;

        if (targetRotationDirection == Vector3.zero)
            targetRotationDirection = transform.forward;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetRotationDirection), rotationSpeed * Time.deltaTime);
    }
}
