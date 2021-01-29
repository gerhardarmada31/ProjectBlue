using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    private Vector3 moveInputs;
    private Vector3 charVelocity;
    private CharacterController charController;
   // private Collider charCollider;
    private float distanceToGround;
    private bool grounded;

    private bool commandMode;

    //Health
    //Spirit Points

    [Header("Player Settings")]
    [SerializeField] private float charSpeed = 3f;
    [SerializeField] private float gravityScale = -9.8f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float castOffset;
    [SerializeField] private float turnSpeed = 0.2f;
    private bool isJump;

    private void Awake()
    {

        charController = GetComponent<CharacterController>();
    }

    void Start()
    {
        //!!!!!!!charController.bounds.extents.y;
        //distanceToGround = charCollider.bounds.extents.y;
        distanceToGround = charController.bounds.extents.y;
    }



    private void Update()
    {
        //don't use charController.grounded use spherecasting instead...

        // grounded = charController.isGrounded;
        // if (grounded && charVelocity.y < 0)
        // {
        //     charVelocity.y = 0;
        // }


        //2 version of ground checker
        Ray downray = new Ray(transform.position + transform.forward * castOffset, Vector3.down);



        // if (Physics.Raycast(transform.position,-transform.up,distanceToGround+0.1f))
        // {
        //     Debug.Log("hit");
        // }


        if (Physics.Raycast(downray, out RaycastHit hit, Mathf.Infinity))
        {
            moveInputs = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveInputs = Vector3.ClampMagnitude(moveInputs, 1);



            if (charVelocity.magnitude > 0.01f && moveInputs != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveInputs);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed);
            }

            grounded = charController.isGrounded;
            //grounded = Physics.SphereCast(transform.position, charController.radius/2, -transform.up, out RaycastHit hit1, distanceToGround + 0.1f);

            if (grounded && charVelocity.y < 0)
            {
                charVelocity.y = -2f;
            }

            if (grounded && Input.GetKeyDown(KeyCode.Space))
            {
                charVelocity.y += Mathf.Sqrt(jumpHeight * -3f * gravityScale);
            }

            //Gravity and Falling Velocity
            charVelocity.y += gravityScale * Time.deltaTime;

            Vector3 movement = moveInputs * charSpeed;
            movement.y = charVelocity.y;

            charController.Move(movement * Time.deltaTime);
            //Movement 
            //charController.Move(moveInputs * charSpeed * Time.deltaTime);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
    //    Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y + distanceToGround - 2f, transform.position.z), charController.radius);
    }
}
