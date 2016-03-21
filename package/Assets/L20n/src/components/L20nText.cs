using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using System.Collections.Generic;

namespace L20nUnity
{
	namespace Components
	{
		[AddComponentMenu("L20n/Text")]
		public sealed class L20nText : MonoBehaviour{
			public string identifier;
			public bool useVariables;
			public Internal.VariableCollection variables;

			#if UI_NGUI
			private UILabel m_TextComponent;
			#else
			private Text m_TextComponent;
			#endif

			void OnEnable() {
				Debug.Assert(
					identifier != "",
					"<L20nText> requires an <identifier> to be givn");

				#if UI_NGUI
				m_TextComponent = GetComponent<UILabel>();
				#else
				m_TextComponent = GetComponent<Text>();
				#endif
				Debug.Assert(
					m_TextComponent != null,
					"<L20nText> requires a <TextComponent> to be attached");
			}
			
			// Update is called once per frame
			void Update () {
				if (identifier == "")
					return;

				if(useVariables)
					m_TextComponent.text = L20n.Translate(identifier,
						variables.keys.ToArray(), variables.GetValues());
				else
					m_TextComponent.text = L20n.Translate(identifier);
			}
		}

		namespace Internal
		{
			[Serializable]
			public sealed class VariableCollection {
				public List<String> keys;
				public List<ExternalValue> values;

				public VariableCollection()
				{
					keys = new List<string>();
					values = new List<ExternalValue>();
				}

				public L20nCore.Objects.L20nObject[] GetValues()
				{
					var output = new L20nCore.Objects.L20nObject[values.Count];
					for(int i = 0; i < output.Length; ++i) {
						output[i] = values[i].GetValue();
					}

					return output;
				}
			}
			
			[Serializable]
			public sealed class ExternalValue {
				public Type type;

				public int literal;
				public string text;
				public HashValueBehaviour hash;

				public ExternalValue()
				{
					type = Type.String;
				}
				
				public L20nCore.Objects.L20nObject GetValue()
				{
					switch (type) {
					case Type.Literal:
						return new L20nCore.Objects.Literal(literal);
					case Type.String:
						return new L20nCore.Objects.StringOutput(text);
					case Type.HashValue:
						if(hash == null) {
							return null;
						}

						return new L20nCore.Objects.Entity(hash);
					}

					return null;
				}

				public enum Type {
					Literal,
					String,
					HashValue
				}
			}

		#if UNITY_EDITOR
			[CustomEditor (typeof (L20nText))]
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

						case ExternalValue.Type.HashValue: {
							var hash = property.FindPropertyRelative("hash");
							hash.objectReferenceValue = EditorGUILayout.ObjectField(
								hash.objectReferenceValue, typeof (HashValueBehaviour), true);
							break;
						}
					}
				}
			}
		#endif
		}
	}
}