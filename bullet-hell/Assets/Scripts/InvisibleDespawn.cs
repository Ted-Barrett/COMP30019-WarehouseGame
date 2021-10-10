using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// when the object becomes invisible, destroy it's root.
///
/// must be on an object with a renderer.
/// </summary>
public class InvisibleDespawn : MonoBehaviour
{
    private void OnBecameInvisible()
    {
        Destroy(transform.root.gameObject);
    }
}
