using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance {  get; private set; }

    [SerializeField] int amountOfBrickPerColor = 30;
    [SerializeField] int unit = 3;

    [SerializeField] BrickPool redPool;
    [SerializeField] BrickPool greenPool;
    [SerializeField] BrickPool bluePool;
    [SerializeField] BrickPool yellowPool;

    private BrickPool GetBrickPoolByType(BrickType brickType)
    {
        switch (brickType)
        {
            case BrickType.RedBrick: 
                return redPool;

            case BrickType.GreenBrick:
                return greenPool;

            case BrickType.BlueBrick:
                return bluePool;

            case BrickType.YellowBrick:
                return yellowPool;

            default:
                return null;
        }
    }

    private List<BrickObject> redBricks = new List<BrickObject>();
    private List<BrickObject> greenBricks = new List<BrickObject>();
    private List<BrickObject> blueBricks = new List<BrickObject>();
    private List<BrickObject> yellowBricks = new List<BrickObject>();

    private List<BrickObject> bricks = new List<BrickObject>();

    private bool isDonePlaceBrick;

    private bool[,] hasBrickStageOne = new bool[12, 10];
    private bool[,] hasBrickStageTwo = new bool[12, 10];
    private bool[,] hasBrickStageThree = new bool[12, 10];

    private bool[,] GetCheckArray(int stage)
    {
        switch (stage)
        {
            case 1:
                return hasBrickStageOne;

            case 2:
                return hasBrickStageTwo;

            case 3:
                return hasBrickStageThree;

            default:
                return null;
        }
    }

    [SerializeField] Vector3 startPositionStageOne;
    [SerializeField] Vector3 startPositionStageTwo;
    [SerializeField] Vector3 startPositionStageTree;

    private Vector3 GetStartPositionByStage(int stageIndex)
    {
        switch (stageIndex)
        {
            case 1: 
                return startPositionStageOne;

            case 2:
                return startPositionStageTwo;

            case 3:
                return startPositionStageTree;

            default:
                return Vector3.one * -1;    
        }
       
    }

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
        EndBridge.onAnyCharacterPass += EndBridge_onAnyCharacterPass;

        Invoke(nameof(PlaceBricks), 0.5f);
    }

    public void SetBrickStartPosition(Vector3 stageOne, Vector3 stageTwo, Vector3 stageThree)
    {
        startPositionStageOne = stageOne;
        startPositionStageTwo = stageTwo;
        startPositionStageTree = stageThree;
    }

    private void EndBridge_onAnyCharacterPass(object sender, EndBridge.OnAnyCharacterPassArgs characterPassArgs)
    {
        Debug.Log("Doi stage chay");

        ReleaseAllBrick(characterPassArgs.characterBrickType);

        SpawnBrickOnNewStage(characterPassArgs);
    }

    private void ReleaseAllBrick(BrickType brickType)
    {
        BrickPool brickPool = GetBrickPoolByType(brickType);
        for(int i = 0; i < brickPool.Count; i++)
        {
            BrickObject brick = brickPool.GetPooledObject();
            brick.Release();    
        }
    }

    private void SpawnBrickOnNewStage(EndBridge.OnAnyCharacterPassArgs e)
    {
        if (e.stageIndex >= 3) return;

        List<BrickObject> list = GetBrickList(e.characterBrickType);
        list.Clear();

        isDonePlaceBrick = false;

        BrickPool brickPool = GetBrickPoolByType(e.characterBrickType);
        Vector3 startPos = GetStartPositionByStage(e.stageIndex);
        bool[,] checkArray = GetCheckArray(e.stageIndex);

        for (int i = 0; i < amountOfBrickPerColor; i++)
        {
            BrickObject newBrick = brickPool.GetPooledObject();
            newBrick.transform.SetParent(null);

            int randomX = 0;
            int randomY = 0;

            while (checkArray[randomX, randomY])
            {
                randomX = Random.Range(0, 12);
                randomY = Random.Range(0, 10);
            }

            newBrick.transform.position = startPos + new Vector3(randomX, 0f, randomY) * 3f;
            checkArray[randomX, randomY] = true;

            list.Add(newBrick);
        }

        isDonePlaceBrick = true;
    }

    private void PlaceBricks()
    {
        for (int i = 0; i < amountOfBrickPerColor; i++)
        {
            BrickObject newBrick = redPool.GetPooledObject();
            redBricks.Add(newBrick);
            bricks.Add(newBrick);
        }

        for (int i = 0; i < amountOfBrickPerColor; i++)
        {
            BrickObject newBrick = greenPool.GetPooledObject();
            greenBricks.Add(newBrick);
            bricks.Add(newBrick);
        }

        for (int i = 0; i < amountOfBrickPerColor; i++)
        {
            BrickObject newBrick = bluePool.GetPooledObject();
            blueBricks.Add(newBrick);
            bricks.Add(newBrick);
        }

        for (int i = 0; i < amountOfBrickPerColor; i++)
        {
            BrickObject newBrick = yellowPool.GetPooledObject();
            yellowBricks.Add(newBrick);
            bricks.Add(newBrick);
        }

        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 12; x++)
            {
                int randomIndex = Random.Range(0, bricks.Count);
                bricks[randomIndex].transform.position = startPositionStageOne + new Vector3(x, 0, y) * unit;
                bricks[randomIndex] = bricks[bricks.Count - 1];
                bricks.RemoveAt(bricks.Count - 1);

                hasBrickStageOne[x,y] = true;
            }
        }

        isDonePlaceBrick = true;
    }

    public List<BrickObject> GetBrickList(BrickType brickType)
    {
        switch(brickType)
        {
            case BrickType.RedBrick:
                return GetRedBrickList();

            case BrickType.BlueBrick:
                return GetBlueBrickList();

            case BrickType.GreenBrick:
                return GetGreenBrickList();

            case BrickType.YellowBrick:
                return GetYellowBrickList();

            default:
                return null;
        }
    }

    public List<BrickObject> GetRedBrickList() => isDonePlaceBrick ? redBricks : null;
    public List<BrickObject> GetBlueBrickList() => isDonePlaceBrick ? blueBricks : null;
    public List<BrickObject> GetGreenBrickList() => isDonePlaceBrick ? greenBricks : null;
    public List<BrickObject> GetYellowBrickList() => isDonePlaceBrick ? yellowBricks : null;

}
