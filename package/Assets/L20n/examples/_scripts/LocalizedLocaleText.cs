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
		/// An example on how you can translate purely via code
		/// </summary>
		public class LocalizedLocaleText : MonoBehaviour
		{
			void Start ()
			{
				SetText ();
			}

			public void SetText ()
			{
				var locale = L20n.CurrentLocale;
				var text = string.Format ("{0} ({1})", L20n.Translate (locale), locale);
				GetComponent<Text> ().text = text;
			}
		}
	}
}
