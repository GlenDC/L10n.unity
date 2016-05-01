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
using System;

using L20nUnity.Components;

namespace L20nUnity
{
	namespace Examples
	{
		/// <summary>
		/// A simple Component used in the L20n examples, to change the current locale.
		/// </summary>
		public class UserChangeButton : MonoBehaviour
		{
			Button m_CurrentButton;
			ButtonToggleList m_Master;
			User m_CurrentUser;
			[SerializeField]
			L20nUIText
				m_L20nText;
			
			void OnEnable ()
			{
				m_Master = GetComponentInParent<ButtonToggleList> ();
				m_CurrentButton = GetComponent<Button> ();
				m_CurrentUser = GetComponent<User> ();
				m_CurrentButton.onClick.AddListener (OnSubmit);
			}
			
			void OnDisable ()
			{
				m_CurrentButton.onClick.RemoveListener (OnSubmit);
			}
			
			public void OnSubmit ()
			{
				m_L20nText.SetVariable ("user", m_CurrentUser);
				m_Master.SetButton (m_CurrentButton);
			}
		}
	}
}
