using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{

    private Rigidbody rigidBody;

    private bool givenScore = false;

    [SerializeField]
    private GameObject onDestroy;

    [SerializeField]
    private BoxType type;

    public BoxType Type { get => type;  }

    public Rigidbody RigidBody { get => rigidBody; }

    public void giveScore()
    {
        givenScore = true;
    }

    public bool GivenScore => givenScore;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Explode()
    {
        GameObject particle = Instantiate(onDestroy, transform.position, Quaternion.identity);
        
    }
}
