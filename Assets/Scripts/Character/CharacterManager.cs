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
        }
        else
        {
            // ������Ǳ�����ң���ӷ�����ȡ����ֵ��ʹ��ƽ����ֵ���ƶ���ɫ�����ǵ������ӳ٣�
            transform.position = Vector3.SmoothDamp(transform.position, characterNetworkManager.networkPosition.Value,
                ref characterNetworkManager.networkPositionVelocity, characterNetworkManager.networkPositionSmoothTime);
        }
    }
}
