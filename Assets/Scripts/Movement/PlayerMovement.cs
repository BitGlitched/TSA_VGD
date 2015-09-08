using UnityEngine;
using System.Collections;

[RequireComponent(typeof (InputScript))]
public class PlayerMovement : MonoBehaviour
{
	public LayerMask groundLayer;
	public LayerMask wallLayer;
	public LayerMask playerLayer;

	public bool canControl = true;

	public float walkSpeed = 12f;
	public float smoothAmount = 0.0f;
	public float airSpeed = 10f;
	public float jumpHeight = 3.5f;
	public float jumpTime = 1f;
	public float characterMass = 4f;
	public float wallJumpForce = 20;
	public float wallJumpWallOffset = 0.1f;

	private float targetHorizontalSpeed;
	private float horizontalSpeed;
	private float smoothDampVelX;
	public float velocityX;
	public float velocityY;
	private float jumpTimestamp;
	private float externalForceX;
	private float externalVelX;

	public bool canJump = false;
	public bool jumping = false;
	public bool wallJumping = false;
	
	private Vector3 currentScale;

	private InputScript inputScript;
	private PlayerGroundCheck groundCheck;
	private Rigidbody2D rigidbody;
	private BoxCollider2D topCollider;

	void Awake()
	{
		groundCheck = transform.GetComponent<PlayerGroundCheck>();
		inputScript = transform.GetComponent<InputScript>();
		rigidbody = transform.GetComponent<Rigidbody2D>();
		topCollider = transform.GetComponent<BoxCollider2D>();
	}

	// Update is called once per frame
	void Update ()
	{
		//Moved here to get input more accurately
		if ((!inputScript.jumpInputPressed)&&(groundCheck.grounded))
		{
			canJump = true;
		}
		
		if ((inputScript.jumpInputUp)&&(jumping))
		{
			canJump = false;
		}

		if ((groundCheck.grounded == false)&&(inputScript.jumpInputDown))
		{
			WallJump();
		}

		if (jumping == false)
		{
			jumpTimestamp = 0;
		}
	}

	void FixedUpdate()
	{
		horizontalSpeed = 0;
		currentScale = transform.localScale;
		Vector2 currentPosition = new Vector2 (transform.position.x, transform.position.y);

/*/////////////////////////////////////////////////////////////////////////////////////
///			HORIZONTAL MOVEMENT SECTION												///
/////////////////////////////////////////////////////////////////////////////////////*/
		if (canControl)
		{
			//Regular movement intention section
			if (inputScript.rightInputPressed)
			{
				horizontalSpeed++;
			}

			if (inputScript.leftInputPressed)
			{
				horizontalSpeed--;
			}

			//Actual movement instructions
			if (groundCheck.grounded == true)
			{
				velocityX = horizontalSpeed * walkSpeed;
			}
			else if (!wallJumping)
			{
				velocityX = horizontalSpeed * airSpeed;
			}
			else
			{
				velocityX += horizontalSpeed * airSpeed * Time.fixedDeltaTime;
				velocityX = Mathf.Clamp(velocityX, -airSpeed, airSpeed);
			}


			/*/////////////////////////////////////////////////////////////////////////////////////
			///			VERTICAL/JUMPING SECTION												///
			/////////////////////////////////////////////////////////////////////////////////////*/

			//Jumping section
			if ((inputScript.jumpInputPressed)&&(canJump)&&(jumpTimestamp < jumpTime))
			{
				jumping = true;
				jumpTimestamp += Time.fixedDeltaTime;

				velocityY = jumpHeight * jumpTime / (jumpTime * jumpTime);
			}
			else if ((jumpTimestamp > jumpTime)&&(jumping))
			{
				canJump = false;
			}
		}


		/*/////////////////////////////////////////////////////////////////////////////////////
		///			GRAVITY SECTION															///
		/////////////////////////////////////////////////////////////////////////////////////*/
		if ((groundCheck.grounded)&&(!jumping))
		{
			velocityY = 0;
		}

		velocityY += (-9.8f * characterMass) * Time.fixedDeltaTime;


		/*/////////////////////////////////////////////////////////////////////////////////////
		///			SPRITE FLIPPING SECTION													///
		/////////////////////////////////////////////////////////////////////////////////////*/

		//Function to flip the sprite
		if ((groundCheck.grounded)&&(horizontalSpeed != 0))
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
		///			FINAL MOVEMENT SECTION													///
		/////////////////////////////////////////////////////////////////////////////////////*/
		
		Vector2 newPosition = new Vector2 (velocityX, velocityY);
		rigidbody.velocity = newPosition;
	}

	void WallJump()
	{
		Vector2 currentDir = Vector2.right * currentScale.x;
		float rayLength = (topCollider.size.x * 0.5f) + wallJumpWallOffset;
		Vector2 currentPosition2D = new Vector2 (transform.position.x, transform.position.y);
		Vector2 startVec2 = new Vector2 (transform.position.x, transform.position.y + topCollider.size.y);

		Debug.DrawRay(startVec2, Vector2.right * currentScale.x);
			
		if (Physics2D.Raycast(currentPosition2D, currentDir, rayLength, playerLayer))
		{
			print ("Walljumped!");
			wallJumping = true;

			velocityY = wallJumpForce;
			velocityX = airSpeed * -currentScale.x;
		}
	}
}
