using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[SelectionBase]
public class BridgePart : MonoBehaviour
{
    [SerializeField] MeshRenderer meshRenderer;
    //[SerializeField] Material[] materials;
    [SerializeField] ColorData colorData;
    [SerializeField] Vector3 offset = new Vector3(0f, 0.2f, 0.5f);
    [SerializeField] int index;

    public BrickType brickType = BrickType.NoColor;
    public bool isOn;

    private void Start()
    {
        meshRenderer.enabled = false;

        index = Mathf.RoundToInt(transform.position.y / 0.2f);

        //just for debug, TODO: remove before build
        if (isOn)
        {
            meshRenderer.enabled = true;
            SetupBridgePart(brickType);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Player>(out var player))
        {
            if (player.CanBuildBridge())
            {
                if (!isOn)
                {
                    isOn = true;
                    brickType = player.CharacterBrickType;
                    meshRenderer.enabled = true;
                    SetupBridgePart(brickType);

                    player.BuildBridge();
                }
                else if(brickType != player.CharacterBrickType)
                {
                    player.BuildBridge();

                    brickType = player.CharacterBrickType;
                    SetupBridgePart(brickType);
                }

                float yPos = index * 0.2f;
                player.SetPlayerHeight(yPos);
            }
            else if(brickType != player.CharacterBrickType)
            {
                player.CanMoveForward = false;
            }
        }

        if(other.TryGetComponent<Enemy>(out var enemy))
        {
            if (enemy.CanBuildBridge())
            {
                if (!isOn)
                {
                    isOn = true;
                    brickType = enemy.CharacterBrickType;
                    meshRenderer.enabled = true;
                    SetupBridgePart(brickType);

                    enemy.BuildBridge();

                    enemy.MoveTo(transform.position + offset);
                }
                else if (brickType != enemy.CharacterBrickType)
                {
                    brickType = enemy.CharacterBrickType;
                    SetupBridgePart(brickType);

                    enemy.BuildBridge();
                    enemy.MoveTo(transform.position + offset);
                }
                else if(brickType == enemy.CharacterBrickType)
                {
                    enemy.MoveTo(transform.position + offset);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Player>(out var player))
        {
            player.CanMoveForward = true;
            player.SetPlayerHeight(index * 0.2f);

            Debug.Log("Ha do cao chay");
        }
    }

    private void SetupBridgePart(BrickType brickType)
    {
        meshRenderer.material = colorData.GetMaterial(brickType);
    }
}
