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
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace L20nUnity
{
	namespace Components
	{
		/// <summary>
		/// A component that can be used in all scenes that you wish to start-up from.
		/// It will initialize & setup the L20n System with all your specified settings.
		/// </summary>
		/// <remarks>
		/// All of its logic will happen during the `Awake` phase.
		/// Once the initialization and setup is done this component will be destroyed
		/// and optionally the object containing this component as well.
		/// </remarks>
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
			Internal.GlobalValueCollection
				globalVariables;
			[SerializeField]
			Font
				defaultFont;

			/// <summary>
			/// Sets all the settings to its defaults.
			/// </summary>
			/// <remarks>
			/// This doesn't do any of the actual L20n setup/initialization yet.
			/// </remarks>
			public L20nSettings ()
			{
				manifestPath = "";
				overrideLocale = "";
				uniqueGameID = "";
				destroyObject = true;
				defaultFont = null;
				fonts = new Internal.L20nFontCollection ();
				globalVariables = new Internal.GlobalValueCollection ();
			}

			/// <summary>
			/// Initialized and setups the L20n System.
			/// Called during the `Awake` phase.
			/// </summary>
			void Awake ()
			{
				// Make sure that L20n isn't initialized yet.
				// This can happen as you might have used this component already in a previous scene.
				// Which might be quite common as this component should be used in all scenes
				// that you may want to start up from directly during development.
				if (!L20n.IsInitialized) {
					if (manifestPath == null) {
						Debug.LogError ("<L20nSettings> requires the manifest to be set", this);
						return;
					}
					
					if (overrideLocale == "")
						overrideLocale = null;

					// Initialize L20n
					L20n.Initialize (uniqueGameID, manifestPath, overrideLocale);

					// Set the default fonts and global variables (if there are any)
					L20n.SetFont (L20n.DefaultLocale, defaultFont);
					foreach (var pair in fonts.GetAllResources()) {
						L20n.SetFont (pair.Key, pair.Value);
					}

					globalVariables.AddValuesToEnvironment();
				}

				if (destroyObject)
					Destroy (gameObject);
				else
					Destroy (this);
			}
		}

		namespace Internal
		{
			/// <summary>
			/// A helper class that serves as the Collection of Global Values.
			/// </summary>
			[Serializable]
			public sealed class GlobalValueCollection
			{
				[SerializeField]
				private List<String>
					keys;
				[SerializeField]
				private List<GlobalValue>
					values;
				
				private int Count {
					get { return Math.Min (keys.Count, values.Count); }
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="L20nUnity.Components.Internal.GlobalValueCollection"/> class.
				/// </summary>
				public GlobalValueCollection ()
				{
					keys = new List<string> ();
					values = new List<GlobalValue> ();
				}

				/// <summary>
				/// Add all the values specified in this collection to the Global Environment.
				/// </summary>
				/// <remarks>
				/// All values in this collection will get removed at the end of this method.
				/// </remarks>
				public void AddValuesToEnvironment ()
				{
					var count = Count;
					for (int i = 0; i < count; ++i) {
						L20n.AddGlobal (keys [i], values [i].GetValue());
					}

					keys.Clear ();
					values.Clear ();
				}
			}

			/// <summary>
			/// A helper class for any supported type of Global Values.
			/// </summary>
			[Serializable]
			public sealed class GlobalValue
			{
				[SerializeField]
				Type
					type;
				[SerializeField]
				bool
					boolean;
				[SerializeField]
				int
					literal;
				[SerializeField]
				string
					text;
				[SerializeField]
				HashValueBehaviour
					hash;

				/// <summary>
				/// Initializes a new instance of the <see cref="L20nUnity.Components.Internal.GlobalValue"/> class.
				/// </summary>
				public GlobalValue ()
				{
					type = Type.String;
				}

				/// <summary>
				/// Returns the L20nObject value based on the specified type and set value.
				/// </summary>
				public L20nCore.Objects.L20nObject GetValue ()
				{
					switch (type) {
					case Type.Boolean:
						return new L20nCore.Objects.BooleanValue (boolean);
					case Type.Literal:
						return new L20nCore.Objects.Literal (literal);
					case Type.String:
						return new L20nCore.Objects.StringOutput (text);
					case Type.HashValue:
						if (hash == null) {
							return null;
						}
						
						return new L20nCore.Objects.Entity (hash);
					}
					
					return null;
				}

				/// <summary>
				/// The supported types of GLobal Variables.
				/// </summary>
				public enum Type
				{
					Boolean,	// a C# boolean
					Literal,	// a C# integer
					String,		// a C# string
					HashValue	// a `HashValueBehaviour`-object
				}
			}

			#if UNITY_EDITOR
			/// <summary>
			/// A custom drawer for the `L20nSettings` component.
			/// </summary>
			/// <remarks>
			/// This class is quite ugly and big, thanks to the poor API exposed by Unity.
			/// If you read this and have any idea's on how to improve it,
			/// feel free to reach out to me and you might get a free license in return.
			/// </remarks>
			[CustomEditor (typeof (L20nSettings))]
			public class L20nSettingsEditor : Editor {
				SerializedProperty manifestPath;
				SerializedProperty overrideLocale;
				SerializedProperty gameID;
				SerializedProperty destroyObject;
				SerializedProperty fonts;
				SerializedProperty defaultFont;
				SerializedProperty globalVariables;

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
					globalVariables = serializedObject.FindProperty ("globalVariables");

					path = null;
					pathIsValid = false;
					manifest = new L20nCore.Internal.Manifest();

					L20nCore.IO.StreamReaderFactory.SetCallback(L20n.CreateStreamReader);
				}
				
				public override void OnInspectorGUI() {
					serializedObject.Update();
					
					var maxWidth = EditorGUIUtility.currentViewWidth;
					var maxWidthOption = GUILayout.MaxWidth (maxWidth);

					EditorGUILayout.LabelField("Manifest Resource Path");
					EditorGUILayout.PropertyField(manifestPath, GUIContent.none, maxWidthOption);

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
						EditorGUILayout.BeginHorizontal (
							GUILayout.MaxWidth (maxWidth));
						EditorGUILayout.LabelField("DefaultLocale:", maxWidthOption);
						EditorGUILayout.LabelField(manifest.DefaultLocale, maxWidthOption);
						EditorGUILayout.EndHorizontal();

						EditorGUILayout.LabelField("OtherLocales:", maxWidthOption);
						EditorGUILayout.LabelField(
							string.Join(", ", manifest.Locales.ToArray()),
							EditorStyles.wordWrappedLabel, maxWidthOption);
					}

					var titleStyle = EditorStyles.boldLabel;
					
					EditorGUILayout.Separator();
					EditorGUILayout.LabelField("Font Settings", titleStyle, maxWidthOption);
					EditorGUILayout.Separator();

					EditorGUILayout.BeginHorizontal (maxWidthOption);
					EditorGUILayout.LabelField("Default Resource:");
					EditorGUILayout.PropertyField(defaultFont, GUIContent.none, maxWidthOption);
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.PropertyField(fonts, maxWidthOption);
					
					EditorGUILayout.Separator();
					EditorGUILayout.LabelField("Custom Global Variables", titleStyle, maxWidthOption);
					EditorGUILayout.Separator();

					var gvsize = globalVariables.FindPropertyRelative ("keys").arraySize;
					EditorGUILayout.BeginHorizontal (
						GUILayout.MinHeight (30 + gvsize * 30 + (gvsize - 1) * 20),
						maxWidthOption);
					EditorGUILayout.PropertyField(globalVariables, maxWidthOption);
					EditorGUILayout.EndHorizontal ();

					EditorGUILayout.Separator();
					EditorGUILayout.LabelField("Development Settings", titleStyle, maxWidthOption);
					EditorGUILayout.Separator();

					EditorGUILayout.PropertyField(gameID, maxWidthOption);
					if (gameID.stringValue == "") {
						EditorGUILayout.HelpBox(
							"Please enter an identifier unique to your game. " +
							"This will prevent any user preference variables name clashing " +
							"on your development machine. ", MessageType.Warning);
					}

					EditorGUILayout.PropertyField(overrideLocale, maxWidthOption);
					if(overrideLocale.stringValue != "")
						EditorGUILayout.HelpBox(
							"This option is available for development purposes " +
							"and should not be used in production. " +
							"This overrides the cached locale choice " +
							"and will result in bad user experience.", MessageType.Warning);
					
					EditorGUILayout.Separator();
					EditorGUILayout.LabelField("Other Settings", titleStyle, maxWidthOption);
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

			/// <summary>
			/// A custom drawer for a `GlobalValueCollection`.
			/// </summary>
			[CustomPropertyDrawer(typeof(GlobalValueCollection))]
			public class GlobalValueCollectionDrawer : PropertyDrawer {
				Rect Offset(Rect position, float sx, float sy, float sw, float sh) {
					return new Rect(position.x + (position.width * sx),
					                position.y + (position.height * sy),
					                position.width * sw, position.height * sh);
				}
				
				public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
					var keys = property.FindPropertyRelative("keys");
					var values = property.FindPropertyRelative("values");
					
					var btnRect = new Rect(
						position.x,
						position.y,
						position.width,
						position.height);
					
					if(GUI.Button(btnRect, new GUIContent("Add Global Variable", "add a custom global variable"))) {
						keys.InsertArrayElementAtIndex(keys.arraySize);
						values.InsertArrayElementAtIndex(values.arraySize);
					}
					
					position = Offset(position, 0, 1.50f, 1, 1);
					
					for (int i = 0; i < keys.arraySize; ++i) {
						var deleteRect = Offset(position, 0, 0, .2f, 1f);
						if(GUI.Button(deleteRect, new GUIContent("delete", "delete this global variable"))) {
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

			/// <summary>
			/// A custom drawer used for each `GlobalValue`.
			/// </summary>
			[CustomPropertyDrawer(typeof(GlobalValue))]
			public class GlobalValueDrawer : PropertyDrawer {
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
					switch ((GlobalValue.Type)type.enumValueIndex) {
						case GlobalValue.Type.Boolean: {
							var value = property.FindPropertyRelative("boolean");
							EditorGUI.PropertyField(valueRect, value, GUIContent.none);
							break;
						}

						case GlobalValue.Type.Literal: {
							var value = property.FindPropertyRelative("literal");
							EditorGUI.PropertyField(valueRect, value, GUIContent.none);
							break;
						}
							
						case GlobalValue.Type.String: {
							var value = property.FindPropertyRelative("text");
							EditorGUI.PropertyField(valueRect, value, GUIContent.none);
							break;
						}
							
						case GlobalValue.Type.HashValue: {
							var hash = property.FindPropertyRelative("hash");
							hash.objectReferenceValue = EditorGUI.ObjectField(
								valueRect, hash.objectReferenceValue, typeof (HashValueBehaviour), true);
							break;
						}
					}
				}
			}
			#endif
		}
	}
}
