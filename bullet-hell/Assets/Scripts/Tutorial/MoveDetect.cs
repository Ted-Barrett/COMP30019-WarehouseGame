using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDetect : MonoBehaviour, TutorialObject
{
    private float moveTime;

    private const float COMPLETE_TIME = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        moveTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Math.Abs(Input.GetAxisRaw("Horizontal")) > 0.1 || Math.Abs(Input.GetAxisRaw("Vertical")) > 0.1)
        {
            moveTime += Time.deltaTime;
        }
    }

    public bool isComplete()
    {
        return moveTime > COMPLETE_TIME;
    }

    public string getHintText()
    {
        return "Use WASD or arrow keys to move!";
    }

    public GameObject getHoverPoint()
    {
        return this.gameObject;
    }
}
