using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is a basic interface with a single required
//method.
public interface IBoxContainer
{
    List<Box> UnloadBoxes(BoxType type);
    public void Hit();
}


public enum BoxType
{
    RED, GREEN, BLUE
}