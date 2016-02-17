using UnityEngine;
using System.Collections;

public class Ladder : MonoBehaviour
{
	public bool inFrontOfLadder;

	public GameObject ladderBottom;
	private Collider2D ladderBottomCollider;
	public GameObject ladderTop;
	private Collider2D ladderTopCollider;


	void ObjectEnter(Collider2D collision)
	{
		inFrontOfLadder = true;
	}

	void ObjectExit(Collider2D collision)
	{
		inFrontOfLadder = false;
	}
}
