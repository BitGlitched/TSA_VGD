using UnityEngine;
using System.Collections;

public class ItemPickup : MonoBehaviour
{
	public LayerMask playerLayer;
	public string itemName;

	private GameObject player;
	private Collider2D pickupCollider;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		pickupCollider = this.gameObject.GetComponent<Collider2D>();
	}

	// Update is called once per frame
	void Update ()
	{
		if (pickupCollider.IsTouching(player.GetComponent<Collider2D>()));
		{
			player.SendMessage("AddItem", itemName, SendMessageOptions.RequireReceiver);

			Destroy(this.gameObject);
		}
	}
}
