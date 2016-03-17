using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class L20nText : MonoBehaviour {

	public string Identifier;
	private Text m_TextComponent;

	void Start () {
		m_TextComponent = GetComponent<Text>();
		Debug.Assert(m_TextComponent != null, "<L20nText> requires a <TextComponent> to be attached");
	}
	
	// Update is called once per frame
	void Update () {
		m_TextComponent.text = L20n.Translate(Identifier);
	}
}
