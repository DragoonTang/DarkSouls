using System;
using UnityEngine;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    PlayerManager player;

    public float verticalMovement;
    public float horizontalMovement;
    Vector3 moveDirection;

    [SerializeField] float walkingSpeed = 2f;
    [SerializeField] float runningSpeed = 5f;

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
}
