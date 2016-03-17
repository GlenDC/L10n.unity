using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		L20n.Initialize("Assets/L20n/examples/manifest.json", "pt-BR");
	}
}
