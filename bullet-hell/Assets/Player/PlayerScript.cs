using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    private Animator animator;

    [SerializeField]
    private float runSpeed;

    [SerializeField]
    private float walkSpeed;

    private float inputV;
    private float inputH;
    private bool isCarrying;
    private bool hasTrolly;

    private float timeIdle;

    private CharacterController controller;
    private GrabScript grabScript;

    [SerializeField]
    private CameraScript cameraScript;

    private Vector3 facing = Vector3.zero;

    void Start() {
        animator = gameObject.GetComponent<Animator>();
        controller = gameObject.GetComponent<CharacterController>();
        grabScript = gameObject.GetComponent<GrabScript>();
    }

    void Update() {
        // We use GetAxisRaw here, because GetAxis returns a smoothed value that goes between no key press and full key press kind of slowly.
        inputV = Input.GetAxis("Vertical");
        inputH = Input.GetAxis("Horizontal");

        if (grabScript.PickedItem != null && grabScript.PickedItem.GetComponent<Box>() != null)
        {
            isCarrying = true;
        } else 
        {
            isCarrying = false;
        }

        if (grabScript.PickedItem != null && grabScript.PickedItem.GetComponent<HandTruck>() != null)
        {
            hasTrolly = true;
        }
        else
        {
            hasTrolly = false;
        }

        if (inputV == 0 && inputH == 0)
        {
            timeIdle += Time.deltaTime;
        } else
        {
            timeIdle = 0;
        }

        Vector3 inputMovement = Vector3.zero;
        if (!IsHit)
        {
            inputMovement = Vector3.ClampMagnitude(new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")), 1.0f);

            if (inputMovement.magnitude >= 0.001f)
            {
                facing = inputMovement;
                bool isRunning = !isCarrying && !hasTrolly;
                float speed = isRunning ? runSpeed : walkSpeed;
                controller.Move(inputMovement * Time.deltaTime * speed);

                Quaternion rotation = Quaternion.LookRotation(facing);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.5f);
            }
        }
        
        animator.SetBool("isMoving", inputMovement.magnitude != 0);
        animator.SetBool("isCarrying", isCarrying);
        animator.SetBool("hasTrolly", hasTrolly);
        animator.SetFloat("timeIdle", timeIdle);

        if (Input.GetKeyDown("h"))
        {
            Hit(Vector3.zero);
        }
    }

    public void Hit(Vector3 direction)
    {
        if (!IsHit)
        {
            cameraScript.Shake();
            grabScript.Drop();
            animator.SetTrigger("isHit");
        }
    }

    private bool IsHit
    {
        get => animator.GetCurrentAnimatorStateInfo(0).IsTag("hit");
    }

}
