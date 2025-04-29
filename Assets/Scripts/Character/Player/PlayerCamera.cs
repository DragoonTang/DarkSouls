using UnityEngine;

/// <summary>
/// 
/// </summary>
public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;
    /// <summary>
    /// 摄像机将锁定的目标
    /// </summary>
    public PlayerManager player;
    public Camera cameraObject;
    [SerializeField] Transform cameraPivotTransform;

    [Header("摄像机控制")]
    /// <summary>
    /// 平滑缓动程度
    /// </summary>
    float cameraSmoothTime = 1f;
    [SerializeField] float minimumPivot = -35f;
    [SerializeField] float maximumPivot = 35f;
    [SerializeField] float leftAndRightRotationSpeed = 220;
    [SerializeField] float upAndDownRotationSpeed = 200;

    [Header("相机参数")]
    Vector3 cameraVelocity;
    [SerializeField] float leftAndRightLookAngle;
    [SerializeField] float upAndDownLookAngle;

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

    /// <summary>
    /// 本身被PlayerManager每帧调用
    /// </summary>
    public void HandleAllCameraAction()
    {
        if (player)
        {
            HandleFollowTarget();
            HandleRotation();
        }
    }

    /// <summary>
    /// 每帧跟随
    /// </summary>
    public void HandleFollowTarget() => transform.position = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmoothTime * Time.deltaTime);

    /// <summary>
    /// 从PlayerInputManager读取用户输入
    /// </summary>
    public void HandleRotation()
    {
        leftAndRightLookAngle += PlayerInputManager.instance.cameraHorizontalInput * Time.deltaTime * leftAndRightRotationSpeed;

        // 镜头上下旋转需要限制
        upAndDownLookAngle -= PlayerInputManager.instance.cameraVerticalInput * Time.deltaTime * upAndDownRotationSpeed;
        upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

        // 旋转摄像机Y轴
        transform.rotation = Quaternion.Euler(Vector3.up * leftAndRightLookAngle);
        // 俯仰镜头（控制的是字节点）
        cameraPivotTransform.localRotation = Quaternion.Euler(Vector3.right * upAndDownLookAngle);
    }
}
