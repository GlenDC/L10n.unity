/*
 * This source file is part of the L20n Unity Plugin.
 * Copyright (c) 2016 Glen De Cauwsemaecker (contact@glendc.com)
 */
using UnityEngine;
using UnityEngine.UI;

namespace L20nUnity
{
	namespace Components
	{
		/// <summary>
		/// A component that can be used in combination with a `UI/Button`.,
		/// such that you can assign this component's `OnSubmit` function
		/// as the action to be executed when the button gets pressed.
		/// 
		/// The locale will be changed to the one specified by this component.
		/// </summary>
		/// <remarks>
		/// You can use this component as the template for any other controller
		/// you may wish to give these powers to.
		/// </remarks>
		[AddComponentMenu("L20n/SubmitLocaleAction")]
		public sealed class L20nSubmitLocaleAction : MonoBehaviour
		{
			[SerializeField]
			private string
				m_LocaleIdentifier = null;

			/// <summary>
			/// Asserts the locale has been set.
			/// </summary>
			void Start ()
			{
				if (m_LocaleIdentifier == null && m_LocaleIdentifier == "")
					Debug.LogError ("<L20nSubmitLocale> requires a local identifier to be specified");
			}

			/// <summary>
			/// Register the `OnSubmit` function as a listener
			/// of the `OnClick` event.
			/// </summary>
			void OnEnable ()
			{
				var btn = GetComponent<Button> ();
				if (btn == null) {
					Debug.LogError ("<L20nSubmitLocale> requires a <UnityEngine.UI.Button> to be specified");
					return;
				}
				btn.onClick.AddListener (OnSubmit);
			}

			/// <summary>
			/// Unregister the `OnSubmit` function as a listener
			/// of the `OnClick` event.
			/// </summary>
			void OnDisable ()
			{
				var btn = GetComponent<Button> ();
				if (btn == null) {
					Debug.LogError ("<L20nSubmitLocale> requires a <UnityEngine.UI.Button> to be specified");
					return;
				}
				btn.onClick.RemoveListener (OnSubmit);
			}

			/// <summary>
			/// Called when the button gets clicked.
			/// Sets the L20n locale to the one specified in this component.
			/// </summary>
			public void OnSubmit ()
			{
				L20n.SetLocale (m_LocaleIdentifier);
			}
		}
	}
}
