using UnityEngine;

public class PlayerManager : CharacterManager
{
    [HideInInspector] PlayerLocomotionManager playerLocomotionManager;
    [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;

    override protected void Awake()
    {
        base.Awake();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
    }

    override protected void Update()
    {
        base.Update();

        // ��������Ʒ�ӵ���ߵĶ���
        if (!IsOwner)
            return;

        playerLocomotionManager.HandleAllMovement();
    }

    protected override void LateUpdate()
    {
        // ֻ���б��û����߼�����������Ʒ�ӵ���ߵĶ���
        if (!IsOwner)
            return;

        base.LateUpdate();

        PlayerCamera.instance.HandleAllCameraAction();
    }

    /// <summary>
    /// ��������ں�����������󱻴���ʱ����
    /// </summary>
    public override void OnNetworkSpawn()
    {
        base.OnNetworkDespawn();

        if (IsOwner)
        {
            PlayerCamera.instance.player = this;
            PlayerInputManager.instance.player = this;
        }
    }
}
