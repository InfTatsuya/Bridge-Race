using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "SO/Level", fileName ="Level_")]
public class Level : ScriptableObject
{
    public Vector3 startPositionStageOne;
    public Vector3 startPositionStageTwo;
    public Vector3 startPositionStageTree;

    public Vector3 winPos;

    public List<Vector3> bridgeStartPositionStageOne;
    public List<Vector3> bridgeStartPositionStageTwo;
    public List<Vector3> bridgeStartPositionStageThree;
}
