using UnityEngine;
using System.Collections;
using System.Threading;

public class TownGenerator : MonoBehaviour 
{
	public Material EnergyMaterial;

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
		//StopAllCoroutines();
		groundX=0;
	}

	public void BuildGround()
	{

		for(int y=-200;y<=200;y+=200)
		{
			GameObject newGround =(GameObject) Instantiate (ground,new Vector3(groundX,0,y), ground.transform.rotation);
			newGround.transform.parent=this.transform;

		}
		groundX+=200;

	}


	public void BuildOneBuilding(Vector3 origin)
	{
		if(buildThem)
		{
			while(origin.x>groundX-500)
			{
				BuildGround ();
			}
			StartCoroutine (BuildEm(origin));

		}
	}

	IEnumerator BuildEm(Vector3 origin)
	{
		//for(int newY=15;newY<25;newY+=10){
		int newY=15;
		int plusX=40;
		int randomN = Random.Range (0,buildings.Length);
		BoxCollider BC = buildings[randomN].GetComponent<BoxCollider>();
		Vector3 center = BC.center;
		float extent = buildings[randomN].transform.TransformPoint(BC.size).z;
		center = buildings[randomN].transform.TransformPoint(center);
		center.y=0;
		GameObject newBuilding =MyInstantiate (buildings[randomN], new Vector3(origin.x+plusX,0,origin.z+ newY-extent/2)-center, buildings[randomN].transform.rotation,false);
		newBuilding.transform.parent=this.transform;

		yield return new WaitForSeconds(0.3f);

		randomN = Random.Range (0,buildings.Length);
		BC = buildings[randomN].GetComponent<BoxCollider>();
		center = BC.center;
		extent = buildings[randomN].transform.TransformPoint(BC.size).z;
		center = buildings[randomN].transform.TransformPoint(center);
		center.y=0;
		newBuilding = MyInstantiate (buildings[randomN], new Vector3(origin.x+plusX,0,origin.z-newY+extent/2)-center, buildings[randomN].transform.rotation,false);
		newBuilding.transform.parent=this.transform;
		yield return new WaitForSeconds(0.3f);


	}

	public GameObject MyInstantiate(GameObject obj, Vector3 position, Quaternion rotation, bool track)
	{
		GameObject newBuilding =(GameObject) Instantiate(obj,position,rotation);

		MeshRenderer[] childrenMR = newBuilding.GetComponentsInChildren<MeshRenderer>();
		for(int i=0;i<childrenMR.Length;i++)
		{
			childrenMR[i].enabled=false;
		}
		//newBuilding.SetActive (false);
		StartCoroutine(Summon(childrenMR,newBuilding, track));

		return newBuilding;
	}

    IEnumerator Summon(MeshRenderer[] childrenMR,GameObject newBuilding,bool track)
	{
		for(int i=0;i<childrenMR.Length;i++)
		{
			if(!childrenMR[i]) continue;
			Material temp = childrenMR[i].material;
			childrenMR[i].material= EnergyMaterial;
			childrenMR[i].enabled=true;
			float time;
			if(track) time = 2f/childrenMR.Length;
			else time =3f/childrenMR.Length;
			yield return new WaitForSeconds(time);
			if(!childrenMR[i]) continue;
			childrenMR[i].material= temp;
		}
	

	}



}
