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
using System.Collections;

namespace L20nUnity
{
	namespace Examples
	{
		/// <summary>
		/// Used in the L20n Examples to showcase how you would L20n yourself.
		/// This is possible in case you can't or don't want to use the <c>L20nSettings</c> component to do so.
		/// </summary>
		public class LocalizitationManager : MonoBehaviour
		{
			[SerializeField]
			Font
				m_DefaultFont;
			[SerializeField]
			Font
				m_JapaneseFont;
			[SerializeField]
			User
				m_User;
			[SerializeField]
			[Tooltip("In Celcius")]
			int
				m_Temperature;

			// It's important that you initialize L20n before you start localizing anything.
			// Doing the initialization in Awake is a good option, or simply
			// making sure that your initialization script is ran first is a possibility as well.
			void Awake ()
			{
				// Make sure that L20n isn't already initialized
				if (!L20n.IsInitialized) {
					// Initialize L20n, setting the Game ID and the Manifest Path
					// this line is the only required method call to initialize L20n!
					L20n.Initialize ("L20nDemo", "L20n/examples/manifest.json");

					// ALL STUFF BELOW IS OPTIONAL!

					// Optinially you can also set the default fonts to use for Text Components
					L20n.SetFont ("jp", m_JapaneseFont);
					L20n.SetFont (m_DefaultFont);

					// Game-Specific Globals can be added as well
					// These variables are avaiable for all translations
					L20n.AddStaticGlobal ("temperature", m_Temperature);
					L20n.AddComplexGlobal ("user", m_User);
				}
			}
		}
	}
}
