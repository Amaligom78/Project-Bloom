using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class Player_Movement : NetworkBehaviour
{

    //References
    [Header("References")]
    [SerializeField] private CharacterController characterController;

    //Movement
    [Header("Player Movement")]
    [SerializeField] private float movementSpeed = 5f;

    // Gravity
    [Header("Gravity")]
    [SerializeField] private float gravity = -9.81f;
    private float verticalVelocity;


    void Start()
    {
        
    }


    void Update()
    {

        if (!IsOwner)
            return;

        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        Vector3 direction = (transform.right * xInput + transform.forward * yInput).normalized;

        if (characterController.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;
        Vector3 velocity = direction * movementSpeed;
        velocity.y = verticalVelocity;

        characterController.Move(velocity * Time.deltaTime);
    }
}
