using UnityEngine;

public class PlayerManager : CharacterManager
{
    PlayerLocomotionManager playerLocomotionManager;

    override protected void Awake()
    {
        base.Awake();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
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

    public override void OnNetworkSpawn()
    {
        base.OnNetworkDespawn();

        if (IsOwner)
        {
            PlayerCamera.instance.player = this;
        }
    }
}
