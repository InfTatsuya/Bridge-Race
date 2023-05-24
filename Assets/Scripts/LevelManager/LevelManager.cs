using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public List<Vector3> bridgeStartPositionStageOne;
    public List<Vector3> bridgeStartPositionStageTwo;
    public List<Vector3> bridgeStartPositionStageThree;

    public Vector3 winPos;

    public List<Level> levelScriptableObjList;

    [SerializeField] private int currentLevel = -1;

    private GameObject levelInstance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        if(currentLevel == -1)
        {
            currentLevel = 0;
            SetLevelInfo(levelScriptableObjList[currentLevel]);
        }
    }

    public void NextLevel()
    {
        currentLevel = (currentLevel + 1) % levelScriptableObjList.Count;
        SetLevelInfo(levelScriptableObjList[currentLevel]);
    }

    private void SetLevelInfo(Level level)
    {
        if(levelInstance != null)
        {
            Destroy(levelInstance.gameObject);
        }

        levelInstance = Instantiate(level.levelPrefab, Vector3.zero, Quaternion.identity);

        //bridgeStartPositionStageOne.Clear();
        //bridgeStartPositionStageTwo.Clear();
        //bridgeStartPositionStageThree.Clear();

        bridgeStartPositionStageOne = level.bridgeStartPositionStageOne;
        bridgeStartPositionStageTwo = level.bridgeStartPositionStageTwo;
        bridgeStartPositionStageThree = level.bridgeStartPositionStageThree;

        winPos = level.winPos;

        PoolManager.Instance.SetBrickStartPosition(level.startPositionStageOne, level.startPositionStageTwo, level.startPositionStageTree);
    }

    public List<Vector3> GetBridgeStartPositionList(int stageIndex)
    {
        switch(stageIndex)
        {
            case 1: 
                return bridgeStartPositionStageOne;

            case 2:
                return bridgeStartPositionStageTwo;

            case 3:
                return bridgeStartPositionStageThree;

            default:
                return null;
        }
    }
}
