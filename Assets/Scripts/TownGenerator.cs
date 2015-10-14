using UnityEngine;
using System.Collections;

public class TownGenerator : MonoBehaviour 
{
	public bool buildThem;
	public GameObject[] buildings;
	public GameObject ground;
	float groundX;
	// Use this for initialization
	void Start () 
	{
		StartNewGame();
		//BuildGround();
	}

	public void StartNewGame()
	{
		groundX=0;
	}

	public void BuildGround()
	{

		for(int y=-150;y<=150;y+=100)
		{
			GameObject newGround =(GameObject) Instantiate (ground,new Vector3(groundX,0,y), ground.transform.rotation);
			newGround.transform.parent=this.transform;

		}
		groundX+=100;

	}


	public void BuildOneBuilding(Vector3 origin)
	{
		if(buildThem)
		{
			while(origin.x>groundX-500) BuildGround ();
			StartCoroutine (BuildEm(origin));

		}
	}

	IEnumerator BuildEm(Vector3 origin)
	{
		//for(int newY=10;newY<6000;newY++){
		int newY=20;
		int plusX=30;
		int randomN = Random.Range (0,buildings.Length);
		BoxCollider BC = buildings[randomN].GetComponent<BoxCollider>();
		Vector3 center = BC.center;
		float extent = buildings[randomN].transform.TransformPoint(BC.size).z;
		center = buildings[randomN].transform.TransformPoint(center);
		center.y=0;
		GameObject newBuilding = (GameObject) Instantiate (buildings[randomN], new Vector3(origin.x+plusX,0,origin.z+ newY-extent/2)-center, buildings[randomN].transform.rotation);
		newBuilding.transform.parent=this.transform;

		yield return new WaitForSeconds(0.3f);

		randomN = Random.Range (0,buildings.Length);
		BC = buildings[randomN].GetComponent<BoxCollider>();
		center = BC.center;
		extent = buildings[randomN].transform.TransformPoint(BC.size).z;
		center = buildings[randomN].transform.TransformPoint(center);
		center.y=0;
		newBuilding = (GameObject) Instantiate (buildings[randomN], new Vector3(origin.x+plusX,0,origin.z-newY+extent/2)-center, buildings[randomN].transform.rotation);
		newBuilding.transform.parent=this.transform;
		yield return new WaitForSeconds(0.3f);


	}

}
