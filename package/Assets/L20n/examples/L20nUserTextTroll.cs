using UnityEngine;
using System.Collections;

public class L20nUserTextTroll : MonoBehaviour {
	// Use this for initialization
	void Update () {
		var name = L20n.Translate("troll");
		GetComponent<L20nUnity.Components.L20nUIText>().SetVariable("user", name);
	}
}
