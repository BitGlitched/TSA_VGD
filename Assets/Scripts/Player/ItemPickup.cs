using UnityEngine;
using System.Collections;

public class ItemPickup : MonoBehaviour
{
	public LayerMask playerLayer;
	public string itemName;
	public GameObject textPopup;
	public Vector3 textPopupOffset;

	// Update is called once per frame
	void OnTriggerEnter2D (Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			collision.SendMessage("AddItem", itemName, SendMessageOptions.RequireReceiver);

			if (textPopup != null)
			{
				Instantiate(textPopup, transform.position + textPopupOffset, transform.rotation);
			}

			print("The pickup is colliding with the player!");
			Destroy(this.gameObject);
		}
	}
}
