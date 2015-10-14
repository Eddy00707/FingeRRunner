using UnityEngine;
using System.Collections;

public class TouchEvents : MonoBehaviour 
{
	public GameObject townBuilder;

	public void OnRestartEasyButtonClick()
	{
		Restart(true);
	}

	public void OnRestartHardButtonClick()
	{
		Restart(false);
	}

	void Restart(bool easyMode)
	{
		Debug.Log ("Restarting now!");
		Time.timeScale=1;
		this.transform.position=Vector3.zero;
		this.GetComponent<TouchInput>().StartNewGame(easyMode);
		townBuilder.GetComponent<TrackGenerator>().StartNewGame();
		townBuilder.GetComponent<TownGenerator>().StartNewGame();
	}


}
