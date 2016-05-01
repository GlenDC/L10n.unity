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

namespace L20nUnity
{
	namespace Examples
	{
		/// <summary>
		/// An example on how you can change the locale via code.
		/// Very similar to the <see cref="SiderLocale"/> component,
		/// with just the controller type being different.
		/// </summary>
		public class SliderLocale : MonoBehaviour
		{
			private Slider m_Slider;
			private LocalizedLocaleText m_Text;

			void Start ()
			{
				m_Slider = GetComponent<Slider> ();
				m_Text = transform.GetComponentInChildren<LocalizedLocaleText> ();

				m_Slider.maxValue = L20n.Locales.Count - 1;
				for (int i = 0; i < L20n.Locales.Count; ++i) {
					if (L20n.Locales [i] != L20n.CurrentLocale) {
						continue;
					}

					var value = (float)i;
					m_Slider.value = value;
					break;
				}
			}

			public void UpdateValue (int offset)
			{
				var value = m_Slider.value + offset;
				if (value > m_Slider.maxValue) {
					m_Slider.value = m_Slider.minValue;
				} else if (value < m_Slider.minValue) {
					m_Slider.value = m_Slider.maxValue;
				} else {
					m_Slider.value = value;
				}
			}

			public void SetLocale ()
			{	
				var locale = L20n.Locales [(int)m_Slider.value];
				L20n.SetLocale (locale);
				m_Text.SetText ();
			}
		}
	}
}
