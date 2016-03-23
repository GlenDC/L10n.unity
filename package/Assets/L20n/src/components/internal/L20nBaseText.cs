/**
 * This source file is part of the Commercial L20n Unity Plugin.
 * 
 * Copyright (c) 2016 Glen De Cauwsemaecker (contact@glendc.com)
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *    http://www.apache.org/licenses/LICENSE-2.0

 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using System.Collections.Generic;

namespace L20nUnity
{
	namespace Components
	{
		namespace Internal
		{
			public abstract class L20nBaseText : MonoBehaviour {
				public string identifier;
				public bool useVariables;
				public Internal.VariableCollection variables;
				
				void OnEnable() {
					Debug.Assert(
						identifier != "",
						"<L20nText> requires an <identifier> to be givn");
					Initialize();
				}
				
				// Update is called once per frame
				void Update () {
					if (identifier == "")
						return;
					
					if (useVariables)
						SetText (L20n.Translate (identifier,
						                       variables.keys.ToArray (), variables.GetValues ()));
					else
						SetText (L20n.Translate (identifier));
				}

				protected abstract void Initialize();
				public abstract void SetText(string text);
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
				[CustomEditor (typeof (L20nBaseText))]
				public class L20nBaseTextEditor : Editor {
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
						
						if (!Application.isPlaying) {
							var text = identifier.stringValue;
							if (text == "") {
								text = "<MISSING IDENTIFIER>";
							} else if (useVariables.boolValue) {
								text += "*";
							}
							
							(target as L20nBaseText).SetText (text);
						}

						if (useVariables.boolValue) {
							var size = variables.FindPropertyRelative("keys").arraySize;
							EditorGUILayout.BeginHorizontal(
								GUILayout.MinHeight(30 + size * 30 + (size - 1) * 20));
							EditorGUILayout.PropertyField (variables);
							EditorGUILayout.EndHorizontal();
						}
						
						serializedObject.ApplyModifiedProperties();
					}
				}
				
				[CustomPropertyDrawer(typeof(VariableCollection))]
				public class VariableCollectionDrawer : PropertyDrawer {
					Rect Offset(Rect position, float sx, float sy, float sw, float sh) {
						return new Rect(position.x + (position.width * sx),
						                position.y + (position.height * sy),
						                position.width * sw, position.height * sh);
					}

					public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
						var keys = property.FindPropertyRelative("keys");
						var values = property.FindPropertyRelative("values");

						var labelRect = new Rect(position.x, position.y, 120, position.height);
						EditorGUI.LabelField(labelRect, "External Variables");
						
						var btnRect = new Rect(
							position.x + labelRect.width,
							position.y,
							position.width - labelRect.width,
							position.height);

						if(GUI.Button(btnRect, new GUIContent("Add Variable", "add an external variable"))) {
							keys.InsertArrayElementAtIndex(keys.arraySize);
							values.InsertArrayElementAtIndex(values.arraySize);
						}
			
						position = Offset(position, 0, 1.50f, 1, 1);

						for (int i = 0; i < keys.arraySize; ++i) {
							var deleteRect = Offset(position, 0, 0, .2f, 1f);
							if(GUI.Button(deleteRect, new GUIContent("delete", "delete this external variable"))) {
								keys.DeleteArrayElementAtIndex(i);
								values.DeleteArrayElementAtIndex(i);
								break;
							}
							
							var keyRect = Offset(position, .21f, 0, .79f, 1f);
							EditorGUI.PropertyField(keyRect, keys.GetArrayElementAtIndex(i), GUIContent.none);
							
							var valueRect = Offset(position, 0, 1.25f, 1f, 1f);
							EditorGUI.PropertyField(valueRect, values.GetArrayElementAtIndex(i), GUIContent.none);
							
							position = Offset(position, 0, 3.0f, 1, 1);
						}
					}
				}
				
				[CustomPropertyDrawer(typeof(ExternalValue))]
				public class ExternalValueDrawer : PropertyDrawer {
					Rect Offset(Rect position, float sx, float sy, float sw, float sh) {
						return new Rect(position.x + (position.width * sx),
						                position.y + (position.height * sy),
						                position.width * sw, position.height * sh);
					}

					public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
						var type = property.FindPropertyRelative("type");
						
						var typeRect = Offset(position, 0, 0, 0.28f, 1f);
						EditorGUI.PropertyField(typeRect, type, GUIContent.none);
						
						var valueRect = Offset(position, 0.29f, 0, 0.71f, 1f);
						switch ((ExternalValue.Type)type.enumValueIndex) {
						case ExternalValue.Type.Literal: {
							var value = property.FindPropertyRelative("literal");
							EditorGUI.PropertyField(valueRect, value, GUIContent.none);
							break;
						}
							
						case ExternalValue.Type.String: {
							var value = property.FindPropertyRelative("text");
							EditorGUI.PropertyField(valueRect, value, GUIContent.none);
							break;
						}
							
						case ExternalValue.Type.HashValue: {
							var hash = property.FindPropertyRelative("hash");
							hash.objectReferenceValue = EditorGUI.ObjectField(valueRect,
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
}