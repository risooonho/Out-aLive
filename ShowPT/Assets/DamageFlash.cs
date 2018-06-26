﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageFlash : MonoBehaviour {

	[SerializeField]
	Color damageColor;

	[SerializeField]
	float flashSpeed = 0.1f;

	Image redOverlay;

	bool damaged = false;

	// Use this for initialization
	void Start () {
		redOverlay = GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (damaged) 
		{
			redOverlay.color = Color.Lerp (redOverlay.color, Color.clear, flashSpeed * Time.deltaTime);

			if (redOverlay.color.a <= 0) 
			{
				damaged = false;
			}
		}
	}

	public void damageFlash()
	{
		redOverlay.color = damageColor;
		damaged = true;
	}
}
