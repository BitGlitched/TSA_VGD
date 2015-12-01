using UnityEngine;
using System.Collections;

public class SwitchLevel : MonoBehaviour
{
	public string levelName;
	
	private GameObject player;
	private Collider2D collider;
	
	// Use this for initialization
	void Awake ()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		
		collider = this.gameObject.GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (collider.IsTouching(player.GetComponent<BoxCollider2D>()))
		{
			Application.LoadLevel(levelName);
		}
	}
}
