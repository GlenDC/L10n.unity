using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace L20nUnity
{
	namespace Components
	{
		[AddComponentMenu("L20n/SubmitLocaleAction")]
		public sealed class L20nSubmitLocaleAction : MonoBehaviour {
			[SerializeField] private string m_LocaleIdentifier = null;

			void Start () {
				Debug.Assert(m_LocaleIdentifier != null && m_LocaleIdentifier != "",
				             "<L20nSubmitLocale> requires a local identifier to be specified");
			}

			public void OnSubmit() {
				L20n.SetLocale(m_LocaleIdentifier);
			}
		}
	}
}
