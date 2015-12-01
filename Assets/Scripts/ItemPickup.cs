using UnityEngine;
using System.Collections;

public class ItemPickup : MonoBehaviour
{
	public LayerMask playerLayer;
	public string itemName;
	public GameObject textPopup;
	public Vector3 textPopupOffset;

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
		if (pickupCollider.IsTouching(player.GetComponent<BoxCollider2D>()))
		{
			player.SendMessage("AddItem", itemName, SendMessageOptions.RequireReceiver);

			if (textPopup != null)
			{
				Instantiate(textPopup, transform.position + textPopupOffset, transform.rotation);
			}

			print("The pickup is colliding with the player!");
			Destroy(this.gameObject);
		}
	}
}
