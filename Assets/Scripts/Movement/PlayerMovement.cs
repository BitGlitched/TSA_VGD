using UnityEngine;
using System.Collections;

[RequireComponent(typeof (InputScript))]
public class PlayerMovement : MonoBehaviour
{
	public LayerMask groundLayer;
	public LayerMask wallLayer;

	public bool canMove = true;

	public float walkSpeed = 12;
	public float speedUpDuration = 0.1f;
	public float airSpeed = 8;
	public float jumpHeight = 3.5f;
	public float jumpSpeed = 6;
	public float jumpTime = 1f;
	public float characterMass = 1;

	private float targetHorizontalSpeed;
	private float horizontalSpeed;
	private float smoothDampVelX;
	private float velocityX;
	public float velocityY;
	private float jumpTimestamp;

	private bool canJump = false;
	private bool jumping = false;
	public bool grounded;

	private Vector3 currentScale;

	private InputScript inputScript;

	private Rigidbody2D rigidbody;
	private Collider2D collider;

	void Awake()
	{
		inputScript = transform.GetComponent<InputScript>();
		rigidbody = transform.GetComponent<Rigidbody2D>();
		collider = transform.GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		//Ground check section, comes first for frame by frame perect accuracy
		grounded = false;

		if (collider.IsTouchingLayers(groundLayer))
		{
			float castRadius = collider.bounds.extents.x;
			float yOffset = (collider.bounds.size.y - collider.offset.y) * 0.5f;
			Vector2 castVector = new Vector2 (transform.position.x, transform.position.y - yOffset);
			Collider2D[] colliders = Physics2D.OverlapCircleAll(castVector, castRadius, groundLayer);
			
			for (int count = 0; count < colliders.Length; count++)
			{
				if (colliders[count].gameObject != gameObject)
				{
					grounded = true;
					canJump = true;
					
					if ((rigidbody.velocity.y <= 0)&&(jumping))
					{
						jumping = false;
						jumpTimestamp = 0;
					}
				}
			}
		}
	}

	void FixedUpdate()
	{
		targetHorizontalSpeed = 0;
		currentScale = transform.localScale;

/*/////////////////////////////////////////////////////////////////////////////////////
///			HORIZONTAL MOVEMENT SECTION												///
/////////////////////////////////////////////////////////////////////////////////////*/

		//Regular movement intention section
		if (inputScript.rightInputPressed)
		{
			targetHorizontalSpeed++;
		}
		
		if (inputScript.leftInputPressed)
		{
			targetHorizontalSpeed--;
		}

		//Smoothing of horizontal speed
		if (horizontalSpeed != targetHorizontalSpeed)
		{
			horizontalSpeed = Mathf.SmoothDamp(horizontalSpeed, targetHorizontalSpeed, ref smoothDampVelX, speedUpDuration);
		}

		//Actual movement instructions
		if (grounded == true)
		{
			velocityX = horizontalSpeed * walkSpeed;
		}
		else
		{
			velocityX = horizontalSpeed * airSpeed;
		}

		/*/////////////////////////////////////////////////////////////////////////////////////
		///			VERTICAL/JUMPING SECTION												///
		/////////////////////////////////////////////////////////////////////////////////////*/

		//Jumping section
		if (inputScript.jumpInputPressed)
		{
			if ((canJump)&&(jumpTimestamp < jumpTime))
			{
				jumping = true;

				velocityY = ((jumpHeight * jumpTime) / (jumpSpeed));
			}
		}
		else if ((inputScript.jumpInputUp)&&(jumping))
		{
			canJump = false;
		}

		/*/////////////////////////////////////////////////////////////////////////////////////
		///			GRAVITY SECTION												///
		/////////////////////////////////////////////////////////////////////////////////////*/
		if ((grounded)&&(!jumping))
		{
			velocityY = -1;
		}
		else if (jumping)
		{
			jumpTimestamp += Time.fixedDeltaTime; 
		}
		else
		{
			velocityY = Mathf.Clamp(velocityY, -9.8f, 9.8f);
		}

		velocityY += Time.fixedDeltaTime * (-9.8f * characterMass);



		/*/////////////////////////////////////////////////////////////////////////////////////
		///			SPRITE FLIPPING SECTION													///
		/////////////////////////////////////////////////////////////////////////////////////*/

		//Function to flip the sprite
		if ((grounded)&&(horizontalSpeed != 0))
		{
			float absDirectionValue = horizontalSpeed / Mathf.Abs(horizontalSpeed);

			if ((currentScale.x != absDirectionValue))
			{
				Vector3 newScale = currentScale;
				newScale.x = absDirectionValue;
				transform.localScale = newScale;
			}
		}

		/*/////////////////////////////////////////////////////////////////////////////////////
		///			FINAL MOVEMENT SECTION												///
		/////////////////////////////////////////////////////////////////////////////////////*/

		Vector2 currentPosition = new Vector2 (transform.position.x, transform.position.y);
		Vector2 newPosition = new Vector2 (velocityX, velocityY);
		rigidbody.MovePosition (currentPosition + newPosition * Time.fixedDeltaTime) ;
	}
}