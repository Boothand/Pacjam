using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PHPRead : MonoBehaviour
{

	Text tx;



	// Use this for initialization
	void Start()
	{
		tx = GetComponent<Text>();
		Refresh();
	}

	public void Refresh()
	{
		string url = "http://worldofaxaka.com/wobbly/read.php";
		//enable under on WebGL build
		//url = "read.php";

		tx.text = "Loading...";
		WWW www = new WWW(url);

		StartCoroutine(WaitForRequest(www));
	}

	IEnumerator WaitForRequest(WWW www)
	{
		yield return www;

		//check for errors
		if (www.error == null)
		{
			//Debug.Log("WWW Ok!: " + www.text);
			tx.text = www.text;
		}
		else
		{
			//Debug.Log("WWW Error: " + www.error);
			tx.text = www.error;
		}
	}


}