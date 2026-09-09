using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class Player_Look : NetworkBehaviour
{

    [Header("References")]
    [SerializeField] private Camera camera;
    [SerializeField] private AudioListener audioListener;
    [SerializeField] private Transform playerOrientation;
    [SerializeField] private Renderer[] renderersNotToRender;
    [SerializeField] private Transform orientateEyes;

    [Header("Camera Settings")]
    [SerializeField] private float cameraSensitivity;
    private float xRotation;

    private NetworkVariable<float> networkLookPitch =
     new NetworkVariable<float>(
         0f,
         NetworkVariableReadPermission.Everyone,
         NetworkVariableWritePermission.Owner
     );

    public override void OnNetworkSpawn()
    {
        camera.enabled = IsOwner;
        audioListener.enabled = IsOwner;

        foreach (Renderer renderer in renderersNotToRender)
        {
            renderer.enabled = !IsOwner;
        }
    }

    void Start()
    {
        
    }


    void Update()
    {
        if (IsOwner)
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * cameraSensitivity;
            float mouseY = Input.GetAxisRaw("Mouse Y") * cameraSensitivity;

            xRotation -= mouseY;

            xRotation = Mathf.Clamp(xRotation, -45f, 45f);
            camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerOrientation.Rotate(Vector3.up * mouseX);

            networkLookPitch.Value = xRotation;
        }

        orientateEyes.localRotation = Quaternion.Euler(networkLookPitch.Value, 0f, 0f);
    }
}
