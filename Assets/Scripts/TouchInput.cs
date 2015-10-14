using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TouchInput : MonoBehaviour 
{
		//public GameObject world;
	
	//const float TOUCH_PLAYER_SPEED=0.5f;
	public Text gesture, deadText;

	public bool easyMode;

	Vector2[] touchDeltas;
	float[] times;
	bool isStrafing=false;
	const float Y_THRESHOLD=20, TAP_THRESHOLD=5;
	
	public Text playerSpeedText;
	public const float PLAYER_MAX_SPEED=8,JUMP_SPEED=8, JUMPER_JUMP_SPEED=16, PLAYER_SPEED=8;
	public static float playerMaxSpeed,jumpSpeed, jumperJumpSpeed, playerSpeed;
	//public float ;
	public bool jumping;
	
	public bool jumperBonus;
	
	Rigidbody rb;
	
	GameObject currentTrack;
	float minRoad, maxRoad;
	
	public static Vector3 playerPosition;

	// Use this for initialization
	void Start () 
	{
		StartNewGame ();
	}


	public void StartNewGame(bool easyMode=true)
	{
		deadText.gameObject.SetActive(false);
		this.easyMode=easyMode;
		StopAllCoroutines();
		SetAllSpeeds();
		touchDeltas = new Vector2[2];
		times = new float[2];
		rb = GetComponent<Rigidbody>();
		rb.velocity=Vector3.zero;
		playerPosition = this.transform.position; //?
		jumping=true;
		jumperBonus=false;
		this.GetComponent<DesktopMovement>().StartNewGame();
		StartCoroutine (PlayerSpeed());
	}

	IEnumerator PlayerSpeed()
	{
		while(true)
		{
			playerPosition=this.transform.position;
			playerSpeedText.text = "Speed: "+rb.velocity;
			yield return new WaitForSeconds(.3f);
		}
	}
	
		
	void SetAllSpeeds()
	{
		playerSpeed=PLAYER_SPEED;
		playerMaxSpeed=PLAYER_MAX_SPEED;
		jumpSpeed=JUMP_SPEED;
		jumperJumpSpeed=JUMPER_JUMP_SPEED;
	}
		
		// Update is called once per frame
	void Update () 
	{
		if(easyMode) Run (playerSpeed);
		if(Input.touchCount>0)
		{
			for(int i=0;i<Input.touchCount;i++)
			{
				Touch touch = Input.GetTouch(i);
				if(touch.phase== TouchPhase.Began)
				{
					touchDeltas[i]=touch.position;
					times[i]=Time.time;
				}
//				else if(touch.phase== TouchPhase.Moved)
//				{
//					//touchDeltas[i]+=touch.deltaPosition;
//					//times[i]+=Time.deltaTime;
//				}
				else if(touch.phase== TouchPhase.Ended)
				{
					Vector2 touchDistance = touch.position-touchDeltas[i];
					if(touchDistance.magnitude<TAP_THRESHOLD)
					{
						gesture.text="jumping Mag ="+touchDistance.magnitude;
						Jump ();
					}

					//tz
					else if(touchDistance.magnitude>TAP_THRESHOLD)
					{
						if(Mathf.Abs(touchDistance.x)>Mathf.Abs(touchDistance.y))
						{
							if(!isStrafing)
							{
								isStrafing=true;
								gesture.text="strafing";
								Strafe(touchDistance.x>0?1:-1);
							}
						}
						else if(touchDistance.y<0&&!easyMode)
						{
							float speed = -touchDistance.y/(Time.time-times[i]);
							gesture.text="running. -y="+(-touchDistance.y)+". Time="+Time.time+". StartTime="+times[i]+".speed="+speed;
							Run (speed/15f);
						}
					}


				 	isStrafing=false;
					//touchDeltas.RemoveAt(i);
					//i--;
				}
				//begin.text=touchDeltas[i].ToString();
				
			}
			///testText.text = (  Input.touches[0].phase.ToString());
		}
	}




	public static void IncreaseSpeed (float multiplier)
	{
		playerMaxSpeed*=multiplier;
		playerSpeed*=multiplier;
		playerSpeed*=multiplier;
	}
	
	public void Strafe (int value)
	{
		if(value>0)
		{
			if(Mathf.RoundToInt(transform.position.z)>maxRoad||jumping)
			{
				rb.MovePosition (transform.position+new Vector3(0,0,-1));
			}
		}
		else
		{
			if(Mathf.RoundToInt(transform.position.z)<minRoad||jumping)
			{
				rb.MovePosition (transform.position+new Vector3(0,0,1));
			}
		}
	}
	
	public void Run (float input)
	{
		if(!jumping)
		{
			rb.AddForce(transform.forward*input,ForceMode.Force);

		}
		if(rb.velocity.x>(playerMaxSpeed)) rb.velocity = new Vector3(playerMaxSpeed,rb.velocity.y,0);
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
	
	void OnTriggerEnter(Collider other) 
	{
		if(other.gameObject.tag=="Jumper")
		{
			Debug.Log ("Jumper is here!");
			jumperBonus=true;
			//Jump (jumperJumpSpeed);
		}
		else 
		{
			GetComponent<Rigidbody>().velocity=Vector3.zero;
			BeDead ();
		}
	}


	void BeDead()
	{
		Debug.Log ("You died");
		deadText.gameObject.SetActive(true);
		Time.timeScale=0;
	}
	
	void OnTriggerExit(Collider other) 
	{
		if(other.gameObject.tag=="Jumper")
		{
			jumperBonus=false;
		}
	}
	
	void OnCollisionEnter(Collision collision) 
	{
		if(jumping)
		{
			if(collision.gameObject.tag=="Ground"||collision.gameObject.tag=="BuildingProp")
			{
				BeDead();
			}
			else
			{
				//we hit the track
				TrackManager currentTrack=collision.gameObject.GetComponent<TrackManager>();
				currentTrack.GetMinAndMaxRoads (out minRoad, out maxRoad);

				//FIXME delete this after release
				GetComponent<DesktopMovement>().minRoad=minRoad;
				GetComponent<DesktopMovement>().maxRoad=maxRoad;
				
				jumping=false;
			}
		}
	}
	
	void OnCollisionExit(Collision collisionInfo) 
	{
		jumping=true;
	}
	
	

}






	
	//		void OnTap(TapGesture gesture) 
	//		{ 
	//			if( gesture.Selection )
	//				Debug.Log( "Tapped object: " + gesture.Selection.name );
	//			else
	//				Debug.Log( "No object was tapped at " + gesture.Position );
	//		}
	//	
	//		void OnSwipe(SwipeGesture gesture) 
	//		{
	//			Debug.Log ("Swipe was commenced" + gesture.Move.ToString());
	//		}
	//	
	//		void OnRun(SwipeGesture gesture) 
	//		{ 
	//			world.transform.position=world.transform.position+new Vector3(0,0,gesture.Move.x)*PLAYER_SPEED;
	//			Debug.Log ("Running..." + gesture.Move.ToString ()) ;
	//		}
