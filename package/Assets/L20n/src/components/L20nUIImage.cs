/*
 * This source file is part of the L20n Unity Plugin.
 * Copyright (c) 2016 Glen De Cauwsemaecker (contact@glendc.com)
 */
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif
using L20nCore.Utils;

namespace L20nUnity
{
	namespace Components
	{
		/// <summary>
		/// A component to set the Sprite of Unity's `UI/Image` component
		/// based on the current locale.
		/// </summary>
		/// <remarks>
		/// This component can be used as the template for any Component that you may
		/// wish to create related to a Sprite.
		/// </remarks>
		[AddComponentMenu("L20n/UIImage (2D)")]
		public sealed class L20nUIImage :
			Internal.L20nBaseResourceComponent<Image, Sprite, Internal.L20nSpriteCollection>
		{
			/// <summary>
			/// Called at start-up and everytime the locale gets set.
			/// </summary>
			/// <param name="sprite">the localized resource to be used</param>
			public override void SetResource (Sprite sprite)
			{
				Component.UnwrapIf (
					(component) => component.sprite = sprite);
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
			/// you're creating your own component based on this component.
			/// </remarks>
			[CustomEditor (typeof (L20nUIImage))]
			public class L20nUIImageEditor : L20nBaseResourceEditor {}
		}
		#endif
	}
}
