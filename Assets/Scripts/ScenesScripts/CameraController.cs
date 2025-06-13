using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController2D : MonoBehaviour
{
    [Header("Speed Settings")]
    public float panSpeed = 20f;
    public float dragSpeed = 1f;

    [Header("Zoom Settings")]
    public float scrollSpeed = 5f;

    [Header("Zoom Limits")]
    public float minZoom = 3f;
    public float maxZoom = 15f;

    private Camera cam;
    private FieldBounds fieldBounds;

    private float targetZoom;

    private bool isDragging = false;
    private Vector3 dragOrigin;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Awake()
    {
        cam = GetComponent<Camera>();
        fieldBounds = FindFirstObjectByType<FieldBounds>();
        targetZoom = cam.orthographicSize;
    }

    private void Update()
    {
        HandleKeyboardPan();
        HandleMouseDrag();
        HandleZoom();
        ClampPosition();
    }

    private void HandleKeyboardPan()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(h, v, 0f) * panSpeed * Time.deltaTime;
        transform.Translate(move, Space.World);
    }

    private void HandleMouseDrag()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isDragging = true;
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(1))
            isDragging = false;

        if (isDragging)
        {
            Vector3 currentPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 diff = dragOrigin - currentPoint;
            transform.position += diff * dragSpeed;
        }
    }

    private void HandleZoom()
    {
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scrollDelta) > Mathf.Epsilon)
        {
            targetZoom -= scrollDelta * scrollSpeed;
            targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
            float currentSize = cam.orthographicSize;
            float maxDelta = scrollSpeed * 10f * Time.deltaTime;
            cam.orthographicSize = Mathf.MoveTowards(currentSize, targetZoom, maxDelta);
        }
    }

    private void ClampPosition()
    {
        if (fieldBounds == null)
            return;

        Rect b = fieldBounds.GetBounds();
        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        float minX = b.xMin + camWidth;
        float maxX = b.xMax - camWidth;
        float minY = b.yMin + camHeight;
        float maxY = b.yMax - camHeight;

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }
}
