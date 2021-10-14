using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script attached to forklift prefab, used to keep track of what node the forklift has reached in it's path.
/// </summary>
public class ForkliftInfoScript : MonoBehaviour
{
    private int currentNode = 0; // current node which the forklift has reached
    
    public int CurrentNode
    {
        get => currentNode;
        set => currentNode = value;
    }
}
