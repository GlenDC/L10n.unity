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
		public abstract class L20nBaseTexture : MonoBehaviour{
			public Internal.TextureCollection textures;
			public Texture defaultTexture;
			
			void OnEnable() {
				OnLocaleChange();
				L20n.OnLocaleChange += OnLocaleChange;
			}

			void OnDisable() {
				L20n.OnLocaleChange -= OnLocaleChange;
			}

			void OnLocaleChange()
			{
				SetTexture(textures.GetTexture(L20n.CurrentLocale)
					.UnwrapOr(defaultTexture));

			}
			
			protected abstract void Initialize();
			public abstract void SetTexture(Texture texture);
		}
		
		namespace Internal
		{
			[Serializable]
			public sealed class TextureCollection {
				public List<String> keys;
				public List<Texture> values;
				
				public TextureCollection()
				{
					keys = new List<string>();
					values = new List<Texture>();
				}
				
				public Option<Texture> GetTexture(string key)
				{
					var result = new Option<Texture>();

					if(keys.Count == values.Count) {
						for(int i = 0; i < keys.Count; ++i) {
							if(keys[i].Equals(key)) {
								result.Set(values[i]);
								break;
							}
						}
					}

					return result;
				}
			}
			
			#if UNITY_EDITOR
			[CustomEditor (typeof (L20nBaseTexture))]
			public class L20nBaseTextureEditor : Editor {
				SerializedProperty textures;
				SerializedProperty defaultTexture;
				
				void OnEnable () {
					textures = serializedObject.FindProperty ("textures");
					defaultTexture = serializedObject.FindProperty ("defaultTexture");
				}
				
				public override void OnInspectorGUI() {
					serializedObject.Update();


					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Default Locale");
					EditorGUILayout.PropertyField(defaultTexture, GUIContent.none);
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.Space();
					
					EditorGUILayout.PropertyField(textures);
					
					serializedObject.ApplyModifiedProperties();
				}
			}
			
			[CustomPropertyDrawer(typeof(TextureCollection))]
			public class TextureCollectionDrawer : PropertyDrawer {
				public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
					EditorGUI.LabelField(position, "Other Locales");
					
					var keys = property.FindPropertyRelative("keys");
					var values = property.FindPropertyRelative("values");
					
					if(GUILayout.Button("Add Locale-Texture")) {
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