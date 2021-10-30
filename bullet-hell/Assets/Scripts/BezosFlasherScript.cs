using UnityEngine;

public class BezosFlasherScript : MonoBehaviour {
    private Animator happyBezosAnimator;

    void Start() {
        this.happyBezosAnimator = this.transform.Find("HappyBezos").GetComponent<Animator>();
    }

    public void BoxCollected() {
        if (Random.Range(0f, 1f) < 0.1f) {
            happyBezosAnimator.SetTrigger("HappyTrigger");
        }
    }
}
