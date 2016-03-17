using UnityEngine;
using System.Collections;

namespace L20nExamples
{
	public class Test : MonoBehaviour
	{
		// Use this for initialization
		void Start () {
			L20n.Initialize("Assets/L20n/examples/manifest.json", "pt-BR");
			Debug.Log(L20n.Translate("enjoy"));
		}
		
		// Update is called once per frame
		void Update () {
		
		}
	}
}
