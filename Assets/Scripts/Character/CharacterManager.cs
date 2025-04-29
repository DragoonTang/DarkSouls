using Unity.Netcode;
using UnityEngine;

/// <summary>
/// ��ɫ����Ļ���
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
            // ������Ǳ�����ң���ӷ�����ȡ����ֵ��ʹ��ƽ����ֵ���ƶ���ɫ�����ǵ������ӳ٣�

            transform.SetPositionAndRotation(
                Vector3.SmoothDamp(// λ��
                        transform.position, characterNetworkManager.networkPosition.Value,
                        ref characterNetworkManager.networkPositionVelocity,
                        characterNetworkManager.networkPositionSmoothTime
                        ),
                Quaternion.Slerp( // ��ת
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
