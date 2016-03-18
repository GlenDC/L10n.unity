using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		L20n.Initialize("L20n/Examples/manifest.json", "pt_BR");
	}
}
