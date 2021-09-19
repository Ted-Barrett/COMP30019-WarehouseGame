using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    private Animator animator;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float rotationSpeed;

    private float inputV;
    private float inputH;
    private bool isCarrying;
    private bool hasTrolly;

    private float timeIdle;

    private CharacterController controller;

    private Vector3 facing = Vector3.zero;

    void Start() {
        animator = gameObject.GetComponent<Animator>();
        controller = gameObject.GetComponent<CharacterController>();
    }

    void Update() {
        // We use GetAxisRaw here, because GetAxis returns a smoothed value that goes between no key press and full key press kind of slowly.
        inputV = Input.GetAxis("Vertical");
        inputH = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown("c"))
        {
            isCarrying = !isCarrying;
            if (isCarrying)
            {
                hasTrolly = false;
            }
        }

        if (Input.GetKeyDown("t"))
        {
            hasTrolly = !hasTrolly;
            if (hasTrolly)
            {
                isCarrying = false;
            }
        }

        if (isCarrying || hasTrolly)
        {
            inputV = Mathf.Max(inputV, 0);
        }

        if (inputV == 0 && inputH == 0)
        {
            timeIdle += Time.deltaTime;
        } else
        {
            timeIdle = 0;
        }

        Vector3 inputMovement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        if (Mathf.Abs(inputMovement.magnitude) >= 0.001)
        {
            facing = inputMovement;
        }
        
        controller.Move(inputMovement * Time.deltaTime * speed);

        Quaternion rotation = Quaternion.LookRotation(facing);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.05f);


        animator.SetFloat("inputV", inputV);
        animator.SetFloat("inputH", inputH);
        animator.SetBool("isMoving", inputMovement.magnitude != 0);
        animator.SetBool("isCarrying", isCarrying);
        animator.SetBool("hasTrolly", hasTrolly);
        animator.SetFloat("timeIdle", timeIdle);

        if (Input.GetKeyDown("h"))
        {
            animator.SetTrigger("isHit");
        }
        
    }
}
