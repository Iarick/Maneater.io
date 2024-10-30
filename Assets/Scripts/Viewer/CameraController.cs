using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float panSpeed = 20f;
    private Vector2 panLimit;

    private Vector3 lastPanPosition;
    private int panFingerId; // Touch mode only
    private bool wasDragging; // Is the user dragging? (Mouse mode only)

    void Start()
    {
        // Установка начального положения и поворота камеры
        transform.position = new Vector3(transform.position.x, 15, transform.position.z);
        transform.rotation = Quaternion.Euler(30, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    void Update()
    {
        if (Input.touchSupported && Application.platform != RuntimePlatform.WebGLPlayer)
        {
            HandleTouch();
        }
        else
        {
            HandleMouse();
        }
    }

    public void setPan(float panSpeed, Vector2 panLimit)
    {
        this.panSpeed = panSpeed;
        this.panLimit = panLimit;
    }

    void HandleTouch()
    {
        switch(Input.touchCount)
        {
            case 1: // Panning
                wasDragging = false;
                // Only start panning if we're not pinching
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    lastPanPosition = Input.GetTouch(0).position;
                    panFingerId = Input.GetTouch(0).fingerId;
                }
                else if (Input.GetTouch(0).fingerId == panFingerId && Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    PanCamera(Input.GetTouch(0).position);
                }
                break;
            default:
                break;
        }
    }

    void HandleMouse()
    {
        // On mouse down, we capture mouse position
        if (Input.GetMouseButtonDown(0))
        {
            lastPanPosition = Input.mousePosition;
            wasDragging = true;
        }

        // Only move camera if mouse button is down
        if (Input.GetMouseButton(0))
        {
            PanCamera(Input.mousePosition);
        } 
        else if (wasDragging)
        {
            wasDragging = false;
        }
    }

    void PanCamera(Vector3 newPanPosition)
    {
        // Determine how much to move the camera
        Vector3 offset = Camera.main.ScreenToViewportPoint(lastPanPosition - newPanPosition);
        Vector3 move = new Vector3(offset.x * panSpeed, 0, offset.y * panSpeed);

        // Perform the movement
        transform.Translate(move, Space.World);  

        // Ensure the camera remains within bounds.
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, -panLimit.x, panLimit.x);
        pos.z = Mathf.Clamp(transform.position.z, -panLimit.y, panLimit.y);
        transform.position = pos;

        // Cache the position
        lastPanPosition = newPanPosition;
    }
}
