using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterMove : MonoBehaviour
{
    //Constants
    private CharacterController playerController;
    private Vector3 moveDirection;

    [Header("Player Settings")]
    [SerializeField] private float playerSpeed = 10f;
    [SerializeField] private float playerJumpSpeed = 0.5f;
    [SerializeField] private float playerGravity = 2f;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float castOffSet;


    //A Lambda setting an expression outside awake
    void Awake() => playerController = GetComponent<CharacterController>();
    private bool playerHasJumped => playerController.isGrounded && Input.GetKey(KeyCode.Space);

    //Vertical velocity
    private float Vspeed;

    void Update()
    {
        Ray downRay = new Ray(transform.position + transform.forward * castOffSet, Vector3.down);

        //Add Ground Mask later
        if (Physics.Raycast(downRay, out RaycastHit hit, Mathf.Infinity))
        {
            //Change direction based raycast direction in order to get the target direction.
            Vector3 inputs = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            //to fix the system clock of the movement
            Vector3 flatMovement = inputs * playerSpeed * Time.deltaTime;

            moveDirection = new Vector3(flatMovement.x, moveDirection.y, flatMovement.z);


            //Changing Player forward Direction

            if (moveDirection.magnitude > 0.01f && inputs != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(inputs);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed);
            }

            if (playerController.isGrounded)
            {
                Vspeed = 0f;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Vspeed = playerJumpSpeed;
                }
            }
            Vspeed -= playerGravity * Time.deltaTime;
            moveDirection.y = Vspeed;
            playerController.Move(moveDirection * Time.deltaTime);


        }
        Debug.DrawRay(transform.position, moveDirection * 200, Color.blue);
        Debug.Log(playerController.isGrounded);

    }


    //Gravity and Jump, most likely do raycasting downward and check if object is grounded

}
