/*
 * This source file is part of the L20n Unity Plugin.
 * Copyright (c) 2016 Glen De Cauwsemaecker (contact@glendc.com)
 */
using UnityEngine;

using System;
using L20nCore.Utils;

namespace L20nUnity
{
	namespace Components
	{
		namespace Internal
		{
			/// <summary>
			/// An abstract base class used for all L20n Resource Components.
			/// The template types specify the ComponentType (T), ResourceTyep (U) and ResourceCollection (V).
			/// It specifies the logic for lazily getting and chacing the specified component.
			/// </summary>
			public abstract class L20nBaseResourceComponent<T, U, V>
				: L20nBaseResource<U, V>
					where U: UnityEngine.Object
					where V: L20nResourceCollection<U>
			{
				protected Option<T> m_Component;

				/// <summary>
				/// Gets the specified component from cache.
				/// If the component has not been cached yet, this will be done first.
				/// </summary>
				protected Option<T> Component {
					get {
						if (!m_Component.IsSet) {
							var text = GetComponent<T> ();
							if (text != null) {
								m_Component.Set (text);
							}
						}
						
						return m_Component;
					}
				}
				
				public L20nBaseResourceComponent ()
				{
					m_Component = new Option<T> ();
				}
				
				protected override void Initialize ()
				{
					if (!Component.IsSet) {
						Debug.LogErrorFormat (
							"{0} requires a {1} to be attached",
					    	GetType (), typeof(T));
					}
				}
			}
		}
	}
}
