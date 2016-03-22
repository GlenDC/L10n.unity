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
			public sealed class SpriteCollection
			: L20nResourceCollection<Sprite> {}
			
			#if UNITY_EDITOR
			[CustomPropertyDrawer(typeof(SpriteCollection))]
			public sealed class SpriteCollectionDrawer
			: L20nResourceCollectionDrawer {}
			#endif
		}
	}
}