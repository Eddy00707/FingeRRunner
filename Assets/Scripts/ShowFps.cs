using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowFps : MonoBehaviour {
	
	string label = "";
	float count;

	public Text fpsText;

	IEnumerator Start ()
	{
		///GUI.depth = 2;
		while (true) 
		{
			if (Time.timeScale == 1) 
			{
				yield return new WaitForSeconds (0.1f);
				count = (1 / Time.deltaTime);
				label = "FPS :" + (Mathf.Round (count));
				fpsText.text=label;
			} 
			else 
			{
				label = "Pause";
			}
			yield return new WaitForSeconds (0.5f);
		}
	}
	

}