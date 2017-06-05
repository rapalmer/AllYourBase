using System.Collections;
using UnityEngine;
using System;

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

    Action<Character> cbCharacterChanged;

    Job myJob;


    public Character(Tile tile)
    {
        currTile = destTile = tile;
    }

    public void Update(float deltaTime)
    {

        // Do I have a job?
        if (myJob == null)
        {
            myJob = currTile.world.jobQueue.Dequeue();
            if (myJob != null)
            {
                destTile = myJob.tile;
                myJob.RegisterJobCancelCallback(OnJobEnded);
                myJob.RegisterJobCompleteCallback(OnJobEnded);
            }
        }

        // Are we there yet??
        if (currTile == destTile)
        {
            if (myJob != null)
            {
                myJob.DoWork(deltaTime);
            }

            return;
        }
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
        if (cbCharacterChanged != null)
        {
            cbCharacterChanged(this);
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

    public void RegisterOnChangedCallback(Action<Character> cb)
    {
        cbCharacterChanged += cb;
    }

    public void UnregisterOnChangedCallback(Action<Character> cb)
    {
        cbCharacterChanged -= cb;
    }

    void OnJobEnded(Job j)
    {
        if (j != myJob)
        {
            Debug.LogError("Character being told about job that is not his.");
            return;
        }
        myJob = null;
    }
}
