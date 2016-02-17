using UnityEngine;
using System.Collections;

public class ItemInventory : MonoBehaviour
{
	public bool hasDoubleJump;
	public bool hasWallJump;
	public bool hasJetpack;

	void AddItem (string itemName)
	{
		if (itemName == "Double Jump")
		{
			hasDoubleJump = true;
		}

		if (itemName == "Wall Jump")
		{
			hasWallJump = true;
		}

		if (itemName == "Jetpack")
		{
			hasJetpack = true;
		}
	}

	void RemoveItem (string itemName)
	{
		if (itemName == "Double Jump")
		{
			hasDoubleJump = false;
		}
		
		if (itemName == "Wall Jump")
		{
			hasWallJump = false;
		}
		
		if (itemName == "Jetpack")
		{
			hasJetpack = false;
		}
	}
}
