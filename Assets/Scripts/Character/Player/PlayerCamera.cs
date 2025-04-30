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
    [SerializeField] float cameraCollisionOffset = 0.2f;
    [SerializeField]
    float cameraCollisionRadius = .2f;
    [SerializeField] LayerMask collideWithLayers;

    [Header("�������")]
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
    /// ����PlayerManagerÿ֡����
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
    /// ÿ֡����
    /// </summary>
    private void HandleFollowTarget() => transform.position = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmoothTime * Time.deltaTime);

    /// <summary>
    /// ��PlayerInputManager��ȡ�û�����
    /// </summary>
    private void HandleRotation()
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

    /// <summary>
    /// ������߼�������嵵�������ǰ���������������Player��ľ���
    /// </summary>
    private void HandleCollisions()
    {
        targetZPosition = cameraZPosition;

        if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, (cameraObject.transform.position - cameraPivotTransform.position).normalized, out RaycastHit hit, Mathf.Abs(targetZPosition), collideWithLayers))
        {
            targetZPosition = cameraCollisionRadius - Vector3.Distance(cameraPivotTransform.position, hit.point);
        }

        // �������С���趨�İ뾶���򽫰뾶��Ϊ����
        if (Mathf.Abs(targetZPosition) < cameraCollisionRadius)
        {
            targetZPosition = -cameraCollisionRadius;
        }

        cameraObject.transform.localPosition = Vector3.forward * Mathf.Lerp(cameraObject.transform.localPosition.z, targetZPosition, cameraCollisionOffset);
    }
}
