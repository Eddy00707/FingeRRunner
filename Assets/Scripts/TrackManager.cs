using UnityEngine;
using System.Collections.Generic;

public class TrackManager : MonoBehaviour 
{

	List<float> zRoadCenters = new List<float>();

	//public static GameObject jumper;

	public float enterTrack, exitTrack;
	// Use this for initialization
	void Start () 
	{
		
	}


	public void AddRoad(float center)
	{
		zRoadCenters.Add (center);
	}


	public void BuildBolt(GameObject bolt)
	{
		Vector3 boltPosition = new Vector3(this.transform.position.x, this.transform.position.y,exitTrack);
		GameObject newBolt=(GameObject) Instantiate(bolt, boltPosition, bolt.transform.rotation);
		newBolt.transform.parent=this.transform;
		Vector3 l=this.transform.localScale;


	}


	public void BuildJumper(GameObject jumper)
	{
		Vector3 jumperPosition = new Vector3(this.transform.position.x+this.transform.lossyScale.x/2-1.5f,this.transform.position.y,exitTrack);
		GameObject newJumper = (GameObject) Instantiate (jumper, jumperPosition, jumper.transform.rotation);
		newJumper.transform.parent = this.transform;
	}

	public void GetMinAndMaxRoads(out float min, out float max)
	{
		min = zRoadCenters[0];
		max=zRoadCenters[zRoadCenters.Count-1];
	}

	public string ShowRoads()
	{
		string str ="";
		for(int i=0;i<zRoadCenters.Count;i++)
		{
			str+=zRoadCenters[i].ToString();
			str+=", ";
		}
		return str+". enter track="+enterTrack+". exit track="+exitTrack;
	}

}
