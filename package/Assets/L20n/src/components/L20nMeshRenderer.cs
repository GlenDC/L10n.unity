/**
 * This source file is part of the Commercial L20n Unity Plugin.
 * 
 * Copyright (c) 2016 Glen De Cauwsemaecker (contact@glendc.com)
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *    http://www.apache.org/licenses/LICENSE-2.0

 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
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
		/// A component to set the Material of Unity's `UI/MeshRenderer` component
		/// based on the current locale.
		/// </summary>
		/// <remarks>
		/// This component can be used as the template for any Component that you may
		/// wish to create related to a Material.
		/// </remarks>
		[AddComponentMenu("L20n/MeshRenderer (3D)")]
		public sealed class L20nMeshRenderer :
			Internal.L20nBaseResourceComponent<MeshRenderer, Material, Internal.L20nMaterialCollection>
		{
			/// <summary>
			/// Called at start-up and everytime the locale gets set.
			/// </summary>
			/// <param name="material">the localized resource to be used</param>
			public override void SetResource (Material material)
			{
				Component.UnwrapIf (
					(component) => component.material = material);
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
			[CustomEditor (typeof (L20nMeshRenderer))]
			public class L20nMeshRendererEditor : L20nBaseResourceEditor {}
		}
		#endif
	}
}
