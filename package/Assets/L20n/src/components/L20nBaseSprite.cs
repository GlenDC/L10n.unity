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
		public abstract class L20nBaseSprite : MonoBehaviour{
			public Internal.SpriteCollection sprites;
			public Sprite defaultSprite;
			
			void OnEnable() {
				L20n.OnLocaleChange += OnLocaleChange;
			}
			
			void OnDisable() {
				L20n.OnLocaleChange -= OnLocaleChange;
			}
			
			public void OnLocaleChange()
			{
				SetSprite(sprites.GetSprite(L20n.CurrentLocale)
				           .UnwrapOr(defaultSprite));
				
			}

			protected abstract void Initialize();
			public abstract void SetSprite(Sprite sprite);
		}
		
		namespace Internal
		{
			[Serializable]
			public sealed class SpriteCollection {
				public List<String> keys;
				public List<Sprite> values;
				
				public SpriteCollection()
				{
					keys = new List<string>();
					values = new List<Sprite>();
				}
				
				public Option<Sprite> GetSprite(string key)
				{
					var result = new Option<Sprite>();

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
			[CustomEditor (typeof (L20nBaseSprite))]
			public class L20nBaseSpriteEditor : Editor {
				SerializedProperty sprites;
				SerializedProperty defaultSprite;
				
				void OnEnable () {
					sprites = serializedObject.FindProperty ("sprites");
					defaultSprite = serializedObject.FindProperty ("defaultSprite");
				}
				
				public override void OnInspectorGUI() {
					serializedObject.Update();
					
					
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Default Locale");
					EditorGUILayout.PropertyField(defaultSprite, GUIContent.none);
					EditorGUILayout.EndHorizontal();
					
					EditorGUILayout.Space();
					
					EditorGUILayout.PropertyField(sprites);
					
					serializedObject.ApplyModifiedProperties();
				}
			}
			
			[CustomPropertyDrawer(typeof(SpriteCollection))]
			public class SpriteCollectionDrawer : PropertyDrawer {
				public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
					EditorGUI.LabelField(position, "Other Locales");
					
					var keys = property.FindPropertyRelative("keys");
					var values = property.FindPropertyRelative("values");
					
					if(GUILayout.Button("Add Locale-Sprite")) {
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