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
}
