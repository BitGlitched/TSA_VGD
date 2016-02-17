using UnityEngine;
using System.Collections;

[RequireComponent(typeof (InputScript))]
public class PlayerMovement : MonoBehaviour
{
	public LayerMask groundLayer;
	public LayerMask wallLayer;
	public LayerMask playerLayer;
	public LayerMask ladderLayer;

	public bool canControl = true;
	public bool stuck = false;
	public bool wallJumpAssist = false;

	public float walkSpeed = 4f;
	public float runSpeed = 12f;
	public float airSpeed = 11f;
	public float climbSpeed = 4f;
	public float jumpHeight = 5f;
	public float jumpTime = 0.35f;
	public float characterMass = 4f;
	public float doubleJumpForce = 11f;
	public float wallJumpForce = 11f;
	public float wallJumpWallOffset = 0.1f;

	private float currentSpeed;
	public float horizontalSpeed;
	public float ladderSpeed;
	private float velocityX;
	public float velocityY;
	private float jumpTimestamp;
	private float externalForceX;

	public bool canJump = false;
	public bool jumping = false;
	public bool doubleJumped = false;
	public bool wallJumping = false;
	public bool running = false;
	public bool onLadder = false;
	private bool canMoveUpLadder = true;
	private bool canMoveDownLadder = true;

	private Vector3 currentScale;

	private InputScript inputScript;
	private ItemInventory inventory;
	private PlayerGroundCheck groundCheck;
	private Rigidbody2D rigidbody;
	private BoxCollider2D topCollider;
	private SpriteAnimator spriteAnimator;

	void Awake()
	{
		groundCheck = transform.GetComponent<PlayerGroundCheck>();
		inventory = transform.GetComponent<ItemInventory>();
		inputScript = transform.GetComponent<InputScript>();
		rigidbody = transform.GetComponent<Rigidbody2D>();
		topCollider = transform.GetComponent<BoxCollider2D>();
		spriteAnimator = transform.GetComponent<SpriteAnimator>();
	}

	// Update is called once per frame
	void Update ()
	{
		if (canControl && !stuck)
		{
			//Checks to see if we are running or walking
			if (inputScript.runInputPressed)
			{
				currentSpeed = runSpeed;
				running = true;
			}
			else
			{
				currentSpeed = walkSpeed;
				running = false;
			}

			//Moved here to get input more accurately
			if (inputScript.upInputDown && canControl && !onLadder)
			{
				RaycastHit2D[] ladderCast = Physics2D.CircleCastAll(transform.position, topCollider.size.x / 2, Vector2.up, topCollider.size.x / 2, ladderLayer);

				if (ladderCast.Length > 0)
				{
					GetOnLadder(ladderCast[0].transform.position.x);
				}
			}

			if (groundCheck.grounded && onLadder && ladderSpeed < 0)
			{
				onLadder = false;
			}

			if ((!inputScript.jumpInputPressed||inputScript.upInputPressed)&&(groundCheck.grounded))
			{
				canJump = true;
			}
			else if ((groundCheck.grounded == false)&&(jumping == false))
			{
				canJump = false;
			}

			if ((inputScript.jumpInputUp||inputScript.upInputUp)&&(jumping))
			{
				canJump = false;
			}

			if ((inputScript.jumpInputDown||inputScript.upInputDown)&&jumping)
			{
				if (inventory.hasDoubleJump)
				{
					if (!doubleJumped)
					{
						velocityY = doubleJumpForce;
						doubleJumped = true;
					}
				}
			}

			if ((wallJumpAssist)&&(groundCheck.grounded == false)&&(inputScript.jumpInputPressed||inputScript.upInputPressed))
			{
				WallJump();
			}
			else if ((groundCheck.grounded == false)&&(inputScript.jumpInputDown||inputScript.upInputDown))
			{
				WallJump();
			}

			if (onLadder && (inputScript.jumpInputDown || inputScript.leftInputDown || inputScript.rightInputDown))
			{
				GetOffLadder();
				
				if (inputScript.jumpInputDown)
				{
					jumpTimestamp = 0;
					jumping = true;
					canJump = true;
				}
			}

			if (jumping == false)
			{
				jumpTimestamp = 0;
			}
		}
	}

