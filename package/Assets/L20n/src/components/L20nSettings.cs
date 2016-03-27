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
		[DisallowMultipleComponent]
		public class L20nSettings : MonoBehaviour
		{
			[SerializeField] string manifestPath;
			[SerializeField] string overrideLocale;
			[SerializeField] bool destroyObject;

			public L20nSettings()
			{
				manifestPath = "";
				overrideLocale = "";
				destroyObject = true;
			}

			void Awake()
			{
				if(manifestPath == null) {
					Debug.LogError("<L20nSettings> requires the manifest to be set", this);
					return;
				}

				if(overrideLocale == "")
					overrideLocale = null;

				L20n.Initialize(manifestPath, overrideLocale);

				if(destroyObject)
					Destroy(gameObject);
				else
					Destroy(this);
			}
		}

		#if UNITY_EDITOR
		[CustomEditor (typeof (L20nSettings))]
		public class L20nSettingsEditor : Editor {
			SerializedProperty manifestPath;
			SerializedProperty overrideLocale;
			SerializedProperty destroyObject;

			string path;
			string errorMessage;
			bool pathIsValid;
			L20nCore.Internal.Manifest manifest;
			
			void OnEnable () {
				manifestPath = serializedObject.FindProperty ("manifestPath");
				overrideLocale = serializedObject.FindProperty ("overrideLocale");
				destroyObject = serializedObject.FindProperty ("destroyObject");

				path = "";
				pathIsValid = false;
				manifest = new L20nCore.Internal.Manifest();

				L20nCore.IO.StreamReaderFactory.SetCallback(L20n.CreateStreamReader);
			}
			
			public override void OnInspectorGUI() {
				serializedObject.Update();

				EditorGUILayout.LabelField("Manifest Resource Path");
				EditorGUILayout.PropertyField(manifestPath, GUIContent.none);

				if (path != manifestPath.stringValue) {
					path = manifestPath.stringValue;
					pathIsValid = Resources.Load (path) != null;
					if (pathIsValid) {
						errorMessage = "";
						try {
							manifest.Import(path);
						} catch (System.Exception e) {
							errorMessage = e.Message;
						}
					} else {
						string extra = "";
						if(path.Contains("\\"))
							extra = " Only use forward slashes '/', " +
								"backward slashes '\\' are not supported.";
						errorMessage = "Please enter a valid Resource path." + extra;
					}
				}

				if (errorMessage != "") {
					EditorGUILayout.HelpBox (errorMessage, MessageType.Error);
				} else {
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("DefaultLocale:", EditorStyles.boldLabel);
					EditorGUILayout.LabelField(manifest.DefaultLocale);
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.LabelField("OtherLocales:", EditorStyles.boldLabel);
					for(int i = 0; i < manifest.Locales.Count; i += 2) {
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField(manifest.Locales[i]);
						if(i+1 < manifest.Locales.Count)
							EditorGUILayout.LabelField(manifest.Locales[i+1]);
						EditorGUILayout.EndHorizontal();
					}
				}

				EditorGUILayout.Separator();
				EditorGUILayout.LabelField("Development Settings");

				EditorGUILayout.PropertyField(overrideLocale);
				if(overrideLocale.stringValue != "")
					EditorGUILayout.HelpBox(
						"This option is available for development purposes " +
						"and should not be used in production. " +
						"This overrides the cached locale choice " +
						"and will result in bad user experience.", MessageType.Warning);
				
				EditorGUILayout.Separator();
				EditorGUILayout.LabelField("Other Settings");
				
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
