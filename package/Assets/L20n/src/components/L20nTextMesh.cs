using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using System;

namespace L20nUnity
{
	namespace Components
	{
		[AddComponentMenu("L20n/TextMesh (3D)")]
		public sealed class L20nTextMesh
			: Internal.L20nBaseTextComponent<TextMesh>
		{
			public override void SetText(string text)
			{
				Component.UnwrapIf(
					(component) => component.text = text);
			}
		}
		
		#if UNITY_EDITOR
		namespace Internal
		{
			[CustomEditor (typeof (L20nTextMesh))]
			public class L20nTextMeshEditor : Internal.L20nBaseTextEditor {}
		}
		#endif
	}
}
