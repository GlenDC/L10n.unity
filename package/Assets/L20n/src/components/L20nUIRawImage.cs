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
		public sealed class L20nUIRawImage :
		Internal.L20nBaseComponent<RawImage, Texture, Internal.TextureCollection> {
			
			public override void SetResource(Texture texture)
			{
				Component.UnwrapIf(
					(component) => component.texture = texture);
			}
		}
		
		#if UNITY_EDITOR
		namespace Internal
		{
			[CustomEditor (typeof (L20nUIRawImage))]
			public class L20nUIRawImageEditor : L20nBaseResourceEditor {}
		}
		#endif
	}
}
