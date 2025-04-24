using Unity.Netcode;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager instance;

    [Header("��������")]
    [SerializeField] bool startGameAsClinet;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        // �ȼ���Ƿ�Ҫ��Ϊ�ͻ��˿�ʼ��Ϸ
        if (startGameAsClinet)
        {
            startGameAsClinet = false;
            // ���Ҫ�Կͻ��˿�ʼ������Ҫ���ж�,��Ϊ����ҳ����Ϊ������ʼ��
            NetworkManager.Singleton.Shutdown();
            // Ȼ����Ϊ�ͻ������¿�ʼ
            NetworkManager.Singleton.StartClient();
        }
    }
}
