using Unity.Netcode;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager instance;

    [Header("加入网络")]
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
        // 先检查是否要作为客户端开始游戏
        if (startGameAsClinet)
        {
            startGameAsClinet = false;
            // 如果要以客户端开始，必须要先中断,因为在首页是作为主机开始的
            NetworkManager.Singleton.Shutdown();
            // 然后作为客户端重新开始
            NetworkManager.Singleton.StartClient();
        }
    }
}
