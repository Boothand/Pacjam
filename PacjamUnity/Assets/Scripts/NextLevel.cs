using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
#if UNITY_EDITOR
	public UnityEngine.Object scene;
#endif

	[HideInInspector]
	[SerializeField]
	public string sceneName = "";

#if UNITY_EDITOR
	public void OnValidate()
	{
		sceneName = "";

		if (scene != null)
		{
			if (scene.ToString().Contains("(UnityEngine.SceneAsset)"))
			{
				sceneName = scene.name;
			}
			else
			{
				scene = null;
			}
		}
	}
#endif
}
