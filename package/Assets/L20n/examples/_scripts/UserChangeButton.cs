/*
 * This source file is part of the L20n Unity Plugin.
 * Copyright (c) 2016 Glen De Cauwsemaecker (contact@glendc.com)
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
