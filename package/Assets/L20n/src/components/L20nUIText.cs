/*
 * This source file is part of the L20n Unity Plugin.
 * Copyright (c) 2016 Glen De Cauwsemaecker (contact@glendc.com)
 */
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace L20nUnity
{
	namespace Components
	{
		/// <summary>
		/// A component to set the text of Unity's `UI/Text` component
		/// based on the current locale.
		/// </summary>
		/// <remarks>
		/// This component can be used as the template for any Component that you may
		/// wish to create related to text.
		/// </remarks>
		[AddComponentMenu("L20n/UIText (2D)")]
		public sealed class L20nUIText
			: Internal.L20nBaseTextComponent<Text>
		{
			/// <summary>
			/// Called every update cycle by the Base Class.
			/// </summary>
			/// <param name="text">the localized text (string) to be used</param>
			/// <param name="font">the font to be used, if not equal to null</param>
			public override void SetText (string text, Font font)
			{
				Component.UnwrapIf ((component) => {
					component.text = text;
					if (font != null)
						component.font = font;
				});
			}
		}
		
		#if UNITY_EDITOR
		namespace Internal
		{
			/// <summary>
			/// The internal editor, used draw this Custom Component.
			/// </summary>
			/// <remarks>
			/// Don't forget to copy this part in case
			/// you're creating your own component based on this component
			/// </remarks>
			[CustomEditor (typeof (L20nUIText))]
			public class L20nUITextEditor : Internal.L20nBaseTextEditor {}
		}
		#endif
	}
}
