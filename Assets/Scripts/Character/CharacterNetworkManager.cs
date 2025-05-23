using Unity.Netcode;
using UnityEngine;

/// <summary>
/// 用来作为中间件在网络上同步角色的状态
/// </summary>
public class CharacterNetworkManager : NetworkBehaviour
{
    [Header("位置")]
    public NetworkVariable<Vector3> networkPosition =
        new(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<Quaternion> networkRotation = new(Quaternion.identity, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public Vector3 networkPositionVelocity;
    /// <summary>
    /// 顺滑度，因为网络传输实际上是不连续的，用来抵消断续
    /// </summary>
    public float networkPositionSmoothTime = 0.1f;
    public float networkRotationSmoothTIme = 0.1f;

    [Header("动画")]
    public NetworkVariable<float> horizontalMovement = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> verticalMovement = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> moveAmount = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
}
