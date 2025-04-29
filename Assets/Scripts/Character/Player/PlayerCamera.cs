using UnityEngine;

/// <summary>
/// 
/// </summary>
public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;
    /// <summary>
    /// �������������Ŀ��
    /// </summary>
    public PlayerManager player;
    public Camera cameraObject;
    [SerializeField] Transform cameraPivotTransform;

    [Header("���������")]
    /// <summary>
    /// ƽ�������̶�
    /// </summary>
    float cameraSmoothTime = 1f;
    [SerializeField] float minimumPivot = -35f;
    [SerializeField] float maximumPivot = 35f;
    [SerializeField] float leftAndRightRotationSpeed = 220;
    [SerializeField] float upAndDownRotationSpeed = 200;

    [Header("�������")]
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
    /// ����PlayerManagerÿ֡����
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
    /// ÿ֡����
    /// </summary>
    public void HandleFollowTarget() => transform.position = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmoothTime * Time.deltaTime);

    /// <summary>
    /// ��PlayerInputManager��ȡ�û�����
    /// </summary>
    public void HandleRotation()
    {
        leftAndRightLookAngle += PlayerInputManager.instance.cameraHorizontalInput * Time.deltaTime * leftAndRightRotationSpeed;

        // ��ͷ������ת��Ҫ����
        upAndDownLookAngle -= PlayerInputManager.instance.cameraVerticalInput * Time.deltaTime * upAndDownRotationSpeed;
        upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

        // ��ת�����Y��
        transform.rotation = Quaternion.Euler(Vector3.up * leftAndRightLookAngle);
        // ������ͷ�����Ƶ����ֽڵ㣩
        cameraPivotTransform.localRotation = Quaternion.Euler(Vector3.right * upAndDownLookAngle);
    }
}
