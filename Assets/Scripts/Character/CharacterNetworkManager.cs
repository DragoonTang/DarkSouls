using Unity.Netcode;
using UnityEngine;

public class CharacterNetworkManager : NetworkBehaviour
{
    [Header("λ��")]
    public NetworkVariable<Vector3> networkPosition =
        new(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<Quaternion> networkRotation = new(Quaternion.identity, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public Vector3 networkPositionVelocity;
    /// <summary>
    /// ˳���ȣ���Ϊ���紫��ʵ�����ǲ������ģ�������������
    /// </summary>
    public float networkPositionSmoothTime = 0.1f;
    public float networkRotationSmoothTIme = 0.1f;
}
