using UnityEngine;

/// <summary>
/// A simple script that loads the first scene.
/// All the loading happens at Awake, so we can just continue at Start.
/// </summary>
public class GameLoader : MonoBehaviour
{
	[SerializeField] string firstScene;

	void Start ()
	{
		Application.LoadLevel(firstScene);
	}
}
