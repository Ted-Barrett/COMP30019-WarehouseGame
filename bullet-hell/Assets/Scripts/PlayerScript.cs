using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
    [SerializeField]
    private float speed;

    private CharacterController controller;

    void Start() {
        controller = gameObject.GetComponent<CharacterController>();
    }

    void Update() {
        // We use GetAxisRaw here, becuase GetAxis returns a smoothed value that goes between no key press and full key press kind of slowly.
        Vector3 inputMovement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        controller.Move(inputMovement * Time.deltaTime * speed);
    }
}
