using UnityEngine;
using System.Collections;

public class TimedDestroyObject : MonoBehaviour {

	public float timeBeforeDestroy = 5;

	public float timeBeforeDestroyTimestamp = 0;

	// Update is called once per frame
	void Update ()
	{
		timeBeforeDestroyTimestamp += Time.fixedDeltaTime;

		if (timeBeforeDestroyTimestamp >= timeBeforeDestroy)
		{
			DestroyObject(gameObject);
		}
	}
}
