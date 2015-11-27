using UnityEngine;
using System.Collections;

public class ItemPickup : MonoBehaviour
{
	public string itemName;

	private Collider2D pickupCollider;

	void Awake()
	{
		pickupCollider = this.gameObject.GetComponent<Collider2D>();
	}

	// Update is called once per frame
	void Update ()
	{

	}
}
