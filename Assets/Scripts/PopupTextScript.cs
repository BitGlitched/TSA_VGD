using UnityEngine;
using System.Collections;

public class PopupTextScript : MonoBehaviour
{
	public float moveSpeed = 1;
	public float textFadeTime = 5;

	private float tempTime = 0;

	private TextMesh textMesh;

	void Awake()
	{
		textMesh = this.gameObject.GetComponent<TextMesh>();
	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		transform.position = new Vector2(transform.position.x, transform.position.y + (moveSpeed * Time.fixedDeltaTime));

		tempTime += Time.fixedDeltaTime;
		if (tempTime >= textFadeTime)
		{
			Destroy(this.gameObject);
		}
	}
}
