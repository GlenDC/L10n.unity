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
		/// A component to set the Texture of Unity's `UI/RawImage` component
		/// based on the current locale.
		/// </summary>
		/// <remarks>
		/// This component can be used as the template for any Component that you may
		/// wish to create related to a Texture.
		/// </remarks>
		[AddComponentMenu("L20n/UIRawImage (2D)")]
		public sealed class L20nUIRawImage :
			Internal.L20nBaseResourceComponent<RawImage, Texture, Internal.L20nTextureCollection>
		{	
			/// <summary>
			/// Called at start-up and everytime the locale gets set.
			/// </summary>
			/// <param name="texture">the localized resource to be used</param>
			public override void SetResource (Texture texture)
			{
				Component.UnwrapIf (
					(component) => component.texture = texture);
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
			[CustomEditor (typeof (L20nUIRawImage))]
			public class L20nUIRawImageEditor : L20nBaseResourceEditor {}
		}
		#endif
	}
}
