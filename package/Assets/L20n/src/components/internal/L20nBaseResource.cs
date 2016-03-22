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
				where U: L20nResourceCollection<T>
			{
				public U resources;
				public T defaultResource;
				
				void OnEnable() {
					L20n.OnLocaleChange += OnLocaleChange;
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
				SerializedProperty sprites;
				SerializedProperty defaultSprite;
				
				void OnEnable () {
					sprites = serializedObject.FindProperty ("resources");
					defaultSprite = serializedObject.FindProperty ("defaultResource");
				}
				
				public override void OnInspectorGUI() {
					serializedObject.Update();

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Default");
					EditorGUILayout.PropertyField(defaultSprite, GUIContent.none);
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.Space();
					
					EditorGUILayout.PropertyField(sprites);
					
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