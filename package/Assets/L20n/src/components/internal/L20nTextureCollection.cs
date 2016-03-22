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
			public sealed class TextureCollection
			: L20nResourceCollection<Texture> {}
			
			#if UNITY_EDITOR
			[CustomPropertyDrawer(typeof(TextureCollection))]
			public sealed class TextureCollectionDrawer
			: L20nResourceCollectionDrawer {}
			#endif
		}
	}
}