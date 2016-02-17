using UnityEngine;
using System.Collections;

public class LadderEnd : MonoBehaviour
{
	public bool TopOfLadder = true;

	private Collider2D collider;

	void Awake()
	{
		collider = transform.GetComponent<Collider2D>();
	}


	void ObjectEnter(Collider2D collision)
	{
			print ("Player has entered");

			collision.gameObject.SendMessage("CantMoveLadderDirection", TopOfLadder);
	}

	void ObjectExit(Collider2D collision)
	{
			collision.gameObject.SendMessage("CanLadderMoveDirection", TopOfLadder);
	}
}
