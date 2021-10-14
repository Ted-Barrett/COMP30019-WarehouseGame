using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script attached to forklift paths, describing what type of path they are.
/// </summary>
public class PathInfo : MonoBehaviour
{
    public enum PathTypeEnum
    {
        Straight,
        Corner
    }

    [SerializeField] private PathTypeEnum pathType;

    public float radius = 3.0f; // corner radius (not used for straights)
    public PathTypeEnum GETType()
    {
        return pathType;
    }
}
