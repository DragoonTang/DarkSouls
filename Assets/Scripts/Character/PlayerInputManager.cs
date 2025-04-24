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
        // ÿ�λ�����ʱ��Ҫִ��һ��
        SceneManager.activeSceneChanged += OnSceneChange;
        instance.enabled = false;
    }

    private void Update()
    {
        HandleMovemontInput();
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

            playerContorls.PlayerMovement.Movement.performed += i => movemomt = i.ReadValue<Vector2>();
        }

        playerContorls.Enable();
    }

    private void OnDestroy()
    {
        // ����ʱ��סע��
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    private void HandleMovemontInput()
    {
        verticalInput = movemomt.y;
        horizontalInput = movemomt.x;

        // ���ؾ���ֵ
        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

        // ����moveAmountΪ0��0.5��1
        if (moveAmount > 0)
            moveAmount = moveAmount > 0.5f ? 1 : 0.5f;
    }
}