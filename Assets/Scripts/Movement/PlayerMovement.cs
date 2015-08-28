using UnityEngine;
using System.Collections;

[RequireComponent(typeof (InputScript))]
public class PlayerMovement : MonoBehaviour
{
	public LayerMask groundLayer;
	public LayerMask wallLayer;

	public bool canMove = true;

	public float walkSpeed = 12f;
	public float speedUpDuration = 0.1f;
	public float airSpeed = 8f;
	public float jumpHeight = 3.5f;
	public float jumpSpeed = 5f;
	public float jumpTime = 1f;
	public float characterMass = 1f;

	private float targetHorizontalSpeed;
	private float horizontalSpeed;
	private float smoothDampVelX;
	private float velocityX;
	public float velocityY;
	public float jumpTimestamp;

	public bool canJump = false;
	public bool jumping = false;
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
					jumping = false;
					jumpTimestamp = 0;
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
				jumpTimestamp += Time.fixedDeltaTime; 

				//This formula uses the gravity scale and height to calculate how fast the object needs to per second to reach the designated height
				velocityY = Mathf.Sqrt(2 * (9.8f / characterMass) * jumpHeight);

				//velocityY = velocityY / (velocityY * jumpTime);
			}
			else
			{
				canJump = false;
			}
		}
		else if ((inputScript.jumpInputUp)&&(jumping))
		{
			canJump = false;
			jumping = false;

			if (jumpTimestamp < jumpTime)
			{
				velocityY = -velocityY;
			}
		}


		/*/////////////////////////////////////////////////////////////////////////////////////
		///			GRAVITY SECTION												///
		/////////////////////////////////////////////////////////////////////////////////////*/
		if ((grounded)&&(!jumping))
		{
			velocityY = 0;
		}

		velocityY += (-9.8f) * Time.fixedDeltaTime;

		velocityY = Mathf.Clamp(velocityY, -9.8f * characterMass, 1000);


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