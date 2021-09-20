using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
    [SerializeField]
    private float speed;

    private CharacterController controller;

    private float vSpeed = 0.0f;

    [SerializeField] private float jumpSpeed = 4;
    
    void Start() {
        controller = gameObject.GetComponent<CharacterController>();
    }

    void Update() {
        // We use GetAxisRaw here, becuase GetAxis returns a smoothed value that goes between no key press and full key press kind of slowly.
        Vector3 inputMovement = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
        if (inputMovement.sqrMagnitude > 1.0)
        {
            inputMovement.Normalize();
        }
        
        vSpeed -= Physics.gravity.magnitude*Time.deltaTime;
        
        if (controller.isGrounded)
        {
            vSpeed = Input.GetAxisRaw("Jump") * jumpSpeed; // if we want to disable jumping, do vSpeed = 0; instead.
        }

        controller.Move(inputMovement * Time.deltaTime * speed);
        controller.Move(new Vector3(0, vSpeed*Time.deltaTime, 0));
    }
}
