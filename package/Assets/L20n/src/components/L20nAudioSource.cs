/*
 * This source file is part of the L20n Unity Plugin.
 * Copyright (c) 2016 Glen De Cauwsemaecker (contact@glendc.com)
 */
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
using L20nCore.Utils;

namespace L20nUnity
{
	namespace Components
	{
		/// <summary>
		/// A component to set the AudioClip of Unity's `UI/AudioSource` component
		/// based on the current locale.
		/// </summary>
		/// <remarks>
		/// This component can be used as the template for any Component that you may
		/// wish to create related to a AudioClip.
		/// </remarks>
		[AddComponentMenu("L20n/AudioSource (clip)")]
		public sealed class L20nAudioSource :
			Internal.L20nBaseResourceComponent<AudioSource, AudioClip, Internal.L20nAudioClipCollection>
		{
			/// <summary>
			/// Called at start-up and everytime the locale gets set.
			/// </summary>
			/// <param name="clip">the localized resource to be used</param>
			public override void SetResource (AudioClip clip)
			{
				Component.UnwrapIf (
					(component) => component.clip = clip);
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
			[CustomEditor (typeof (L20nAudioSource))]
			public class L20nAudioSourceEditor : L20nBaseResourceEditor {}
		}
		#endif
	}
}
