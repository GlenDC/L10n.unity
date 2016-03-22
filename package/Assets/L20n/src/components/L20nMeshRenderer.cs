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
		[AddComponentMenu("L20n/MeshRenderer (3D)")]
		public sealed class L20nMeshRenderer :
		Internal.L20nBaseComponent<MeshRenderer, Material, Internal.MaterialCollection> {
			
			public override void SetResource(Material material)
			{
				Component.UnwrapIf(
					(component) => component.material = material);
			}
		}
		
		#if UNITY_EDITOR
		namespace Internal
		{
			[CustomEditor (typeof (L20nMeshRenderer))]
			public class L20nMeshRendererEditor : L20nBaseResourceEditor {}
		}
		#endif
	}
}
