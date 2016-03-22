using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using L20nCore.Utils;

namespace L20nUnity
{
	namespace Components
	{
		namespace Internal
		{
			public abstract class L20nBaseTextComponent<T> : L20nBaseText {
				protected Option<T> m_Component;
				protected Option<T> Component
				{
					get
					{
						if(!m_Component.IsSet) {
							var text = GetComponent<T>();
							if(text != null) {
								m_Component.Set(text);
							}
						}
						
						return m_Component;
					}
				}
				
				public L20nBaseTextComponent()
				{
					m_Component = new Option<T>();
				}
				
				protected override void Initialize()
				{
					Debug.Assert(Component.IsSet,
					             "{0} requires a {1} to be attached",
					             GetType(), typeof(T));
				}
			}
		}
	}
}