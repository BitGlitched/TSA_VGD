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
	private float velocityX;
	private float velocityY;
	private float jumpTimestamp;
	private float externalForceX;
	private float externalVelX;

	public bool canJump = false;
	public bool jumping = false;
	
	private Vector3 currentScale;

	private InputScript inputScript;
	private PlayerGroundCheck groundCheck;
	private Rigidbody2D rigidbody;
	private BoxCollider2D topCollider;
	private CircleCollider2D bottomCollider;

	void Awake()
	{
		groundCheck = transform.GetComponent<PlayerGroundCheck>();
		inputScript = transform.GetComponent<InputScript>();
		rigidbody = transform.GetComponent<Rigidbody2D>();
		topCollider = transform.GetComponent<BoxCollider2D>();
		bottomCollider = transform.GetComponent<CircleCollider2D>();
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
		currentScale = transform.localScale;
		horizontalSpeed = 0;


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
			else
			{
				velocityX = horizontalSpeed * airSpeed;
			}


			/*/////////////////////////////////////////////////////////////////////////////////////
			///			VERTICAL/JUMPING SECTION												///
			/////////////////////////////////////////////////////////////////////////////////////*/

			//Jumping section
			if ((inputScript.jumpInputPressed)&&(canJump)&&(jumpTimestamp < jumpTime))
			{
				jumping = true;
				jumpTimestamp += Time.fixedDeltaTime;

				velocityY = (jumpHeight) / (jumpTime);
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

		if (externalForceX != 0)
		{
			externalForceX = Mathf.SmoothDamp(externalForceX, 0, ref externalVelX, 1);
		}

		Vector2 currentPosition = new Vector2 (transform.position.x, transform.position.y);
		Vector2 newPosition = new Vector2 (velocityX + externalForceX, velocityY);
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
				//externalForceX = -currentScale.x * airSpeed;
				rigidbody.velocity = new Vector2(-currentScale.x * airSpeed, rigidbody.velocity.y);
				velocityY = wallJumpForce;
		}
	}

	void SetVelocityX(float velX)
	{
		velocityX = velX;
	}

	void SetVelocityY(float velY)
	{
		velocityY = velY;
	}
}
