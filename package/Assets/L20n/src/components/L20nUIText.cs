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
		[AddComponentMenu("L20n/UIText (2D)")]
		public sealed class L20nUIText
			: Internal.L20nBaseTextComponent<Text>
		{
			public override void SetText(string text)
			{
				Component.UnwrapIf(
					(component) => component.text = text);
			}
		}
		
		#if UNITY_EDITOR
		namespace Internal
		{
			[CustomEditor (typeof (L20nUIText))]
			public class L20nUITextEditor : Internal.L20nBaseTextEditor {}
		}
		#endif
	}
}
