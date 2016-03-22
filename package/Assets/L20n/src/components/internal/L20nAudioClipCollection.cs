using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace L20nUnity
{
	namespace Components
	{
		namespace Internal
		{
			[Serializable]
			public sealed class AudioClipCollection
			: L20nResourceCollection<AudioClip> {}

			#if UNITY_EDITOR
			[CustomPropertyDrawer(typeof(AudioClipCollection))]
			public sealed class AudioClipCollectionDrawer
				: L20nResourceCollectionDrawer {}
			#endif
		}
	}
}