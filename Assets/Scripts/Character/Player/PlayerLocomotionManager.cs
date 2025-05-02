using System;
using UnityEngine;

/// <summary>
/// ÿ���ͻ����ϣ����ܳ��ֶ���ýű���ʵ��
/// ���У�ֻ��һ�����Լ��ģ��������Ǳ����������ڱ����ϵĿ�¡
/// ���ԣ���Ҫ���Լ�������ֵ������������ɱ��˵Ŀͻ����ϵĿ�¡����ȡ
/// ��֮�������ϵĿ�¡ҲҪ����������ϵ�����ֵ�����Լ����Դ���ʵ������ͬ�������ֱ�����ҵĲ�����
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

        // ���Լ���ߵ�����ֵ�����������
        if (player.IsOwner)
        {
            player.characterNetworkManager.verticalMovement.Value = verticalMovement;
            player.characterNetworkManager.horizontalMovement.Value = horizontalMovement;
            player.characterNetworkManager.moveAmount.Value = moveAmount;
        }
        // ������ű��Ǳ��˵�ʱ����Ҫ����������ϻ�ȡ����ֵ
        else
        {
            verticalMovement = player.characterNetworkManager.verticalMovement.Value;
            horizontalMovement = player.characterNetworkManager.horizontalMovement.Value;
            moveAmount = player.characterNetworkManager.moveAmount.Value;

            // Ȼ��ͨ����ȡ��ֵ���ڱ����ϴ�����������ʵ�ֶ�����ͬ��
            player.playerAnimatorManager.UpdateAnimatorMovement(0,moveAmount);

            // ���������״̬����Ҫ�����ƶ�ʱ
        }
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

    void GetMovementValue()
    {
        // ��ȡ��ֱ��ˮƽ����ֵ
        verticalMovement = PlayerInputManager.instance.verticalInput;
        horizontalMovement = PlayerInputManager.instance.horizontalInput;
        moveAmount = PlayerInputManager.instance.moveAmount;
    }

    void HandleGroundedMovement()
    {
        GetMovementValue();

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
