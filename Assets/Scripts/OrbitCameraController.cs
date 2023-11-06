using UnityEngine;

public class OrbitCameraController : MonoBehaviour
{
    [Header("References")]
    public Transform target;       // El objeto alrededor del cual la cámara girará

    [Header("Orbit Parameters")]
    public float orbitSpeed = 90f; // Velocidad de rotación
    public Vector2 orbitDamping = new Vector2(7f, 7f); // Desaceleración de la rotación

    [Header("Zoom Parameters")]
    public float zoomSpeed = 5f;   // Velocidad de zoom
    public float minZoom = 5f;     // Zoom mínimo
    public float maxZoom = 30f;    // Zoom máximo
    public float zoomDamping = 5f; // Desaceleración del zoom

    private Vector3 currentVelocity;
    private float targetZoom;
    private Vector2 currentRotation, targetRotation;

    private void Start()
    {
        if (target == null)
        {
            Debug.LogError("Error: No se ha asignado un target a la cámara orbit.");
            return;
        }

        targetZoom = Vector3.Distance(transform.position, target.position);
        currentRotation.y = Vector3.Angle(Vector3.right, transform.right);
    }

    private void Update()
    {
        HandleMouseInput();
        RotateCamera();
        HandleZoom();
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButton(1)) // Botón derecho del ratón
        {
            targetRotation.x += Input.GetAxis("Mouse Y") * orbitSpeed;
            targetRotation.y += Input.GetAxis("Mouse X") * orbitSpeed;
        }

        targetZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
    }

    private void RotateCamera()
    {
        if (target == null) return;

        currentRotation.x = Mathf.Lerp(currentRotation.x, targetRotation.x, 1f / orbitDamping.x);
        currentRotation.y = Mathf.Lerp(currentRotation.y, targetRotation.y, 1f / orbitDamping.y);

        transform.eulerAngles = new Vector3(-currentRotation.x, currentRotation.y, 0);
        transform.position = target.position - transform.forward * targetZoom;
    }

    private void HandleZoom()
    {
        float currentDistance = Vector3.Distance(transform.position, target.position);
        float newZoom = Mathf.Lerp(currentDistance, targetZoom, Time.deltaTime * zoomDamping);

        transform.position = target.position - transform.forward * newZoom;
    }
}
