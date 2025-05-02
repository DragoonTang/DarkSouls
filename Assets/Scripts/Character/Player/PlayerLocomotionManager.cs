using System;
using UnityEngine;

/// <summary>
/// 每个客户端上，可能出现多个该脚本的实例
/// 其中，只有一个是自己的，其他的是别的玩家生成在本机上的克隆
/// 所以，需要把自己的输入值传给网络对象，由别人的客户端上的克隆来获取
/// 反之，本机上的克隆也要把网络对象上的输入值传给自己，以此来实现网络同步（复现别人玩家的操作）
/// </summary>
public class PlayerLocomotionManager : CharacterLocomotionManager
{
    PlayerManager player;

    public float verticalMovement;
    public float horizontalMovement;
    public float moveAmount;

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

    protected override void Update()
    {
        base.Update();

        // 把自己这边的输入值传给网络对象
        if (player.IsOwner)
        {
            player.characterNetworkManager.verticalMovement.Value = verticalMovement;
            player.characterNetworkManager.horizontalMovement.Value = horizontalMovement;
            player.characterNetworkManager.moveAmount.Value = moveAmount;
        }
        // 当这个脚本是别人的时候，则要从网络对象上获取输入值
        else
        {
            verticalMovement = player.characterNetworkManager.verticalMovement.Value;
            horizontalMovement = player.characterNetworkManager.horizontalMovement.Value;
            moveAmount = player.characterNetworkManager.moveAmount.Value;

            // 然后通过获取的值，在本机上传给动画，以实现动画的同步
            player.playerAnimatorManager.UpdateAnimatorMovement(0,moveAmount);

            // 如果是锁敌状态，需要横轴移动时
        }
    }

    /// <summary>
    /// 由PlayerManager每帧调用
    /// </summary>
    public void HandleAllMovement()
    {
        // 地面上移动
        HandleGroundedMovement();
        // 旋转
        HandleRotation();
    }

    void GetMovementValue()
    {
        // 获取垂直和水平输入值
        verticalMovement = PlayerInputManager.instance.verticalInput;
        horizontalMovement = PlayerInputManager.instance.horizontalInput;
        moveAmount = PlayerInputManager.instance.moveAmount;
    }

    void HandleGroundedMovement()
    {
        GetMovementValue();

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
