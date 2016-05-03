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
			[DisallowMultipleComponent]
			public abstract class L20nBaseResource<T, U> : MonoBehaviour
				where T: UnityEngine.Object
				where U: L20nResourceCollection<T>
			{
				[SerializeField]
				U
					resources;
				[SerializeField]
				T
					defaultResource;
				
				public bool SetResource (string key, T value)
				{
					if (key == null) {
						Debug.LogWarning ("tried to assign a resource to a <L20nBaseResource> with null as a key", this);
						return false;
					}

					return resources.SetResource (key, value);
				}

				public void SetDefaultResource (T value)
				{
					defaultResource = value;
				}
				
				public void OnLocaleChange ()
				{
					SetResource (resources.GetResource (L20n.CurrentLocale)
					          .UnwrapOr (defaultResource));	
				}

				void OnEnable ()
				{
					L20n.OnLocaleChange += OnLocaleChange;
					Initialize ();
					OnLocaleChange ();
				}
				
				void OnDisable ()
				{
					L20n.OnLocaleChange -= OnLocaleChange;
				}

				void OnBecameVisible ()
				{
					enabled = true;
				}
				
				void OnBecameInvisible ()
				{
					enabled = false;
				}
				
				protected abstract void Initialize ();

				public abstract void SetResource (T resource);
			}

			[Serializable]
			public class L20nResourceCollection<T>
			{
				[SerializeField]
				List<String>
					keys;
				[SerializeField]
				List<T>
					values;

				public int Count {
					get { return Math.Min (keys.Count, values.Count); }
				}
				
				public L20nResourceCollection ()
				{
					keys = new List<string> ();
					values = new List<T> ();
				}
				
				public Option<T> GetResource (string key)
				{
					var result = new Option<T> ();
					
					var count = Count;
					for (int i = 0; i < count; ++i) {
						if (keys [i].Equals (key)) {
							result.Set (values [i]);
							break;
						}
					}
					
					return result;
				}

				public bool SetResource (string key, T value)
				{
					var count = Count;
					for (int i = 0; i < count; ++i) {
						if (keys [i].Equals (key)) {
							values [i] = value;
							return true;
						}
					}
					
					return false;
				}

				public Dictionary<string, T> GetAllResources ()
				{
					var count = Count;
					var resources = new Dictionary<string, T> (count);
					for (int i = 0; i < count; ++i) {
						resources.Add (keys [i], values [i]);
					}
					
					return resources;
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
					if (Application.isPlaying) {
						EditorGUILayout.HelpBox (
							"L20n components can't be modified on runtime via the editor.",
							MessageType.Info);
						
						return;
					}

					serializedObject.Update();

					EditorGUILayout.LabelField ("Default");
					EditorGUILayout.PropertyField (defaultResource, GUIContent.none);

					EditorGUILayout.Space ();
					
					EditorGUILayout.PropertyField (resources);
					
					serializedObject.ApplyModifiedProperties();
				}
			}

			public class L20nResourceCollectionDrawer : PropertyDrawer {
				public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
					EditorGUI.LabelField(position, "Other Locales");
					
					var keys = property.FindPropertyRelative("keys");
					var values = property.FindPropertyRelative("values");
					
					if(GUILayout.Button("Add Other Localized Resource")) {
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