using UnityEngine;
using System.Collections;

public class Visibility : MonoBehaviour {

	Vector3 position;

	void Start()
	{
		position = this.transform.position;
		StartCoroutine(CheckVisibility());
	}

	IEnumerator CheckVisibility()
	{
		while(true)
		{
			Vector3 playerPosition = TouchInput.playerPosition;
			if   ((playerPosition.x>position.x-30&&  this.gameObject.tag=="BuildingProp") // for props
			    ||(playerPosition.x>position.x+40&&  this.gameObject.tag=="Building") //for track
			    ||(playerPosition.x>position.x+100)&&this.gameObject.tag=="Ground") //for ground
			{
				Destroy(this.gameObject);
				StopCoroutine("CheckVisibility");
			}
			yield return new WaitForSeconds(2f);
		}
	}


}
