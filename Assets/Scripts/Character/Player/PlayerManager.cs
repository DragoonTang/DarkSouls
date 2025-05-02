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

        // 不允许控制非拥有者的对象
        if (!IsOwner)
            return;

        playerLocomotionManager.HandleAllMovement();
    }

    protected override void LateUpdate()
    {
        // 只运行本用户的逻辑，不允许控制非拥有者的对象
        if (!IsOwner)
            return;

        base.LateUpdate();

        PlayerCamera.instance.HandleAllCameraAction();
    }

    /// <summary>
    /// 这个生命期函数在网络对象被创建时调用
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
