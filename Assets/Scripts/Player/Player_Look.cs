using UnityEngine;

public class Player_Look : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Camera camera;
    [SerializeField] private Transform playerOrientation;


    [Header("Camera Settings")]
    [SerializeField] private float cameraSensitivity;
    private float xRotation;

    void Start()
    {
        
    }


    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * cameraSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * cameraSensitivity;

        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerOrientation.Rotate(Vector3.up * mouseX);
    }
}
