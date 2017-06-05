using System.Collections;
using UnityEngine;

public class Character
{
    public float X
    {
        get { return Mathf.Lerp(currTile.X, destTile.X, movementPercentage); }
    }

    public float Y
    {
        get { return Mathf.Lerp(currTile.Y, destTile.Y, movementPercentage); }
    }

    public Tile currTile { get; protected set; }
    Tile destTile;
    float movementPercentage; // Goes from 0 to 1 as we move from currTile to destTile

    float speed = 2f; // Tiles per second


    public Character(Tile tile)
    {
        currTile = destTile = tile;
    }

    public void Update(float deltaTime)
    {
        // Are we there yet??
        if (currTile == destTile) return;
        // Total distance a -> b
        float distToTravel = Mathf.Sqrt(Mathf.Pow(currTile.X - destTile.X, 2) + Mathf.Pow(currTile.Y - destTile.Y, 2));
        // Distance for this update
        float distThisFrame = speed * deltaTime;
        // Convert to a percentage
        float percThisFrame = distThisFrame / distToTravel;
        // Add to overall percent traveled
        movementPercentage += percThisFrame;
        if (movementPercentage >= 1)
        {
            // We have reached our destination *TOOT* *TOOT*
            currTile = destTile;
            movementPercentage = 0;
        }
    }

    public void SetDestination(Tile tile)
    {
        if (currTile.IsNeighbor(tile, true) == false)
        {

            Debug.Log("Character::SetDestination -- Our destination tile isn't actually our neighbor!");
        }

        destTile = tile;
    }       
    
}
