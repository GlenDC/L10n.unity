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
		[AddComponentMenu("L20n/AudioSource (clip)")]
		public sealed class L20nAudioSource :
			Internal.L20nBaseComponent<AudioSource, AudioClip, Internal.AudioClipCollection> {
			
			public override void SetResource(AudioClip clip)
			{
				Component.UnwrapIf(
					(component) => component.clip = clip);
			}
		}
		
		#if UNITY_EDITOR
		namespace Internal
		{
			[CustomEditor (typeof (L20nAudioSource))]
			public class L20nAudioSourceEditor : L20nBaseResourceEditor {}
		}
		#endif
	}
}
