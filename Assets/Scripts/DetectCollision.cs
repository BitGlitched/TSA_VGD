using UnityEngine;
using System.Collections;

public class DetectCollision : MonoBehaviour
{
	public string ObjectTag = "Player";
	
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == ObjectTag)
		{
			SendMessage("ObjectEnter", collision);
			print("Player entered detection");
		}
	}

	void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == ObjectTag)
		{
			SendMessage("ObjectExit", collision, SendMessageOptions.DontRequireReceiver);
		}
	}
}
