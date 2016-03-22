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
			public sealed class MaterialCollection
			: L20nResourceCollection<Material> {}
			
			#if UNITY_EDITOR
			[CustomPropertyDrawer(typeof(MaterialCollection))]
			public sealed class MaterialCollectionDrawer
			: L20nResourceCollectionDrawer {}
			#endif
		}
	}
}