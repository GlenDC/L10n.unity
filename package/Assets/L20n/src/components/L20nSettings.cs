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
using System.Collections.Generic;

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
			[SerializeField]
			string
				manifestPath;
			[SerializeField]
			string
				overrideLocale;
			[SerializeField]
			string
				uniqueGameID;
			[SerializeField]
			bool
				destroyObject;
			[SerializeField]
			Internal.L20nFontCollection
				fonts;
			[SerializeField]
			Font
				defaultFont;

			public L20nSettings ()
			{
				manifestPath = "";
				overrideLocale = "";
				uniqueGameID = "";
				destroyObject = true;
			}

			void Awake ()
			{
				if (!L20n.IsInitialized) {
					if (manifestPath == null) {
						Debug.LogError ("<L20nSettings> requires the manifest to be set", this);
						return;
					}
					
					if (overrideLocale == "")
						overrideLocale = null;

					L20n.Initialize (uniqueGameID, manifestPath, overrideLocale);
					L20n.SetFont (L20n.DefaultLocale, defaultFont);
					foreach (var pair in fonts.GetAllResources()) {
						L20n.SetFont (pair.Key, pair.Value);
					}
				}

				if (destroyObject)
					Destroy (gameObject);
				else
					Destroy (this);
			}
		}

		#if UNITY_EDITOR
		[CustomEditor (typeof (L20nSettings))]
		public class L20nSettingsEditor : Editor {
			SerializedProperty manifestPath;
			SerializedProperty overrideLocale;
			SerializedProperty gameID;
			SerializedProperty destroyObject;
			SerializedProperty fonts;
			SerializedProperty defaultFont;

			string path;
			string errorMessage;
			bool pathIsValid;
			L20nCore.Internal.Manifest manifest;
			
			void OnEnable () {
				manifestPath = serializedObject.FindProperty ("manifestPath");
				overrideLocale = serializedObject.FindProperty ("overrideLocale");
				gameID = serializedObject.FindProperty ("uniqueGameID");
				destroyObject = serializedObject.FindProperty ("destroyObject");
				fonts = serializedObject.FindProperty ("fonts");
				defaultFont = serializedObject.FindProperty ("defaultFont");

				path = null;
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
					EditorGUILayout.LabelField("DefaultLocale:");
					EditorGUILayout.LabelField(manifest.DefaultLocale);
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.LabelField("OtherLocales:");
					EditorGUILayout.LabelField(
						string.Join(", ", manifest.Locales.ToArray()),
						EditorStyles.wordWrappedLabel);
				}

				var titleStyle = EditorStyles.boldLabel;
				
				EditorGUILayout.Separator();
				EditorGUILayout.LabelField("Font Settings", titleStyle);
				EditorGUILayout.Separator();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Default Resource:");
				EditorGUILayout.PropertyField(defaultFont, GUIContent.none);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.PropertyField(fonts);

				EditorGUILayout.Separator();
				EditorGUILayout.LabelField("Development Settings", titleStyle);
				EditorGUILayout.Separator();

				EditorGUILayout.PropertyField(gameID);
				if (gameID.stringValue == "") {
					EditorGUILayout.HelpBox(
						"Please enter an identifier unique to your game. " +
						"This will prevent any user preference variables name clashing " +
						"on your development machine. ", MessageType.Warning);
				}

				EditorGUILayout.PropertyField(overrideLocale);
				if(overrideLocale.stringValue != "")
					EditorGUILayout.HelpBox(
						"This option is available for development purposes " +
						"and should not be used in production. " +
						"This overrides the cached locale choice " +
						"and will result in bad user experience.", MessageType.Warning);
				
				EditorGUILayout.Separator();
				EditorGUILayout.LabelField("Other Settings", titleStyle);
				EditorGUILayout.Separator();
				
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
