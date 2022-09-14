using UnityEngine;

// Written by Nicholas Sebastian Hendrata on 14/08/2022.

public class PlayerLook : MonoBehaviour
{
    public float mouseSensitivity = 150f;
    public float cameraSmoothing = 0.5f;
    public Vector3 cameraOffset;
    public float maxCameraAngle = 80;

    private new Transform camera;
    private float verticalRotation;

    void Awake()
    {
        camera = Camera.main.transform;
    }

    void Start()
    {
        camera.position = transform.position + cameraOffset;
    }

    void FixedUpdate()
    {
        CameraFollow();
    }

    void LateUpdate()
    {
        if (GameManagerJoseph.Main.isPlaying)
        {
            HorizontalLook();
            VerticalLook();
        }
    }

    private void CameraFollow()
    {
        var targetPosition = transform.position + cameraOffset;
        camera.position = Vector3.Lerp(camera.position, targetPosition, cameraSmoothing);
    }

    private void HorizontalLook()
    {
        float mouseX = Input.GetAxis("Mouse X");
        var horizontalAngle = mouseX * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up, horizontalAngle);
    }

    private void VerticalLook()
    {
        float mouseY = Input.GetAxis("Mouse Y");

        verticalRotation -= mouseY * mouseSensitivity * Time.deltaTime;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxCameraAngle, maxCameraAngle);

        var horizontal = new Vector3(transform.forward.x, 0, transform.forward.z);
        var vertical = Quaternion.AngleAxis(verticalRotation, transform.right);

        camera.forward = vertical * horizontal;
    }
}
