using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using L20nCore.Utils;

namespace L20nUnity
{
	namespace Components
	{
		[AddComponentMenu("L20n/TextMesh (3D)")]
		public sealed class L20nTextMesh : L20nBaseText {
			Option<TextMesh> m_Component;
			Option<TextMesh> Component
			{
				get
				{
					if(!m_Component.IsSet) {
						var text = GetComponent<TextMesh>();
						if(text != null) {
							m_Component.Set(text);
						}
					}
					
					return m_Component;
				}
			}
			
			public L20nTextMesh()
			{
				m_Component = new Option<TextMesh>();
			}
			
			void OnEnable()
			{
				Debug.Assert(Component.IsSet,
				             "<L20nTextMesh> requires a <TextMesh> component to be attached");
			}
			
			public override void SetText(string text)
			{
				Component.UnwrapIf(
					(component) => component.text = text);
			}
		}
		
		#if UNITY_EDITOR
		namespace Internal
		{
			[CustomEditor (typeof (L20nTextMesh))]
			public class L20nTextMeshEditor : L20nTextEditor {}
		}
		#endif
	}
}
