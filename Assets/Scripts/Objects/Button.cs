using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour
{
	public GameObject objectAffected;
	//private Rigidbody2D rb;
	public string functionName = "foo";
	private bool pressed;

	void Awake()
	{
		//rb = objectAffected.GetComponent<Rigidbody2D>();
	}

	void ObjectEnter()
	{
		if (objectAffected)
		{
			objectAffected.SendMessage(functionName, SendMessageOptions.DontRequireReceiver);
		}
	}
}
