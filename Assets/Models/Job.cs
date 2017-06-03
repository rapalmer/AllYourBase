﻿using UnityEngine;
using System.Collections.Generic;
using System;

public class Job {

	// This class holds info for a queued up job, which can include
	// things like placing furniture, moving stored inventory,
	// working at a desk, and maybe even fighting enemies.

	public Tile tile { get; protected set; }
	float jobTime;

	Action<Job> cbJobComplete;
	Action<Job> cbJobCancel;

	public Job ( Tile tile, Action<Job> cbJobComplete, float jobTime = 1f ) {
		this.tile = tile;
		this.cbJobComplete += cbJobComplete;
	}

	public void RegisterJobCompleteCallback(Action<Job> cb) {
		cbJobComplete += cb;
	}

	public void RegisterJobCancelCallback(Action<Job> cb) {
		cbJobCancel += cb;
	}

	public void DoWork(float workTime) {
		jobTime -= workTime;

		if(jobTime <= 0) {
			if(cbJobComplete != null)
				cbJobComplete(this);
		}
	}

	public void CancelJob() {
		if(cbJobCancel != null)
			cbJobCancel(this);		
	}
}
