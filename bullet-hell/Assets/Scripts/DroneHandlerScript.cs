using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneHandlerScript : MonoBehaviour {
    [SerializeField] private float speed;

    // The tolerance for how close the drone can be to the target before it drops the crate
    [SerializeField] private float delta;

    public Vector3 target;
    public Vector3 exitTarget;

    private bool crateDropped = false;

    [SerializeField] private float crateDropDuration;
    private float crateDropTime;

    // Magic number that defines where the crate should be spawned
    private Vector3 crateOffset = new Vector3(0, -0.161f, 0);

    [SerializeField] private GameObject[] crates;

    void Start() {
        // Attach a crate to the drone
        int crateIndex = Random.Range(0, crates.Length);
        GameObject crate = crates[crateIndex];
        GameObject newCrate = Instantiate(crate, this.transform.position + crateOffset, Quaternion.identity, this.transform.GetChild(0));
        newCrate.transform.SetAsFirstSibling();

        Rigidbody newRigidbody = newCrate.AddComponent<Rigidbody>();
        newRigidbody.useGravity = false;
        newRigidbody.isKinematic = true;
        newRigidbody.detectCollisions = true;
    }

    void Update() {
        Vector3 toTarget = target - this.transform.position;

        if (crateDropped && Time.time - crateDropTime < crateDropDuration) {
            // If we're waiting out the crate drop duration, do nothing...
            return;
        }

        if (!crateDropped && toTarget.magnitude < delta) {
            // Drop the crate halfway through the drop duration
            StartCoroutine(DropCrate(crateDropDuration / 2));
            crateDropTime = Time.time;
            crateDropped = true;
        } else if (!crateDropped) {
            // Move towards the target
            Vector3 direction = toTarget.normalized;
            this.transform.position += direction * speed * Time.deltaTime;
        } else {
            if ((exitTarget - this.transform.position).magnitude < delta) {
                Destroy(gameObject);
            }

            // Move towards the exit area
            Vector3 toExitTarget = exitTarget - this.transform.position;
            Vector3 direction = toExitTarget.normalized;
            this.transform.position += direction * speed * Time.deltaTime;
        }
    }

    IEnumerator DropCrate(float seconds) {
        yield return new WaitForSeconds(seconds);

        Transform crate = this.transform.GetChild(0).transform.GetChild(0);
        Rigidbody rigidBody = crate.GetComponent<Rigidbody>();
        rigidBody.useGravity = true;
        rigidBody.isKinematic = false;
        rigidBody.detectCollisions = false;

        // Have to do this weird logic to fix a bug with the crate falling
        StartCoroutine(EnableCrateCollisions(crate, rigidBody));
    }

    // Wait for a short duration, then enable collisions.
    IEnumerator EnableCrateCollisions(Transform crate, Rigidbody rigidBody) {
        yield return new WaitForSeconds(0.2f);

        rigidBody.detectCollisions = true;
        crate.parent = GameObject.Find("Crates").transform;
    }
}
