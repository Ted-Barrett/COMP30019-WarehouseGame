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
        Vector3 inputMovement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(inputMovement * Time.deltaTime * speed);
    }
}
