using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("L20n/Text")]
public sealed class L20nText : MonoBehaviour {
	[SerializeField] private string m_Identifier;
	private Text m_TextComponent;

	void Start () {
		m_TextComponent = GetComponent<Text>();
		Debug.Assert(
			m_TextComponent != null,
			"<L20nText> requires a <TextComponent> to be attached");
		Debug.Assert(
			m_Identifier != null && m_Identifier != "",
			"<L20nText> requires a <string-identifier> to be attached");
	}
	
	// Update is called once per frame
	void Update () {
		m_TextComponent.text = L20n.Translate(m_Identifier);
	}
}
