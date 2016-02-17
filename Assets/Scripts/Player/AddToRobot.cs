using UnityEngine;
using System.Collections;

public class AddToRobot : MonoBehaviour
{
	public GameObject robotPlayer;

	void AddItem(string itemName)
	{
		if (robotPlayer)
		{
			robotPlayer.SendMessage("AddItem", itemName, SendMessageOptions.RequireReceiver);
		}
		else
		{
			Debug.LogError("No 'robotPlayer' was put on the script.");
		}
	}
}
