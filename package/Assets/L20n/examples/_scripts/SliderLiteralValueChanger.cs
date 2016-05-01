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

using L20nUnity.Components;

namespace L20nUnity
{
	namespace Examples
	{
		/// <summary>
		/// Used in the L20n Examples.
		/// It showcases how you can in theory modify an external value of registered to a L20n Text Component.
		/// An external value can't be added or removed, as that doesn't make sense.
		/// </summary>
		public class SliderLiteralValueChanger : MonoBehaviour
		{
			[SerializeField]
			L20nUIText
				m_Text;
			[SerializeField]
			string
				m_VariableName;
			[SerializeField]
			Text
				m_LabelText;
			Slider m_Slider;

			void OnEnable ()
			{
				m_Slider = GetComponent<Slider> ();
				m_Slider.onValueChanged.AddListener (OnSliderChange);
				OnSliderChange (m_Slider.value);
			}

			void OnDisable ()
			{
				m_Slider.onValueChanged.RemoveListener (OnSliderChange);
			}

			void OnSliderChange (float value)
			{
				m_Slider.value = value;
				m_LabelText.text = ((int)value).ToString ();
				m_Text.SetVariable (m_VariableName, (int)value);
			}
		}
	}
}
