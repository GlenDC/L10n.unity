using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using System.Collections.Generic;

namespace L20nUnity
{
	[AddComponentMenu("L20n/Text")]
	public sealed class L20nText : MonoBehaviour{
		public string identifier;
		public bool useVariables;
		public VariableCollection variables;

		private Text m_TextComponent;

		void OnEnable() {
			m_TextComponent = GetComponent<Text>();

			Debug.Assert(
				m_TextComponent != null,
				"<L20nText> requires a <TextComponent> to be attached");
		}
		
		// Update is called once per frame
		void Update () {
			if (identifier == "")
				return;

			if(useVariables)
				m_TextComponent.text = L20n.Translate(identifier, variables.GetVariables());
			else
				m_TextComponent.text = L20n.Translate(identifier);
		}
	}
	
	[Serializable]
	public sealed class VariableCollection {
		public List<String> keys;
		public List<ExternalValue> values;

		public VariableCollection()
		{
			keys = new List<string>();
			values = new List<ExternalValue>();
		}

		public L20nCore.UserVariables GetVariables()
		{
			var variables = new L20nCore.UserVariables();
			for (int i = 0; i < keys.Count; ++i) {
				variables.Add(keys[i], values[i].GetValue());
			}

			return variables;
		}
	}

	public interface IWhatever {}
	
	[Serializable]
	public sealed class ExternalValue {
		public Type type;

		public int literal;
		public string text;
		public UnityEngine.Object hash;

		public ExternalValue()
		{
			type = Type.String;
		}
		
		public L20nCore.UserVariable GetValue()
		{
			switch (type) {
			case Type.Literal:
				return literal;
			case Type.String:
				return text;
			case Type.HashTable:
				var k = hash as IWhatever;
				return null;
			}

			return null;
		}

		public enum Type {
			Literal,
			String,
			HashTable
		}
	}

#if UNITY_EDITOR
	[CustomEditor (typeof (L20nText))]
	[CanEditMultipleObjects]
	public class L20nTextEditor : Editor {
		SerializedProperty identifier;
		SerializedProperty useVariables;
		SerializedProperty variables;	
		
		void OnEnable () {
			identifier = serializedObject.FindProperty ("identifier");
			useVariables = serializedObject.FindProperty ("useVariables");
			variables = serializedObject.FindProperty ("variables");
		}
		
		public override void OnInspectorGUI() {
			serializedObject.Update();

			EditorGUILayout.PropertyField(identifier);
			EditorGUILayout.PropertyField(useVariables);

			if(useVariables.boolValue)
				EditorGUILayout.PropertyField(variables);

			serializedObject.ApplyModifiedProperties();
		}
	}

	[CustomPropertyDrawer(typeof(VariableCollection))]
	public class VariableCollectionDrawer : PropertyDrawer {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			EditorGUI.LabelField(position, "External Variables");

			var keys = property.FindPropertyRelative("keys");
			var values = property.FindPropertyRelative("values");

			if(GUILayout.Button("Add Value")) {
				keys.InsertArrayElementAtIndex(keys.arraySize);
				values.InsertArrayElementAtIndex(values.arraySize);
			}

			for (int i = 0; i < keys.arraySize; ++i) {
				EditorGUILayout.Separator();

				EditorGUILayout.BeginHorizontal();

				if(GUILayout.Button("delete")) {
					keys.DeleteArrayElementAtIndex(i);
					values.DeleteArrayElementAtIndex(i);
					break;
				}

				EditorGUILayout.PropertyField(keys.GetArrayElementAtIndex(i), GUIContent.none);
				EditorGUILayout.EndHorizontal();

				
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PropertyField(values.GetArrayElementAtIndex(i), GUIContent.none);
				EditorGUILayout.EndHorizontal();
			}
		}
	}
	
	[CustomPropertyDrawer(typeof(ExternalValue))]
	public class ExternalValueDrawer : PropertyDrawer {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			var type = property.FindPropertyRelative("type");
			EditorGUILayout.PropertyField(type, GUIContent.none);

			string field = null;
			switch ((ExternalValue.Type)type.enumValueIndex) {
				case ExternalValue.Type.Literal: {
					var value = property.FindPropertyRelative("literal");
					EditorGUILayout.PropertyField(value, GUIContent.none);
					break;
				}

				case ExternalValue.Type.String: {
					var value = property.FindPropertyRelative("text");
					EditorGUILayout.PropertyField(value, GUIContent.none);
					break;
				}

				case ExternalValue.Type.HashTable: {
					var hash = property.FindPropertyRelative("hash");
					hash.objectReferenceValue = EditorGUILayout.ObjectField(
						hash.objectReferenceValue, typeof (L20nCore.External.UserHashValue), true);
					break;
				}
			}
		}
	}
#endif
}