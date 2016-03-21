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
		public abstract class L20nBaseAudioClip : MonoBehaviour{
			public Internal.AudioClipCollection clips;
			public AudioClip defaultClip;
			
			void OnEnable() {
				L20n.OnLocaleChange += OnLocaleChange;
			}
			
			void OnDisable() {
				L20n.OnLocaleChange -= OnLocaleChange;
			}
			
			public void OnLocaleChange()
			{
				SetClip(clips.GetClip(L20n.CurrentLocale)
				          .UnwrapOr(defaultClip));
				
			}
			
			protected abstract void Initialize();
			public abstract void SetClip(AudioClip clip);
		}
		
		namespace Internal
		{
			[Serializable]
			public sealed class AudioClipCollection {
				public List<String> keys;
				public List<AudioClip> values;
				
				public AudioClipCollection()
				{
					keys = new List<string>();
					values = new List<AudioClip>();
				}
				
				public Option<AudioClip> GetClip(string key)
				{
					var result = new Option<AudioClip>();
					
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
			[CustomEditor (typeof (L20nBaseAudioClip))]
			public class L20nBaseAudioClipEditor : Editor {
				SerializedProperty clips;
				SerializedProperty defaultClip;
				
				void OnEnable () {
					clips = serializedObject.FindProperty ("clips");
					defaultClip = serializedObject.FindProperty ("defaultClip");
				}
				
				public override void OnInspectorGUI() {
					serializedObject.Update();
					
					
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Default Locale");
					EditorGUILayout.PropertyField(defaultClip, GUIContent.none);
					EditorGUILayout.EndHorizontal();
					
					EditorGUILayout.Space();
					
					EditorGUILayout.PropertyField(clips);
					
					serializedObject.ApplyModifiedProperties();
				}
			}
			
			[CustomPropertyDrawer(typeof(AudioClipCollection))]
			public class AudioClipDrawer : PropertyDrawer {
				public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
					EditorGUI.LabelField(position, "Other Locales");
					
					var keys = property.FindPropertyRelative("keys");
					var values = property.FindPropertyRelative("values");
					
					if(GUILayout.Button("Add Locale-AudioClip")) {
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