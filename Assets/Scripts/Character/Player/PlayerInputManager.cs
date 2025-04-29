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

    [Header("移动输入")]
    [SerializeField] Vector2 movemomtInput;
    public float verticalInput;
    public float horizontalInput;
    public float moveAmount;

    [Header("相机输入")]
    [SerializeField] Vector2 cameraInput;
    public float cameraVerticalInput;
    public float cameraHorizontalInput;

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
        HandleCameraInput();
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

            playerContorls.PlayerMovement.Movement.performed += i => movemomtInput = i.ReadValue<Vector2>();
            playerContorls.PlayerCamera.CamerasControls.performed += i => cameraInput = i.ReadValue<Vector2>();
        }

        playerContorls.Enable();
    }

    /// <summary>
    /// 如果游戏程序窗口失去焦点，则暂停监听
    /// </summary>
    /// <param name="focus"></param>
    private void OnApplicationFocus(bool focus)
    {
        if (enabled)
        {
            if (focus)
                playerContorls.Enable();
            else
                playerContorls.Disable();
        }
    }

    private void OnDestroy()
    {
        // 销毁时记住注销
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    private void HandleMovemontInput()
    {
        verticalInput = movemomtInput.y;
        horizontalInput = movemomtInput.x;

        // 返回绝对值
        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

        // 限制moveAmount为0或0.5或1
        if (moveAmount > 0)
            moveAmount = moveAmount > 0.5f ? 1 : 0.5f;
    }

    private void HandleCameraInput() {
        cameraVerticalInput = cameraInput.y;
        cameraHorizontalInput = cameraInput.x;
    }
}