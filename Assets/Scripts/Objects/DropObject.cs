using UnityEngine;
using System.Collections;

public class DropObject : MonoBehaviour
{
	public float objectMass = 1;
	private Rigidbody2D rb;

	void Awake()
	{
		this.GetComponent<Rigidbody2D>();
	}

	public void Drop()
	{
		if (rb)
		{
			rb.isKinematic = false;
		}
	}

	void OnCollisionEnter2D()
	{
		if (rb.mass != objectMass)
		{
			rb.isKinematic = true;
		}
	}
}
