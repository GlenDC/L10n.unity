﻿/*
 * This source file is part of the L20n Unity Plugin.
 * Copyright (c) 2016 Glen De Cauwsemaecker (contact@glendc.com)
 */
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
			/// <summary>
			/// A Texture Collection, that can be used
			/// in combination with any L20nBaseResource class.
			/// </summary>
			/// <remarks>
			/// Look at the `L20nUIRawImage` class for a use-case example.
			/// </remarks>
			[Serializable]
			public sealed class L20nTextureCollection
			: L20nResourceCollection<Texture>
			{
			}
			
			#if UNITY_EDITOR
			/// <summary>
			/// A custom drawer for this collection,
			/// inhereting away the template argument.
			/// </summary>
			[CustomPropertyDrawer(typeof(L20nTextureCollection))]
			public sealed class L20nTextureCollectionDrawer
			: L20nResourceCollectionDrawer
			{
			}
			#endif
		}
	}
}