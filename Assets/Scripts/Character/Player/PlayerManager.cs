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

        // 不允许控制非拥有者的对象
        if (!IsOwner)
            return;

        playerLocomotionManager.HandleAllMovement();
    }
}
