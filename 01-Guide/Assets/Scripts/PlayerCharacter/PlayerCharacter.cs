using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    #region  Constants
    private Vector3 moveInputs;
    private Vector3 charVelocity;
    private CharacterController charController;
    private float distanceToGround;
    private bool grounded;
    private bool isSlope;
    public bool commandMode;
    private Vector3 hitNormal;
    private GameObject commandObj;

    public PlayerCharacter_SO settings;
    #endregion

    //Health
    //Spirit Points

    [Header("Player Settings")]
    //Health
    [SerializeField]
    private int currentHP = 3;
    [SerializeField] private int maxHP = 3;


    //Attack
    [SerializeField] private int attackPoint = 1;
    public int AttackPoint { get { return attackPoint; } }

    //Spirit
    [SerializeField] private int maxSP = 1;
    [SerializeField] private int currentSP;
    public int CurrentSP
    {
        get { return currentSP; }
        set { currentSP = value; }
    }
    [SerializeField] private float spRate;

    //Movement
    [SerializeField] private float charSpeed = 3f;
    [SerializeField] private float slideFriction = 0.3f;
    [SerializeField] private float gravityScale = -9.8f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float castOffset;
    [SerializeField] private float turnSpeed = 0.2f;
    private bool isJump;

    private void Awake()
    {
        charController = GetComponent<CharacterController>();
        commandObj = this.gameObject.transform.GetChild(1).gameObject;
        settings.hp = currentHP;
        settings.maxHp = maxHP;
        settings.attackPoint = attackPoint;
        settings.playerPosition = gameObject.transform.position;
        settings.hasTeleport = false;

        //commandObj.SetActive(false);
    }

    void Start()
    {
        distanceToGround = charController.bounds.extents.y;
    }
    private void Update()
    {
        //MovementBool(commandMode);

        if (settings.hasTeleport == true)
        {
            StartCoroutine(WaitForTeleportUpdate());
            gameObject.transform.position = settings.playerPosition;

        }
        else
        {
            Movement();

            StopCoroutine(WaitForTeleportUpdate());
        }

        //Disable movement once position has been set
        //Did player teleport? if yes movement needs to wait for few seconds.
        CommandSystem();
        SpiritCharge();



        //        Debug.Log($"Spirit charge {currentSP} Spirite Rate {spRate}");
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        //Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y + distanceToGround - 2f, transform.position.z), charController.radius);
    }

    public void SpiritCharge()
    {
        if (spRate >= 3)
        {
            currentSP++;
            spRate = 0;
        }
        else if (currentSP >= maxSP)
        {
            currentSP = maxSP;
        }

        if (currentSP != maxSP)
        {
            spRate += Time.deltaTime;
        }
    }


    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitNormal = hit.normal;
    }

    #region Movement Functions
    public void Movement()
    {
        Ray downray = new Ray(transform.position + transform.forward * castOffset, Vector3.down);

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

            isSlope = (Vector3.Angle(Vector3.up, hitNormal) <= charController.slopeLimit);


            if (grounded && Input.GetKeyDown(KeyCode.Space))
            {
                charVelocity.y += Mathf.Sqrt(jumpHeight * -3f * gravityScale);
            }

            //JumpBoost from a ghostPoint


            //Gravity and Falling Velocity
            charVelocity.y += gravityScale * Time.deltaTime;

            //Add velocity from a raycast that's deducting the slope speed. try looking at okoa's script
            Vector3 movement = moveInputs * charSpeed;
            movement.y = charVelocity.y;

            // if (!isSlope && grounded)
            // {
            //     movement.x += (1f - hitNormal.y) * hitNormal.x * (-gravityScale - slideFriction);
            //     movement.z += (1f - hitNormal.y) * hitNormal.z * (-gravityScale - slideFriction);
            // }
            if (grounded && charVelocity.y < 0)
            {
                charVelocity.y = -3f;
            }
            charController.Move(movement * Time.deltaTime);
        }




    }
    #endregion


    public void CommandSystem()
    {
        if (Input.GetMouseButtonDown(0) && !commandMode)
        {
            commandMode = true;
            StartCoroutine(WaitForEnemies());
        }

        if (Input.GetMouseButtonDown(1) || currentSP == 0 || !commandMode)
        {
            commandMode = false;
            commandObj.SetActive(false);
            Time.timeScale = 1f;
            //Turn this into a delegate
            StopCoroutine(WaitForEnemies());
        }

        bool isComRangeCreated = false;

        if (commandMode == true && !isComRangeCreated)
        {
            isComRangeCreated = true;
            commandObj.SetActive(true);
        }
    }

    IEnumerator WaitForEnemies()
    {
        yield return new WaitForSeconds(0.1f);
        Time.timeScale = 0f;
    }

    IEnumerator WaitForTeleportUpdate()
    {
        yield return new WaitForSeconds(0.01f);
        charVelocity.y = 30f;
        settings.hasTeleport = false;

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            int enemyDmg = other.gameObject.GetComponent<EnemyStatus>().attack;
            //Get the damage
            currentHP -= enemyDmg;
        }
    }


    /*Create a function of a health system.
make get public void Damage (int dmg) dmg = other.enemies.attack
if value <= 0
health = 0;
else if (value >= 100)
health = 100
else
   _health = value;
*/
}
