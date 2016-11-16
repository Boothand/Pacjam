using UnityEngine;
using System.Collections;
using UnityEditor;

[ExecuteInEditMode]
public class SlabHelper : MonoBehaviour
{
	void Update()
	{
		if (Application.isPlaying) return;
		GetComponent<SpriteRenderer>().enabled = (GetComponentInParent<Slab>().candy != null);
	}

	void Start()
	{
		GetComponent<SpriteRenderer>().enabled = false;
	}

}
