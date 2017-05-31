﻿using UnityEngine;

public class AutoVertSize : MonoBehaviour {

    public float childHeight = 35f;

	// Use this for initialization
	void Start () {
		AdjustSize();
	}

    public void AdjustSize()
    {
        var size = this.GetComponent<RectTransform>().sizeDelta;
        size.y = this.transform.childCount * childHeight;
        this.GetComponent<RectTransform>().sizeDelta = size;
    }
}
