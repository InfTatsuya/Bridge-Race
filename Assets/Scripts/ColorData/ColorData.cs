using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Color Data", fileName ="ColorData_")]
public class ColorData : ScriptableObject
{
    //public BrickType brickType;

    public List<Material> materials;

    public Material GetMaterial(BrickType brickType)
    {
        if((int)brickType >= 4)
        {
            return materials[4];
        }
        return materials[(int)brickType];
    }
}

public enum BrickType
{
    RedBrick,
    GreenBrick,
    BlueBrick,
    YellowBrick,
    UndefinedBrick,
    NoColor
}
