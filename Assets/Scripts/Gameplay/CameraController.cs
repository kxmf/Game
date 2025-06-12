using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Настройки перемещения")]
    [SerializeField]
    private float moveSpeed = 5f;

    [Header("Настройки зума")]
    [SerializeField]
    private float zoomSpeed = 2f;

    [SerializeField]
    private float minZoom = 2f;

    [SerializeField]
    private float maxZoom = 10f;

    [Header("Ограничения перемещения")]
    [SerializeField]
    private float mapMinX;

    [SerializeField]
    private float mapMaxX;

    [SerializeField]
    private float mapMinY;

    [SerializeField]
    private float mapMaxY;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        HandleMovement();
        HandleZoom();
        EnforceBounds();
    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        var moveDirection = new Vector3(moveX, moveY, 0);
        transform.position += cam.orthographicSize * moveSpeed * Time.deltaTime * moveDirection;
    }

    private void HandleZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(scrollInput) < 0.01f)
            return;

        float newSize = cam.orthographicSize - scrollInput * zoomSpeed;
        cam.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
    }

    private void EnforceBounds()
    {
        Vector3 newPos = transform.position;

        float cameraHeight = cam.orthographicSize;
        float cameraWidth = cameraHeight * cam.aspect;

        newPos.x = Mathf.Clamp(newPos.x, mapMinX + cameraWidth, mapMaxX - cameraWidth);
        newPos.y = Mathf.Clamp(newPos.y, mapMinY + cameraHeight, mapMaxY - cameraHeight);

        transform.position = newPos;
    }
}
