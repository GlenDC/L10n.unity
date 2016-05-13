/*
 * This source file is part of the L20n Unity Plugin.
 * Copyright (c) 2016 Glen De Cauwsemaecker (contact@glendc.com)
 */
using UnityEngine;
using UnityEngine.UI;

namespace L20nUnity
{
	namespace Examples
	{
		/// <summary>
		/// Used for the arrow buttons on the sliders used in the L20n Examples.
		/// </summary>
		public class SliderButton : MonoBehaviour
		{

			[SerializeField]
			SliderLocale m_Slider;
			[SerializeField]
			int m_Offset;

			public void OnSubmit ()
			{
				m_Slider.UpdateValue (m_Offset);
			}
		}
	}
}
