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
		[AddComponentMenu("L20n/UIText (2D)")]
		public sealed class L20nUIText : L20nBaseText {
			Option<Text> m_Component;
			Option<Text> Component
			{
				get
				{
					if(!m_Component.IsSet) {
						var text = GetComponent<Text>();
						if(text != null) {
							m_Component.Set(text);
						}
					}

					return m_Component;
				}
			}

			public L20nUIText()
			{
				m_Component = new Option<Text>();
			}

			protected override void Initialize()
			{
				Debug.Assert(Component.IsSet,
				             "<L20nUIText> requires a <Text> component to be attached");
			}

			public override void SetText(string text)
			{
				Component.UnwrapIf(
					(component) => component.text = text);
			}
		}
		
		#if UNITY_EDITOR
		namespace Internal
		{
			[CustomEditor (typeof (L20nUIText))]
			public class L20nUITextEditor : L20nBaseTextEditor {}
		}
		#endif
	}
}
