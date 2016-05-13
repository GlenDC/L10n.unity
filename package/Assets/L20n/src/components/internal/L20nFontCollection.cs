/*
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
			/// A Font Collection, that can be used
			/// in combination with any L20nBaseResource class.
			/// </summary>
			[Serializable]
			public sealed class L20nFontCollection
				: L20nResourceCollection<Font>
			{
			}
			
			#if UNITY_EDITOR
			[CustomPropertyDrawer(typeof(L20nFontCollection))]
			public sealed class L20nFontCollectionDrawer
				: L20nResourceCollectionDrawer
			{
			}
			#endif
		}
	}
}