	void FixedUpdate()
	{
		horizontalSpeed = 0;
		ladderSpeed = 0;
		currentScale = transform.localScale;
		Vector2 currentPosition = new Vector2 (transform.position.x, transform.position.y);

/*/////////////////////////////////////////////////////////////////////////////////////
///			HORIZONTAL MOVEMENT SECTION												///
/////////////////////////////////////////////////////////////////////////////////////*/
		if (canControl && !stuck && (!onLadder))
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
				velocityX = horizontalSpeed * currentSpeed;
				doubleJumped = false;
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
			if ((inputScript.jumpInputPressed||inputScript.upInputPressed)&&(canJump)&&(jumpTimestamp < jumpTime))
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
		else if (onLadder)
		{
			if (inputScript.upInputPressed && canMoveUpLadder)
			{
				ladderSpeed++;
			}

			if (inputScript.downInputPressed && canMoveDownLadder)
			{
				ladderSpeed--;
			}

			float finalSpeed = ladderSpeed * climbSpeed * Time.fixedDeltaTime;

			transform.position = new Vector2(transform.position.x, transform.position.y + finalSpeed);
		}


		/*/////////////////////////////////////////////////////////////////////////////////////
		///			GRAVITY SECTION															///
		/////////////////////////////////////////////////////////////////////////////////////*/
		if (((groundCheck.grounded)&&(!jumping))||(onLadder))
		{
			velocityY = 0;
		}

		if (!onLadder)
		{
			velocityY += (-9.8f * characterMass) * Time.fixedDeltaTime;
		}


		/*/////////////////////////////////////////////////////////////////////////////////////
		///			FINAL MOVEMENT SECTION													///
		/////////////////////////////////////////////////////////////////////////////////////*/
		
		Vector2 newPosition = new Vector2 (velocityX, velocityY);
		rigidbody.velocity = newPosition;

		/*/////////////////////////////////////////////////////////////////////////////////////
		///			SPRITE FLIPPING SECTION													///
		/////////////////////////////////////////////////////////////////////////////////////*/
		
		//Function to flip the sprite
		if (horizontalSpeed != 0)
		{
			float absDirectionValue = horizontalSpeed / Mathf.Abs(horizontalSpeed);
			
			if ((currentScale.x != absDirectionValue))
			{
				FlipSprite(absDirectionValue);
			}
		}
	}

	void FlipSprite(float dir)
	{
		Vector3 newScale = currentScale;
		newScale.x = dir;
		transform.localScale = newScale;
	}

	public void IsStuck(bool isIt)
	{
		stuck = isIt;
	}

	public void CanControl(bool canIt)
	{
		canControl = canIt;
	}

	public void ResetSpeed()
	{
		horizontalSpeed = 0;
		currentSpeed = 0;
		velocityX = 0;
		running = false;
	}


	/*/////////////////////////////////////////////////////////////////////////////////////
	///			WALLJUMP SECTION														///
	/////////////////////////////////////////////////////////////////////////////////////*/

	void WallJump()
	{
		if (inventory.hasWallJump)
		{
			Vector2 currentDir = Vector2.right * currentScale.x;
			float rayLength = (topCollider.size.x * 0.5f) + wallJumpWallOffset;
			Vector2 currentPosition2D = new Vector2 (transform.position.x, transform.position.y);
			Vector2 startVec2 = new Vector2 (transform.position.x, transform.position.y + topCollider.size.y);

			Debug.DrawRay(startVec2, Vector2.right * currentScale.x);
				
			if (Physics2D.Raycast(currentPosition2D, currentDir, rayLength, wallLayer))
			{
				print ("Walljumped!");
				wallJumping = true;

				velocityY = wallJumpForce;
				velocityX = airSpeed * -currentScale.x;
				FlipSprite(-currentDir.x);
			}
			else if (Physics2D.Raycast(currentPosition2D, -currentDir, rayLength, wallLayer))
			{
				print ("Walljumped!");
				wallJumping = true;
				
				velocityY = wallJumpForce;
				velocityX = airSpeed * currentScale.x;
				FlipSprite(-currentDir.x);
			}
		}
	}

	/*/////////////////////////////////////////////////////////////////////////////////////
	///			LADDER SECTION															///
	/////////////////////////////////////////////////////////////////////////////////////*/

	public void GetOnLadder(float ladderXPosition)
	{
		print ("Getting on the ladder!");
		horizontalSpeed = 0;
		currentSpeed = 0;
		velocityX = 0;
		rigidbody.velocity = Vector2.zero;
		onLadder = true;
		transform.position = new Vector2(ladderXPosition, transform.position.y);

	}

	public void GetOffLadder()
	{
		onLadder = false;
	}

	public void ClimbOffLadder(float exitYPosition)
	{
		transform.position = new Vector2(transform.position.x, exitYPosition);
		onLadder = false;
	}

	public void CanLadderMoveDirection(bool up)
	{
		if (up)
		{
			canMoveUpLadder = true;
		}
		else
		{
			canMoveDownLadder = true;
		}
	}

	public void CantMoveLadderDirection(bool up)
	{
		if (up)
		{
			canMoveUpLadder = false;
		}
		else
		{
			canMoveDownLadder = false;
		}
	}
}
