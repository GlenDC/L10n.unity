using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using System.Collections.Generic;

using UserVariable = L20nCore.UserVariable;

namespace L20nUnity
{
	[AddComponentMenu("L20n/Text")]
	public sealed class L20nText : MonoBehaviour {
		public string Identifier = "";
		public List<string> Values = new List<string>();
		public List<string> Keys = new List<string>();

		private Text m_TextComponent = null;
		private L20nCore.UserVariables m_UserVariables = null;

		void OnEnable()
		{
			m_UserVariables = new L20nCore.UserVariables(Keys.Count);
			for(int i = 0; i < Keys.Count; ++i) {
				if(Values[i] != null && Keys[i] != null) {
					m_UserVariables.Add(Keys[i], Values[i]);
				}
			}
			
			m_TextComponent = GetComponent<Text>();

			Debug.Assert(
				m_TextComponent != null,
				"<L20nText> requires a <TextComponent> to be attached");
			Debug.Assert(
				Identifier != null && Identifier != "",
				"<L20nText> requires a <string-identifier> to be attached");
		}
		
		// Update is called once per frame
		void Update () {
			if(Identifier == "") return;

			if (m_UserVariables.Count > 0) {
				m_TextComponent.text = L20n.Translate(Identifier, m_UserVariables);
			} else {
				m_TextComponent.text = L20n.Translate(Identifier);
			}
		}
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(L20nText))]
	[CanEditMultipleObjects]
	public class L20nTextEditor : Editor
	{	
		L20nText m_Target;
		
		public void OnEnable()
		{
			m_Target = target as L20nText;
		}
		
		public override void OnInspectorGUI()
		{
			m_Target.Identifier = EditorGUILayout.TextField(
				"Identifier", m_Target.Identifier);

			var text = m_Target.gameObject.GetComponent<Text>();
			if (text != null)
				text.text = m_Target.Identifier;

			EditorGUILayout.LabelField("User Variables");

			EditorGUILayout.BeginHorizontal();

			if (m_Target.Keys.Count > 0) {
				if (GUILayout.Button ("-")) {
					m_Target.Keys.RemoveAt (0);
					m_Target.Values.RemoveAt (0);
				}
				else if (GUILayout.Button ("+")) {
					m_Target.Keys.Insert (0, null);
					m_Target.Values.Insert (0, null);
				}
			}

			EditorGUILayout.EndHorizontal();

			for(int i = 0; i < m_Target.Keys.Count; ++i) {
				EditorGUILayout.BeginHorizontal();

				if(GUILayout.Button("-")) {
					m_Target.Keys.RemoveAt(i);
					m_Target.Values.RemoveAt(i);
					break;
				}

				m_Target.Keys[i] = EditorGUILayout.TextField(m_Target.Keys[i]);
				m_Target.Values[i] = EditorGUILayout.TextField(m_Target.Values[i]);

				EditorGUILayout.EndHorizontal();
			}

			EditorGUILayout.BeginHorizontal();
			
			if (m_Target.Keys.Count > 0 && GUILayout.Button ("-")) {
				var i = m_Target.Keys.Count - 1;
				m_Target.Keys.RemoveAt(i);
				m_Target.Values.RemoveAt(i);
			}
			if (GUILayout.Button ("+")) {
				m_Target.Keys.Add(null);
				m_Target.Values.Add(null);
			}

			EditorGUILayout.EndHorizontal();
		}
	}
#endif
}
