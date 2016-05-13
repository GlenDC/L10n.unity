/*
 * This source file is part of the L20n Unity Plugin.
 * Copyright (c) 2016 Glen De Cauwsemaecker (contact@glendc.com)
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
