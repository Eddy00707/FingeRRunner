using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TapManager : MonoBehaviour {

	public Text testText, begin,end;

	public GameObject movingWorld;

	public float playerSpeed;

	List<Vector2> touchDeltas;

	const float Y_THRESHOLD=20, TAP_THRESHOLD=10;

	// Use this for initialization
	void Start () 
	{
		touchDeltas = new List<Vector2>();
	}
	
	// Update is called once per frame
	void Update () 
	{

		if(Input.touchCount>0)
		{
			for(int i=0;i<Input.touchCount;i++)
			{
				Touch touch = Input.GetTouch(i);
				if(touch.phase== TouchPhase.Began)
				{
					touchDeltas.Add(Vector2.zero);
				}
				else if(touch.phase== TouchPhase.Moved)
				{
					touchDeltas[i]=touchDeltas[i]+touch.deltaPosition;
					RecognizeJesture();
				}
				else if(touch.phase== TouchPhase.Ended)
				{
					if(Input.touchCount==2)
					{
						RecognizeJump();
					}
					touchDeltas.RemoveAt(i);
					i--;
				}
				begin.text=touchDeltas[i].ToString();

			}
			testText.text = (  Input.touches[0].phase.ToString());
		}
	}

	void MovePlayer (float swipe)
	{
		if(swipe<0)
		{
			Vector3 pos = movingWorld.transform.position;
			Vector3 endPosition = new Vector3(pos.x,pos.y,pos.z + swipe);
			movingWorld.transform.position= endPosition;
		}

	}


	void RecognizeJesture ()
	{
		if(touchDeltas.Count>2)
		{
			end.text = "Nothing";
		}
		else
		{
			if(touchDeltas.Count==2)
			{
				float y1 =touchDeltas[0].y, y2=touchDeltas[1].y;
				float yDifference = Mathf.Abs(y1-y2);
				if(y1>TAP_THRESHOLD||y2>TAP_THRESHOLD)
				{

					
					if(yDifference<Y_THRESHOLD)
					{
						end.text="double swipe";
					}
					else 
					{
						end.text="running";
						MovePlayer((y1+y2)*playerSpeed);
					}
				}
			}
			else if(touchDeltas.Count==1)
			{
				end.text="running";
				MovePlayer((touchDeltas[0].y)*playerSpeed);
			}
		}
	}

	void RecognizeJump ()
	{
		float y1 =touchDeltas[0].y, y2=touchDeltas[1].y;
		if(y1<TAP_THRESHOLD&&y2<TAP_THRESHOLD)
		{
			end.text="jump";
		}
	}
}
