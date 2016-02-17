using UnityEngine;
using System.Collections;

public class PlayerGroundCheck : MonoBehaviour
{
	public LayerMask groundLayer;
	public LayerMask wallLayer;
	public LayerMask playerLayer;

	public bool grounded = false;

	private PlayerMovement playerMove;
	private BoxCollider2D topCollider;
	private CircleCollider2D bottomCollider;

	void Awake()
	{
		playerMove = transform.GetComponent<PlayerMovement>();
		topCollider = transform.GetComponent<BoxCollider2D>();
		bottomCollider = transform.GetComponent<CircleCollider2D>();
	}

	// Update is called once per frame
	void Update ()
	{
		//Ground check section, comes first for frame by frame accuracy
		grounded = false;
		
		if (bottomCollider.IsTouchingLayers(groundLayer))
		{
			float castRadius = bottomCollider.bounds.extents.x;
			float yOffset = (bottomCollider.bounds.size.y - bottomCollider.offset.y) * 0.5f;
			Vector2 castVector = new Vector2 (transform.position.x, transform.position.y - yOffset);
			Collider2D[] colliders = Physics2D.OverlapCircleAll(castVector, castRadius, groundLayer);

			Debug.DrawRay(castVector, Vector3.up * castRadius);
			Debug.DrawRay(castVector, Vector3.down * castRadius);
			Debug.DrawRay(castVector, Vector3.left * castRadius);
			Debug.DrawRay(castVector, Vector3.right * castRadius);
			
			for (int count = 0; count < colliders.Length; count++)
			{
				if (colliders[count].gameObject != gameObject)
				{
					grounded = true;
					playerMove.jumping = false;
					playerMove.wallJumping = false;
				}
			}
		}

		if ((topCollider.IsTouchingLayers(groundLayer)))
		{
			playerMove.canJump = false;

			float castRadius = topCollider.bounds.extents.x * 0.9f;
			float yOffset = (topCollider.bounds.size.y + topCollider.offset.y * 1.5f);
			Vector2 castVector = new Vector2 (transform.position.x, transform.position.y + yOffset);
			Collider2D[] colliders = Physics2D.OverlapCircleAll(castVector, castRadius, groundLayer);

			Debug.DrawRay(castVector, Vector3.up * castRadius);

			for (int count = 0; count < colliders.Length; count++)
			{
				if (colliders[count].gameObject != gameObject)
				{
					playerMove.velocityY = 0;
				}

			}
		}
	}
}
