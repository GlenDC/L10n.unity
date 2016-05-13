/*
 * This source file is part of the L20n Unity Plugin.
 * Copyright (c) 2016 Glen De Cauwsemaecker (contact@glendc.com)
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
			/// <summary>
			/// A template abstract class to be used for any L20n Resource Component.
			/// The template types specify the ResourceType (T) and ResourceCollectionType (U).
			/// </summary>
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

				/// <summary>
				/// Allows you to modify a resource identified by the key, using the given value.
				/// The return value will be true in case the resource has been updated.
				/// </summary>
				public bool SetResource (string key, T value)
				{
					if (key == null) {
						Debug.LogWarning ("tried to assign a resource to a <L20nBaseResource> with null as a key", this);
						return false;
					}

					return resources.SetResource (key, value);
				}

				/// <summary>
				/// Set the given value as the default resource.
				/// This function will override any previously specified function.
				/// </summary>
				public void SetDefaultResource (T value)
				{
					defaultResource = value;
				}

				/// <summary>
				/// The function that will be called when the L20n locale gets changed.
				/// This function will set the resource linked to the current locale,
				/// if that's not found it will set default resource.
				/// </summary>
				/// <remarks>
				/// Note that this does mean that the final resource might be null.
				/// </remarks>
				public void OnLocaleChange ()
				{
					SetResource (resources.GetResource (L20n.CurrentLocale)
					          .UnwrapOr (defaultResource));	
				}

				/// <summary>
				/// Initialize this component, set the resource for the current locale.
				/// </summary>
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

			/// <summary>
			/// The base class for all Resource Collections.
			/// </summary>
			[Serializable]
			public abstract class L20nResourceCollection<T>
			{
				[SerializeField]
				List<String>
					keys;
				[SerializeField]
				List<T>
					values;

				/// <summary>
				/// Returns the amount of resources in this collection.
				/// </summary>
				public int Count {
					get { return Math.Min (keys.Count, values.Count); }
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="L20nUnity.Components.Internal.L20nResourceCollection`1"/> class.
				/// </summary>
				public L20nResourceCollection ()
				{
					keys = new List<string> ();
					values = new List<T> ();
				}

				/// <summary>
				/// Returns the optional resource based on the given locale key.
				/// </summary>
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

				/// <summary>
				/// Sets the resource for the given lcoale key.
				/// Returns true if the value could be set, otherwise false gets returned.
				/// </summary>
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

				/// <summary>
				/// Returns the collection of resources as a dictionary.
				/// </summary>
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
			/// <summary>
			/// A custom drawer that will be used for any L20n Resource Component.
			/// </summary>
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

			/// <summary>
			/// A custom drawer that will be used for any L20n Resource Collection.
			/// </summary>
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