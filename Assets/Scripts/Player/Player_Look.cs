using System.Globalization;
using Unity.Netcode;
using Unity.VisualScripting;
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

    [Header("Interaction Settings")]
    [SerializeField] private float interactDistance;
    public Transform holdYPoint;
    private I_Interactable heldInteractable;
    private I_Interactable currentInteractable;



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

            HandleDetection();
        }

        orientateEyes.localRotation = Quaternion.Euler(networkLookPitch.Value, 0f, 0f);
    }

    private void HandleDetection()
    {
        if (heldInteractable != null)
        {
            if (Input.GetKeyDown(Input_Manager.instance.dropKey))
            {
                heldInteractable.Remove();
                heldInteractable = null;
            }

            return;
        }

        Detection();

        if (currentInteractable != null && Input.GetKeyDown(Input_Manager.instance.interactKey))
        {
            currentInteractable.Interact();
            heldInteractable = currentInteractable;
            currentInteractable = null;
            UI_Manager.instance.hud.DisableDetect();
        }
    }

    private void Detection()
    {
        Vector3 origin = camera.transform.position;
        Vector3 direction = camera.transform.forward;
        
        Debug.DrawRay(origin, direction * interactDistance, Color.red);

        if (Physics.Raycast(origin, direction, out RaycastHit hit, interactDistance))
        {
            I_Interactable interactable = hit.collider.GetComponentInParent<I_Interactable>();

            if (interactable != null)
            {
                if(currentInteractable != interactable)
                {
                    currentInteractable = interactable;
                    currentInteractable.Detect();
                }

                return;
            }
        }

        currentInteractable = null;
        UI_Manager.instance.hud.DisableDetect();
    }
}
