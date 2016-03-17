using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("L20n/SubmitLocale")]
public sealed class L20nSubmitLocale : MonoBehaviour {
	[SerializeField] private string m_LocaleIdentifier = null;

	void Start () {
		Debug.Assert(m_LocaleIdentifier != null && m_LocaleIdentifier != "",
		             "<L20nSubmitLocale> requires a local identifier to be specified");
	}

	public void OnSubmit() {
		L20n.SetLocale(m_LocaleIdentifier);
	}
}
