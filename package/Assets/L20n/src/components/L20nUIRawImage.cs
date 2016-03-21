using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using L20nCore.Utils;

namespace L20nUnity
{
	namespace Components
	{
		[AddComponentMenu("L20n/UIRawImage (2D)")]
		public sealed class L20nUIRawImage : L20nBaseTexture {
			Option<RawImage> m_Component;
			Option<RawImage> Component
			{
				get
				{
					if(!m_Component.IsSet) {
						var text = GetComponent<RawImage>();
						if(text != null) {
							m_Component.Set(text);
						}
					}
					
					return m_Component;
				}
			}
			
			public L20nUIRawImage()
			{
				m_Component = new Option<RawImage>();
			}
			
			protected override void Initialize()
			{
				Debug.Assert(Component.IsSet,
				             "<L20nUIRawImage> requires a <RawImage> component to be attached");
			}
			
			public override void SetTexture(Texture texture)
			{
				Component.UnwrapIf(
					(component) => component.texture = texture);
			}
		}
		
		#if UNITY_EDITOR
		namespace Internal
		{
			[CustomEditor (typeof (L20nUIRawImage))]
			public class L20nUIRawImageEditor : L20nBaseTextureEditor {}
		}
		#endif
	}
}
