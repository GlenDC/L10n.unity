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
