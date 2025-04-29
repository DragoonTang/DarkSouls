using Unity.Netcode;
using UnityEngine;

/// <summary>
/// 角色管理的基类
/// </summary>
public class CharacterManager : NetworkBehaviour
{
    public CharacterController characterController;
    CharacterNetworkManager characterNetworkManager;

    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);

        characterController = GetComponent<CharacterController>();
        characterNetworkManager = GetComponent<CharacterNetworkManager>();
    }

    protected virtual void Update()
    {
        if (IsOwner)
        {
            characterNetworkManager.networkPosition.Value = transform.position;
            characterNetworkManager.networkRotation.Value = transform.rotation;
        }
        else
        {
            // 如果不是本地玩家，则从服务器取得数值后使用平滑插值来移动角色（考虑到网络延迟）

            transform.SetPositionAndRotation(
                Vector3.SmoothDamp(// 位置
                        transform.position, characterNetworkManager.networkPosition.Value,
                        ref characterNetworkManager.networkPositionVelocity,
                        characterNetworkManager.networkPositionSmoothTime
                        ),
                Quaternion.Slerp( // 旋转
                        transform.rotation,
                        characterNetworkManager.networkRotation.Value,
                        characterNetworkManager.networkRotationSmoothTIme
                        )
                );
        }
    }

    protected virtual void LateUpdate()
    {
        
    }
}
