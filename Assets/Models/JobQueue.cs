﻿using System.Collections.Generic;
using UnityEngine;
using System;

public class JobQueue {

    Queue<Job> jobQueue;

    Action<Job> cbJobCreated;

    public JobQueue()
    {
        jobQueue = new Queue<Job>();
    }

    public void Enqueue(Job j)
    {
        jobQueue.Enqueue(j);

        //TODO: Call callbacks

        if (cbJobCreated != null)
        {
            cbJobCreated(j);
        }
    }

    public void RegisterJobCreationCallback(Action<Job> cb)
    {
        cbJobCreated += cb;
    }

    public void UnregisterJobCreationCallback(Action<Job> cb)
    {
        cbJobCreated -= cb;
    }
 
}
