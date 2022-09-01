using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockManMove : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float walkSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float runSpeed;

    private Vector3 moveDirection;
    private Vector3 velocity;
    private CharacterController controller;
    private Animator animator;
    [SerializeField] private Transform rockModel;

    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpHeight;



    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        onMove();
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(Attack());   
        }
        
    }

    private void onMove()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);
        float moveZ = Input.GetAxis("Vertical");
        Debug.Log(moveZ);

        if (moveZ < 0.0f) rockModel.localScale = new Vector3(0.5f, 0.5f, -0.5f);
        else if (moveZ > 0.0f) rockModel.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        moveDirection = new Vector3(0, 0, moveZ);
        moveDirection = transform.TransformDirection(moveDirection); // para modificar el axis en modo local


        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;

        }

        if (isGrounded)
        {


            if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            {
                Walk();
            }
            else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
            {
                Run();
            }
            else if (moveDirection == Vector3.zero)
            {
                Idle();
            }
            moveDirection *= moveSpeed;

            if (Input.GetKey(KeyCode.Space))
            {
                Jump();
            }

            if (Input.GetKey(KeyCode.LeftControl))
            {
                Victory();
            }

        }
        //animator.SetBool("Victory", false);

        //controller.Move(moveDirection * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        moveDirection.y = velocity.y;
        controller.Move(moveDirection * Time.deltaTime);
    }

    private void Idle()
    {
        moveSpeed = 0;
        animator.SetFloat("Speed_Blend", 0, 0.1f, Time.deltaTime);
        animator.SetBool("Victory", false);
    }

    private void Walk()
    {
        moveSpeed = walkSpeed;
        animator.SetFloat("Speed_Blend", 0.5f, 0.1f, Time.deltaTime);
    }

    private void Run()
    {
        moveSpeed = runSpeed;
        animator.SetFloat("Speed_Blend", 1, 0.1f, Time.deltaTime);
    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        animator.SetFloat("Speed_Blend", 0, 0.01f, Time.deltaTime);
    }
    private void Victory()
    {
        animator.SetBool("Victory", true);
        //moveSpeed = 0;
    }


    private IEnumerator Attack()
    {
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.9f);
    }
}
