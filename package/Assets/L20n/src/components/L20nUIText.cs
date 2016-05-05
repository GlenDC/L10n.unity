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
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace L20nUnity
{
	namespace Components
	{
		/// <summary>
		/// A component to set the text of Unity's `UI/Text` component
		/// based on the current locale.
		/// </summary>
		/// <remarks>
		/// This component can be used as the template for any Component that you may
		/// wish to create related to text.
		/// </remarks>
		[AddComponentMenu("L20n/UIText (2D)")]
		public sealed class L20nUIText
			: Internal.L20nBaseTextComponent<Text>
		{
			/// <summary>
			/// Called every update cycle by the Base Class.
			/// </summary>
			/// <param name="text">the localized text (string) to be used</param>
			/// <param name="font">the font to be used, if not equal to null</param>
			public override void SetText (string text, Font font)
			{
				Component.UnwrapIf ((component) => {
					component.text = text;
					if (font != null)
						component.font = font;
				});
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
			/// you're creating your own component based on this component
			/// </remarks>
			[CustomEditor (typeof (L20nUIText))]
			public class L20nUITextEditor : Internal.L20nBaseTextEditor {}
		}
		#endif
	}
}
