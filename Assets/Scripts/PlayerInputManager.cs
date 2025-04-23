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
        // ÿ�λ�����ʱ��Ҫִ��һ��
        SceneManager.activeSceneChanged += OnSceneChange;
    }

    private void Start()
    {
        instance.enabled = false;
        DontDestroyOnLoad(gameObject);
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
}