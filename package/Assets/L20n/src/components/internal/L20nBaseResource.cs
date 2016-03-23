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

using L20nCore.Utils;

namespace L20nUnity
{
	namespace Components
	{
		namespace Internal
		{
			public abstract class L20nBaseResource<T, U> : MonoBehaviour
				where T: UnityEngine.Object
				where U: L20nResourceCollection<T>
			{
				public U resources;
				public T defaultResource;
				
				void OnEnable() {
					L20n.OnLocaleChange += OnLocaleChange;
					Initialize();
				}
				
				void OnDisable() {
					L20n.OnLocaleChange -= OnLocaleChange;
				}
				
				public void OnLocaleChange()
				{
					SetResource(resources.GetResource(L20n.CurrentLocale)
					          .UnwrapOr(defaultResource));
					
				}
				
				protected abstract void Initialize();
				public abstract void SetResource(T resource);
			}

			[Serializable]
			public class L20nResourceCollection<T> {
				public List<String> keys;
				public List<T> values;
				
				public L20nResourceCollection()
				{
					keys = new List<string>();
					values = new List<T>();
				}
				
				public Option<T> GetResource(string key)
				{
					var result = new Option<T>();
					
					var count = Math.Min(keys.Count, values.Count);
					for(int i = 0; i < count; ++i) {
						if(keys[i].Equals(key)) {
							result.Set(values[i]);
							break;
						}
					}
					
					return result;
				}
			}
			
			#if UNITY_EDITOR
			public class L20nBaseResourceEditor : Editor {
				SerializedProperty resources;
				SerializedProperty defaultResource;
				
				void OnEnable () {
					resources = serializedObject.FindProperty ("resources");
					defaultResource = serializedObject.FindProperty ("defaultResource");
				}
				
				public override void OnInspectorGUI() {
					serializedObject.Update();

					EditorGUILayout.LabelField("Default");
					EditorGUILayout.PropertyField(defaultResource, GUIContent.none);

					EditorGUILayout.Space();
					
					EditorGUILayout.PropertyField(resources);
					
					serializedObject.ApplyModifiedProperties();
				}
			}

			public class L20nResourceCollectionDrawer : PropertyDrawer {
				public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
					EditorGUI.LabelField(position, "Other Locales");
					
					var keys = property.FindPropertyRelative("keys");
					var values = property.FindPropertyRelative("values");
					
					if(GUILayout.Button("Add Localized Resource")) {
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
			#endif
		}
	}
}