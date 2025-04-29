using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;

    /// <summary>
    /// ��Ҫ��Input Actionת��ΪC#��
    /// </summary>
    PlayerControls playerContorls;

    [Header("�ƶ�����")]
    [SerializeField] Vector2 movemomtInput;
    public float verticalInput;
    public float horizontalInput;
    public float moveAmount;

    [Header("�������")]
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
        // ÿ�λ�����ʱ��Ҫִ��һ��
        SceneManager.activeSceneChanged += OnSceneChange;
        instance.enabled = false;
    }

    private void Update()
    {
        HandleMovemontInput();
        HandleCameraInput();
    }

    /// <summary>
    /// ��������Ϸ����ʱ��������Ҳ���
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
    /// �����Ϸ���򴰿�ʧȥ���㣬����ͣ����
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
        // ����ʱ��סע��
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    private void HandleMovemontInput()
    {
        verticalInput = movemomtInput.y;
        horizontalInput = movemomtInput.x;

        // ���ؾ���ֵ
        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

        // ����moveAmountΪ0��0.5��1
        if (moveAmount > 0)
            moveAmount = moveAmount > 0.5f ? 1 : 0.5f;
    }

    private void HandleCameraInput() {
        cameraVerticalInput = cameraInput.y;
        cameraHorizontalInput = cameraInput.x;
    }
}