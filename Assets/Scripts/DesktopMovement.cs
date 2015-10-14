using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DesktopMovement : MonoBehaviour 
{
	public Text playerSpeedText;

	public static float playerMaxSpeed,jumpSpeed, jumperJumpSpeed, playerSpeed;
	//public float ;
	bool jumping;

	bool jumperBonus=false;

	Rigidbody rb;

	GameObject currentTrack;
	public float minRoad, maxRoad;

	public static Vector3 playerPosition;

	// Use this for initialization
	void Start () 
	{
		StartNewGame();
	}

	public void StartNewGame ()
	{
		rb = GetComponent<Rigidbody>();
		//playerPosition = this.transform.position;
		jumping=true;
		SetAllSpeeds ();
	}

	public static void SetAllSpeeds()
	{
		playerSpeed=TouchInput.playerSpeed;
		playerMaxSpeed=TouchInput.playerMaxSpeed;
		jumpSpeed=TouchInput.jumpSpeed;
		jumperJumpSpeed=TouchInput.JUMPER_JUMP_SPEED;
	}

	// Update is called once per frame
	void Update () 
	{
		jumperBonus=GetComponent<TouchInput>().jumperBonus;
		jumping=GetComponent<TouchInput>().jumping;
		float verticalInput=Input.GetAxis("Vertical");
		float horizontalInput = Input.GetAxis ("Horizontal");
		if(verticalInput!=0)
		{
			Run(verticalInput);


		}

		if(Input.GetKeyDown(KeyCode.Space))
		{
			Jump ();

		}

		if(Input.GetKeyDown(KeyCode.D))
		{
			Strafe(1);
			 //rb.MovePosition (rb.gameObject.transform.position+new Vector3(0,0,-1));
		}
		if(Input.GetKeyDown(KeyCode.A))
		{
			Strafe(-1);
			//rb.MovePosition (rb.gameObject.transform.position+new Vector3(0,0,1));
		}
		//GetComponent<Rigidbody>().velocity = forces;
	}


	public void Strafe (int value)
	{

		if(value>0)
		{
			if(Mathf.RoundToInt(transform.position.z)>maxRoad||jumping)
			{
				rb.MovePosition (rb.gameObject.transform.position+new Vector3(0,0,-1));
			}
		}
		else
		{
			if(Mathf.RoundToInt(transform.position.z)<minRoad||jumping)
			{
				rb.MovePosition (rb.gameObject.transform.position+new Vector3(0,0,1));
			}
		}
	}

	public void Run (float input, bool desktopInput=true)
	{
		if(!jumping)
		{
			if(desktopInput) 
			{
				rb.AddForce(transform.forward*playerSpeed*((input>0)?1:-1),ForceMode.Force);
			}
			else
			{
				rb.AddForce(transform.forward*input,ForceMode.Force);
			}
		}
		if(rb.velocity.x>playerMaxSpeed) rb.velocity = new Vector3(playerMaxSpeed,rb.velocity.y,0);
	}

	public void Jump()
	{
		if(!jumping)
		{
			float jumpStrength;
			jumpStrength=jumperBonus?jumperJumpSpeed:jumpSpeed;
			GetComponent<Rigidbody>().AddForce(transform.up*jumpStrength,ForceMode.VelocityChange);
			jumping=true;
		}
	}

//	void OnTriggerEnter(Collider other) 
//	{
//		if(other.gameObject.tag=="Jumper")
//		{
//			Debug.Log ("Jumper is here!");
//			jumperBonus=true;
//			//Jump (jumperJumpSpeed);
//		}
//		else GetComponent<Rigidbody>().velocity=Vector3.zero;
//	}
//
//
//	void OnTriggerExit(Collider other) 
//	{
//		if(other.gameObject.tag=="Jumper")
//		{
//			jumperBonus=false;
//		}
//	}
//
//	void OnCollisionEnter(Collision collision) 
//	{
//		if(jumping)
//		{
//			if(collision.gameObject.tag=="Ground"||collision.gameObject.tag=="BuildingProp")
//			{
//
//			}
//			else
//			{
//	//			Debug.Log ("We touched the ground. Track is "+collision.gameObject.name);
//				TrackManager currentTrack=collision.gameObject.GetComponent<TrackManager>();
//
//				currentTrack.GetMinAndMaxRoads (out minRoad, out maxRoad);
//	//			Debug.Log(minRoad+","+maxRoad+""+Mathf.RoundToInt(transform.position.z));
////				Debug.Log(currentTrack.ShowRoads());
//				jumping=false;
//			}
//		}
//	}
//
//	void OnCollisionExit(Collision collisionInfo) 
//	{
//		jumping=true;
//	}


}
