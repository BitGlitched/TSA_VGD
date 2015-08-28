using UnityEngine;
using System.Collections;

public class PixelPerfectOrthographicCamera : MonoBehaviour
{
	public float pixelsPerUnit = 32;

	public float pixelsPerUnitScale = 1;

	private Camera camera;

	void Awake()
	{
		camera = GetComponent<Camera>();
	}

	// Update is called once per frame
	void Update ()
	{
		camera.orthographicSize = ((Screen.height)/(pixelsPerUnitScale * pixelsPerUnit)) * 0.5f;
	}
}
