using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections.Generic;

using UserVariable = L20nCore.UserVariable;

namespace L20nUnity
{
	[AddComponentMenu("L20n/Text")]
	public sealed class L20nText : MonoBehaviour {
		[SerializeField] private string m_Identifier;
		[SerializeField] private List<L20nVariable> m_Variables = new List<L20nVariable>();

		private Text m_TextComponent;
		private L20nCore.UserVariables m_UserVariables;

		void Start () {
			m_TextComponent = GetComponent<Text>();
			Debug.Assert(
				m_TextComponent != null,
				"<L20nText> requires a <TextComponent> to be attached");
			Debug.Assert(
				m_Identifier != null && m_Identifier != "",
				"<L20nText> requires a <string-identifier> to be attached");

			m_UserVariables = new L20nCore.UserVariables();
			for(int i = 0; i < m_Variables.Count; ++i)
				m_UserVariables.Add(m_Variables[i].Key, m_Variables[i].Value);
			m_Variables = null;


		}
		
		// Update is called once per frame
		void Update () {
			if (m_UserVariables.Count > 0) {
				m_TextComponent.text = L20n.Translate(m_Identifier, m_UserVariables);
			} else {
				m_TextComponent.text = L20n.Translate(m_Identifier);
			}
		}
	}

	[Serializable]
	class L20nVariable
	{
		public string Key { get { return m_Key; } }
		public UserVariable Value
		{
			get
			{
				if(m_Value.Variable != null)
					return m_Value.Variable;
				if(m_Value.Text != null && m_Value.Text != "")
					return m_Value.Text;
				return m_Value.Literal;
			}
		}

		[SerializeField] private string m_Key;
		[SerializeField] private L20nVariableValue m_Value;
	}
	
	
	[Serializable]
	class L20nVariableValue
	{
		public int Literal = 0;
		public string Text = "";
		public L20nCore.External.UserHashValue Variable = null;
	}
}
