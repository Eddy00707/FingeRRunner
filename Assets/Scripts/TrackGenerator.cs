using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrackGenerator : MonoBehaviour 
{
	public bool showTrackBalls;



	public GameObject player,jumper,bolt;
	public GameObject[] trackBuildings;
	public GameObject jumpWay, enter,exit;

	public float trackRenderDistance;

	GameObject prevBuilding;

	public float playerSpeedHelperPercent;
	float speedToMakeIt, jumpForce,gravity, jumperJumpForce;

	float yOrigin;

	float xPositionForNextLevel;

	bool tickTock=false;

	int iterator=0;
	bool heightChangeDirection;
	int prevI, prevJ;
	float xCenter;
	List<KeyValuePair<int, float>> Heights;
	Vector3[] positions, globalScales;


	// Use this for initialization
	void Start () 
	{
		StartNewGame ();
	}

	public void StartNewGame()
	{
		heightChangeDirection=Random.value<.5f?true:false;
		foreach (Transform child in transform) 
		{
			GameObject.Destroy(child.gameObject);
		}
		//StopAllCoroutines ();
		xPositionForNextLevel=100;
		jumpForce=TouchInput.JUMP_SPEED-playerSpeedHelperPercent;
		jumperJumpForce=TouchInput.JUMPER_JUMP_SPEED-playerSpeedHelperPercent;
		gravity=-Physics.gravity.y;
		speedToMakeIt=TouchInput.PLAYER_MAX_SPEED;
		trackRenderDistance = speedToMakeIt*2;
		
		positions    = new Vector3[trackBuildings.Length];
		globalScales = new Vector3[trackBuildings.Length];
		
		for(int i=0;i<trackBuildings.Length;i++)
		{
			
			Transform t = trackBuildings[i].GetComponent<TrackFinder>().track.transform;
//			t.gameObject.GetComponent<MeshRenderer>().enabled=false;
			positions[i] = t.position;
			globalScales[i] = t.lossyScale;
			
		}
		
		//building section
		
		Heights = new List<KeyValuePair<int, float>>();
		
		for(int j=0;j<positions.Length;j++)
		{
			Heights.Add (new KeyValuePair<int, float>(j, positions[j].y));
		}
		Heights.Sort ((x,y) => x.Value.CompareTo (y.Value));
		xCenter=0;
		
		yOrigin=0;
		
		prevI=-1;
		prevJ=-1;
		iterator =Random.Range (0,trackBuildings.Length);
		
		StartCoroutine(CreateTrack());
	}




	void Update () 
	{
		if(TouchInput.playerPosition.x>=xPositionForNextLevel)
		{
			//IncreaseSpeed();
			if(!tickTock) IncreaseBuilding (1.1f);
			else if(TouchInput.playerPosition.x>=xPositionForNextLevel+trackRenderDistance&&tickTock)
			{
				IncreaseSpeed(1.1f);
			}
		}


	}

	IEnumerator CreateTrack()
	{
		while(true)
		{
			if(TouchInput.playerPosition.x>=xCenter-trackRenderDistance)
			{
				AddBuilding();	
				ChangeIterator();
				
			}
			yield return new WaitForSeconds(.2f);
		}
	}

	void IncreaseBuilding(float multiplier)
	{
		speedToMakeIt*=multiplier;
		trackRenderDistance = speedToMakeIt*2;
		tickTock = true;

	}
	void IncreaseSpeed (float multiplier)
	{
		TouchInput.IncreaseSpeed(multiplier);
		DesktopMovement.SetAllSpeeds ();
		tickTock=false;
		xPositionForNextLevel+=xPositionForNextLevel*1.2f;


	}

	void ChangeIterator()
	{
		float rnd = Random.value;
		int max = trackBuildings.Length-1;
		float threshold=heightChangeDirection?0.8f:0.2f;
		if(rnd<threshold)
		{
			if(iterator>1&&rnd<threshold/2) iterator-=2;
			else if(iterator>0&&rnd>=threshold/2) iterator--;
			else 
			{
				heightChangeDirection=false;
				iterator++;
			}
		}
		else
		{
			if(iterator<max-1&&rnd>threshold+(1-threshold)/2) iterator+=2;
			else if(iterator<max&&rnd<=threshold+(1-threshold)/2) iterator++;
			else 
			{
				heightChangeDirection=true;
				iterator--;
			}
		}
		//iterator++;
	}

	void SetPlayerPosition (Vector3 value)
	{
		player.transform.position = value;
	}

	int FindExitTrack (int enterTrack, int trackCount)
	{
		int rnd = Random.Range (-2,3);

		rnd+=enterTrack;
		if(0>rnd) rnd=0;
		else if(trackCount-1<rnd) rnd=trackCount-1;
		//Debug.Log ("exit"+rnd);
		return rnd;
	}

	void AddBuilding()
	{
		int i=Heights[iterator].Key;

		GameObject newBuilding;
		if(prevI==-1)
		{
			newBuilding =  (GameObject)Instantiate(trackBuildings[i],new Vector3(xCenter-positions[i].x,0,-positions[i].z), trackBuildings[i].transform.rotation);
			//this.GetComponent<TownGenerator>().BuildOneBuilding (new Vector3(xCenter-positions[i].x,0,yOrigin));
		}
		else
		{
			float currentJumpForce = jumpForce;
			jumper:	float x0=xCenter+globalScales[prevI].x/2;
			
			float g=gravity;
			
			float A = Mathf.Atan2(currentJumpForce,speedToMakeIt);
			float sinA = Mathf.Sin (A);
			float cosA = Mathf.Cos (A);
			float v = Vector3.Magnitude (new Vector3(speedToMakeIt,currentJumpForce,0));
			
			float y0 = Heights[prevJ].Value, y = Heights[iterator].Value;
			float a = 0.5f*g, b = -v*sinA, c=y-y0;
			float D= b*b - 4*a*c;
			if(D<0) 
			{ 
				Debug.Log("uh-oh"); 
				if(currentJumpForce==jumperJumpForce-playerSpeedHelperPercent)
				{
					Debug.Log("we have to return");
					return;
				}
				else
				{
					currentJumpForce=TouchInput.JUMPER_JUMP_SPEED - playerSpeedHelperPercent; 
					prevBuilding.GetComponent<TrackFinder>().track.GetComponent<TrackManager>().BuildJumper(jumper);
					goto jumper ;
				}
			}
			else
			{
				float t = (-b + Mathf.Sqrt (D))/(2*a);
				float x = x0+v*cosA*t;
				xCenter=x+globalScales[i].x/2;
				newBuilding = this.GetComponent<TownGenerator>().MyInstantiate(trackBuildings[i],
				                                                               new Vector3(xCenter-positions[i].x,0,-positions[i].z), trackBuildings[i].transform.rotation, true);


			}
		}

		// testzone

		//EnterTrack
		int trackCount = Mathf.RoundToInt(globalScales[i].y);
		int enterTrack = Random.Range (0,trackCount);
		float yOffset = -(float)trackCount/2f+0.5f+enterTrack;
		newBuilding.transform.position+=new Vector3(0,0,yOffset + yOrigin);
		int exitTrack = FindExitTrack(enterTrack, trackCount);//Random.Range (0,trackCount);
		float yOffsetExit = -(float)trackCount/2f+0.5f+exitTrack;
		float yResultOffset = yOffset-yOffsetExit;
		yOrigin += yResultOffset;
		
		float yPos = positions[i].z+newBuilding.transform.position.z;
//		Debug.Log (yOffset+","+yOffsetExit+","+yOrigin);

		this.GetComponent<TownGenerator>().BuildOneBuilding (new Vector3(xCenter-positions[i].x,0,yOrigin));

		newBuilding.GetComponent<TrackFinder>().track.AddComponent<TrackManager>();
		TrackManager TM = newBuilding.GetComponent<TrackFinder>().track.GetComponent<TrackManager>();
		for(int track=0;track<trackCount;track++)
		{
			float yOffsetTrack = -(float)trackCount/2f+0.5f+track;
			if(track==enterTrack)
			{
				if(showTrackBalls)Instantiate(enter,new Vector3(xCenter,positions[i].y+1,-yOffsetTrack+yPos), jumpWay.transform.rotation);
				TM.enterTrack = -yOffsetTrack+yPos;

			}
			if(track==exitTrack)
			{
				if(showTrackBalls)Instantiate(exit,new Vector3(xCenter,positions[i].y+1,-yOffsetTrack+yPos), jumpWay.transform.rotation);
				TM.exitTrack = -yOffsetTrack+yPos;
			}
			else 
			{
				if(showTrackBalls) Instantiate(jumpWay,new Vector3(xCenter,positions[i].y+1,-yOffsetTrack+yPos), jumpWay.transform.rotation);
			}
			TM.AddRoad (-yOffsetTrack+yPos);
		}
		if(prevI==-1)
		{
			SetPlayerPosition(new Vector3(TM.gameObject.transform.position.x-TM.gameObject.transform.lossyScale.x/2,Heights[iterator].Value+1,TM.exitTrack));
		}

		TM.BuildBolt(bolt);

		prevI=i;
		prevJ=iterator;
		newBuilding.transform.parent=this.gameObject.transform;
		prevBuilding = newBuilding;
//		else
//		{
			//tz

//			float x0=xCenter+globalScales[prevI].x/2;
//			
//			float g=gravity;
//			
//			float A = Mathf.Atan2(jumpForce,speedToMakeIt);
//			float sinA = Mathf.Sin (A);
//			float cosA = Mathf.Cos (A);
//			float v = Vector3.Magnitude (new Vector3(speedToMakeIt,jumpForce,0));
//			
//			float y0 = Heights[prevJ].Value, y = Heights[iterator].Value;
//			//прыжковый путь
//			for(float k=0;k<3;k+=0.1f)
//			{
//				float x1,y1;
//				x1=x0+v*cosA*k;
//				y1=y0+v*sinA*k-0.5f*g*Mathf.Pow(k,2);
//				Instantiate(jumpWay,new Vector3(x1,y1,yPos-yOffsetExit),jumpWay.transform.rotation);
//			}
//		}
		//


		//Instantiate(trackBuildings[i],new Vector3(xCenter-positions[i].x,0,-positions[i].z), trackBuildings[i].transform.rotation);

	}




}
