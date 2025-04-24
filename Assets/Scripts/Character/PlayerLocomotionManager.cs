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
    /// 由PlayerManager每帧调用
    /// </summary>
    public void HandleAllMovement()
    {
        // 地面上移动
        HandleGroundedMovement();
    }

    void GetVerticalInputAndHorizontalInput()
    {
        // 获取垂直和水平输入值
        verticalMovement = PlayerInputManager.instance.verticalInput;
        horizontalMovement = PlayerInputManager.instance.horizontalInput;
    }

    void HandleGroundedMovement()
    {
        GetVerticalInputAndHorizontalInput();

        // 当前移动方向等于X轴和Z轴的输入值与相机的前向和右向的乘积
        moveDirection = PlayerCamera.instance.transform.forward * verticalMovement + PlayerCamera.instance.transform.right * horizontalMovement;
        // 归一化
        moveDirection.Normalize();
        // 因为我们只需要在X轴和Z轴上移动，所以Y轴的值设为0
        moveDirection.y = 0;

        // 如果当前正在地面移动，则输入值大于0.5为奔跑，否则为行走
        if (PlayerInputManager.instance.moveAmount > 0)
            player.characterController.Move((PlayerInputManager.instance.moveAmount > .5f ? runningSpeed : walkingSpeed) * Time.deltaTime * moveDirection);
    }
}
