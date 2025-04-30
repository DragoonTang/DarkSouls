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
    [SerializeField] float cameraCollisionOffset = 0.2f;
    [SerializeField]
    float cameraCollisionRadius = .2f;
    [SerializeField] LayerMask collideWithLayers;

    [Header("相机参数")]
    Vector3 cameraVelocity;
    //Vector3 cameraObjectPosition;
    [SerializeField] float leftAndRightLookAngle;
    [SerializeField] float upAndDownLookAngle;
    float cameraZPosition;
    float targetZPosition;

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
        cameraZPosition = cameraObject.transform.localPosition.z;
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
            HandleCollisions();
        }
    }

    /// <summary>
    /// 每帧跟随
    /// </summary>
    private void HandleFollowTarget() => transform.position = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmoothTime * Time.deltaTime);

    /// <summary>
    /// 从PlayerInputManager读取用户输入
    /// </summary>
    private void HandleRotation()
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

    /// <summary>
    /// 如果射线检测有物体档在摄像机前，则缩短摄像机和Player间的距离
    /// </summary>
    private void HandleCollisions()
    {
        targetZPosition = cameraZPosition;

        if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, (cameraObject.transform.position - cameraPivotTransform.position).normalized, out RaycastHit hit, Mathf.Abs(targetZPosition), collideWithLayers))
        {
            targetZPosition = cameraCollisionRadius - Vector3.Distance(cameraPivotTransform.position, hit.point);
        }

        // 如果距离小于设定的半径，则将半径作为距离
        if (Mathf.Abs(targetZPosition) < cameraCollisionRadius)
        {
            targetZPosition = -cameraCollisionRadius;
        }

        cameraObject.transform.localPosition = Vector3.forward * Mathf.Lerp(cameraObject.transform.localPosition.z, targetZPosition, cameraCollisionOffset);
    }
}
