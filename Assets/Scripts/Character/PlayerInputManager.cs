using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;

    /// <summary>
    /// 需要把Input Action转换为C#类
    /// </summary>
    PlayerControls playerContorls;

    [SerializeField] Vector2 movemomt;
    public float verticalInput;
    public float horizontalInput;
    public float moveAmount;

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
        // 每次换场景时需要执行一次
        SceneManager.activeSceneChanged += OnSceneChange;
        instance.enabled = false;
    }

    private void Update()
    {
        HandleMovemontInput();
    }

    /// <summary>
    /// 当不在游戏场景时，禁用玩家操作
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="newScene"></param>
    private void OnSceneChange(Scene arg0, Scene newScene)
    {
        instance.enabled = WorldSaveGameManager.instance.GetWorldSceneIndex() == newScene.buildIndex;
    }

    private void OnEnable()
    {
        if (playerContorls == null)
        {
            playerContorls = new PlayerControls();

            playerContorls.PlayerMovement.Movement.performed += i => movemomt = i.ReadValue<Vector2>();
        }

        playerContorls.Enable();
    }

    private void OnDestroy()
    {
        // 销毁时记住注销
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    private void HandleMovemontInput()
    {
        verticalInput = movemomt.y;
        horizontalInput = movemomt.x;

        // 返回绝对值
        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

        // 限制moveAmount为0或0.5或1
        if (moveAmount > 0)
            moveAmount = moveAmount > 0.5f ? 1 : 0.5f;
    }
}