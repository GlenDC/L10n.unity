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
		public sealed class L20nAudioSource : L20nBaseAudioClip {
			Option<AudioSource> m_Component;
			Option<AudioSource> Component
			{
				get
				{
					if(!m_Component.IsSet) {
						var text = GetComponent<AudioSource>();
						if(text != null) {
							m_Component.Set(text);
						}
					}
					
					return m_Component;
				}
			}
			
			public L20nAudioSource()
			{
				m_Component = new Option<AudioSource>();
			}
			
			protected override void Initialize()
			{
				Debug.Assert(Component.IsSet,
				             "<L20nAudioSource> requires a <AudioSource> component to be attached");
			}
			
			public override void SetClip(AudioClip clip)
			{
				Component.UnwrapIf(
					(component) => component.clip = clip);
			}
		}
		
		#if UNITY_EDITOR
		namespace Internal
		{
			[CustomEditor (typeof (L20nAudioSource))]
			public class L20nAudioSourceEditor : L20nBaseAudioClipEditor {}
		}
		#endif
	}
}
