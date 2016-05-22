using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

public class PHPWrite : MonoBehaviour
{

	private static string salt = "rxjH36OD2HJP41oGsmq06IyVwkpa0ePS";

	private bool success = false;

	public string sendState = "";

	public bool Submit(string name, int score)
	{
		if (name.Length > 0)
		{
			if (score > 0)
			{
				Send(name, score);
				sendState = "Sending...";
				return true;
			}
			else
			{
				return true;
			}
		}
		else
		{
			sendState = "You need to enter a name";
		}
		return false;
	}

	IEnumerator WaitForRequest(WWW www)
	{
		yield return www;

		//check for errors

		if (www.error == null)
		{
			Debug.Log("WWW Ok!: " + www.text);
			//sendState = www.text;
			sendState = "Success!";
			success = true;
		}
		else
		{
			Debug.Log("WWW Error: " + www.error);
			//sendState = www.error;
			sendState = "Could not Complete! Try Again.";
		}

	}

	void Send(string User, int Score)
	{
		if (success)
			return;

		if (sendState == "Sending...")
			return;

		string uncoded = salt + User + Score.ToString();
		//string uncoded = "123";
		string code = CalculateMD5Hash(uncoded);
		//Debug.Log (uncoded);

		string url = "http://worldofaxaka.com/wobbly/write.php?user=" + User + "&score=" + Score.ToString() + "&code=" + code;
		//enable under on WebGL build
		//url = "write.php?user=" + User + "&score=" + Score.ToString() + "&code=" + code;

		//Debug.Log (url);
		WWW www = new WWW(url);

		StartCoroutine(WaitForRequest(www));
	}

	string CalculateMD5Hash(string input)
	{
		//calculate MD5 hash from input
		MD5 md5 = System.Security.Cryptography.MD5.Create();
		byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
		byte[] hash = md5.ComputeHash(inputBytes);

		//convert byte array to hex string
		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < hash.Length; i++)
		{
			sb.Append(hash[i].ToString("x2"));
		}
		return sb.ToString();
	}
}
