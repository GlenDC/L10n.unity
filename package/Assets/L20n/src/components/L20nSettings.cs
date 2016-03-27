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

namespace L20nUnity
{
	namespace Components
	{
		[AddComponentMenu("L20n/Settings (initialization)")]
		public class L20nSettings : MonoBehaviour
		{
			[SerializeField] string manifestPath;
			[SerializeField] string defaultLocale;
			[SerializeField] bool destroyObject;

			public L20nSettings()
			{
				manifestPath = "";
				defaultLocale = "";
				destroyObject = true;
			}

			void Awake()
			{
				if(manifestPath == null) {
					Debug.LogError("<L20nSettings> requires the manifest to be set", this);
					return;
				}

				if(defaultLocale == "")
					defaultLocale = null;

				L20n.Initialize(manifestPath, defaultLocale);

				if(destroyObject)
					Destroy(gameObject);
				else
					Destroy(this);
			}
		}

		#if UNITY_EDITOR
		[CustomEditor (typeof (L20nSettings))]
		[CanEditMultipleObjects]
		public class L20nSettingsEditor : Editor {
			SerializedProperty manifestPath;
			SerializedProperty defaultLocale;
			SerializedProperty destroyObject;

			string path;
			
			void OnEnable () {
				manifestPath = serializedObject.FindProperty ("manifestPath");
				defaultLocale = serializedObject.FindProperty ("defaultLocale");
				destroyObject = serializedObject.FindProperty ("destroyObject");
			}
			
			public override void OnInspectorGUI() {
				serializedObject.Update();

				EditorGUILayout.LabelField("Manifest Resource Path");
				EditorGUILayout.PropertyField(manifestPath, GUIContent.none);
				if (manifestPath.stringValue == "") {
					EditorGUILayout.HelpBox(
						"Please enter a valid Resource path.",
						MessageType.Error);
				} else if (manifestPath.stringValue.Contains ("\\")) {
					EditorGUILayout.HelpBox(
						"Please use only forward slashes `\\` in your manifest resource path!",
						MessageType.Error);
				}

				EditorGUILayout.Separator();

				EditorGUILayout.PropertyField(defaultLocale);
				if(defaultLocale.stringValue != "")
					EditorGUILayout.HelpBox(
						"DefaultLocale is available for development purposes " +
						"and should not be used in production.", MessageType.Warning);

				EditorGUILayout.PropertyField(destroyObject);
				if (destroyObject.boolValue) {
					EditorGUILayout.HelpBox(
						"The object to which this component is attached will be destroyed " +
						"when this component awakes, as is requested with this checkbox.", MessageType.Info);
				}
				
				serializedObject.ApplyModifiedProperties();
			}
		}
		#endif
	}
}
