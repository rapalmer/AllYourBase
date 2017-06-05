using UnityEngine;
using System.Collections.Generic;

public class JobSpriteController : MonoBehaviour
{

    FurnitureSpriteController fsc;
    Dictionary<Job, GameObject> jobGameObjectMap;

	void Start ()
	{
	    jobGameObjectMap = new Dictionary<Job, GameObject>();
	    fsc = GameObject.FindObjectOfType<FurnitureSpriteController>();

        // FIXME: No Such thing as a jobqueue
	    WorldController.Instance.world.jobQueue.RegisterJobCreationCallback(OnJobCreated);
	}

    void OnJobCreated(Job j)
    {
        // FIXME: We can only do furniture-building jobs
        GameObject job_go = new GameObject();

        // Add our tile/GO pair to the dictionary.
        jobGameObjectMap.Add(j, job_go);

        job_go.name = "JOB_" + j.jobObjectType + "_" + j.tile.X + "_" + j.tile.Y;
        job_go.transform.position = new Vector3(j.tile.X, j.tile.Y, 0);
        job_go.transform.SetParent(this.transform, true);

        SpriteRenderer sr = job_go.AddComponent<SpriteRenderer>();
        sr.sprite = fsc.GetSpriteForFurniture(j.jobObjectType);
        sr.color = new Color(0.5f, 1f, 0.5f, 0.3f);
        sr.sortingLayerName = "Jobs";

        j.RegisterJobCompleteCallback(OnJobEnded);
        j.RegisterJobCancelCallback(OnJobEnded);
    }

    void OnJobEnded(Job j)
    {
        // Executes whether completed or cancelled
        // FIXME: We can only do furniture-building jobs

        GameObject job_go = jobGameObjectMap[j];
        j.UnregisterJobCancelCallback(OnJobEnded);
        j.UnregisterJobCompleteCallback(OnJobEnded);
        Destroy(job_go);
    }

}
