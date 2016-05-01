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

namespace L20nUnity
{
	namespace Examples
	{
		/// <summary>
		/// A simple helper class that allows us to have a group of buttons
		/// that behave like one big toggle controller.
		/// This is just an helper used in the L20n Examples and doesn't
		/// showcase any L20n feature.
		/// </summary>
		public class ButtonToggleList : MonoBehaviour
		{
			[SerializeField]
			Button
				m_CurrentButton;

			void Start ()
			{
				ToggleButtons ();
			}

			public void SetButton (Button btn)
			{
				m_CurrentButton = btn;
				ToggleButtons ();
			}

			void ToggleButtons ()
			{
				foreach (var btn in gameObject.GetComponentsInChildren<Button>()) {
					btn.interactable = btn != m_CurrentButton;
				}
			}
		}
	}
}
