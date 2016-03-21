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
		[AddComponentMenu("L20n/UIImage (2D)")]
		public sealed class L20nUIImage : L20nBaseSprite {
			Option<Image> m_Component;
			Option<Image> Component
			{
				get
				{
					if(!m_Component.IsSet) {
						var text = GetComponent<Image>();
						if(text != null) {
							m_Component.Set(text);
						}
					}
					
					return m_Component;
				}
			}
			
			public L20nUIImage()
			{
				m_Component = new Option<Image>();
			}

			protected override void Initialize()
			{
				Debug.Assert(Component.IsSet,
				             "<L20nUIImage> requires a <Image> component to be attached");
			}
			
			public override void SetSprite(Sprite sprite)
			{
				Component.UnwrapIf(
					(component) => component.sprite = sprite);
			}
		}
		
		#if UNITY_EDITOR
		namespace Internal
		{
			[CustomEditor (typeof (L20nUIImage))]
			public class L20nUIImageEditor : L20nBaseSpriteEditor {}
		}
		#endif
	}
}
