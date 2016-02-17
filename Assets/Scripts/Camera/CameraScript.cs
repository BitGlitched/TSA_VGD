using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
	public float moveTime = 0.1f;
	public float defaultHieght = 5;
	public float minHeight = 4.5f;
	public float maxHeight = 8.5f;

	public float cameraZPosition = -10;

	private Vector2 cameraPositionVec2;
	private Vector2 playerPositionVec2;

	private Vector2 velocityRef;
	private Vector3 newPosition;

	public GameObject target;

	void Update()
	{
		cameraPositionVec2 = new Vector2 (transform.position.x, transform.position.y);
		playerPositionVec2 = new Vector2 (target.transform.position.x, target.transform.position.y + (target.transform.localScale.y * 0.5f));

		if (transform.position != target.transform.position)
		{
			newPosition = Vector2.SmoothDamp(cameraPositionVec2, playerPositionVec2, ref velocityRef, moveTime);
		}
		
		if (newPosition.y < minHeight)
		{
			newPosition.y = minHeight;
		}

		transform.position = new Vector3 (newPosition.x, newPosition.y, cameraZPosition);
	}
}
