using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bound : MonoBehaviour
{
    public enum Direction
    {
        Forward,
        Backward,
        Left,
        Right,
        None
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Player>(out var player))
        {
            ConstrainPlayerMove(CalculateRelativePosition(player), player, false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Player>(out var player))
        {
            ConstrainPlayerMove(CalculateRelativePosition(player), player, true);
        }
    }

    private void ConstrainPlayerMove(Direction direction, Player player ,bool canMove)
    {
        switch(direction)
        {
            case Direction.Forward:
                player.CanMoveForward = canMove; 
                break;

            case Direction.Backward:
                player.CanMoveBackward = canMove;
                break;

            case Direction.Left:
                player.CanMoveLeft = canMove;
                break;

            case Direction.Right:
                player.CanMoveRight = canMove;
                break;

            default:
                break;
        }
    }

    private Direction CalculateRelativePosition(Player player)
    {
        if(transform.rotation == Quaternion.identity)
        {
            if (player.transform.position.x > transform.position.x)
            {
                return Direction.Left;
            }
            else if (player.transform.position.x <= transform.position.x)
            {
                return Direction.Right;
            }
        }
        else
        {
            if (player.transform.position.z > transform.position.z)
            {
                return Direction.Backward;
            }
            else if (player.transform.position.z <= transform.position.z)
            {
                return Direction.Forward;
            }
        }

        return Direction.None;
    } 
}
