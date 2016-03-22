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
		public sealed class L20nUIImage :
		Internal.L20nBaseComponent<Image, Sprite, Internal.SpriteCollection> {
			
			public override void SetResource(Sprite sprite)
			{
				Component.UnwrapIf(
					(component) => component.sprite = sprite);
			}
		}
		
		#if UNITY_EDITOR
		namespace Internal
		{
			[CustomEditor (typeof (L20nUIImage))]
			public class L20nUIImageEditor : L20nBaseResourceEditor {}
		}
		#endif
	}
}
