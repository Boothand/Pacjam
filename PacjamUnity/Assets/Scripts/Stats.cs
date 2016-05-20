using UnityEngine;

public class Stats : MonoBehaviour
{
	[SerializeField] int score;

	public int Score
	{
		get { return score; }
		set
		{
			value = Mathf.Clamp(value, 0, 10000);
			score = value;
		}
	}
	
	void Start ()
	{
		
	}
	
	void Update ()
	{
		
	}
}