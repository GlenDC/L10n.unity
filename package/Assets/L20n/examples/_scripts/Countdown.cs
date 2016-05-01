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
using System;

namespace L20nUnity
{
	namespace Examples
	{
		/// <summary>
		/// A simple IHashValue component used to show a simple and custom IHashValue class.
		/// It showcases how you would/can expose a class to the L20n environment.
		/// </summary>
		public class Countdown : L20nUnity.HashValueBehaviour
		{
			[SerializeField]
			int
				m_TotalSeconds;
			DateTime m_StartTime;

			void Start ()
			{
				m_StartTime = DateTime.Now;
			}

			public override void Collect (L20nCore.External.InfoCollector info)
			{
				var span = DateTime.Now - m_StartTime;
				int seconds = m_TotalSeconds - ((int)span.TotalSeconds % m_TotalSeconds);

				int hours = seconds / 3600;
				seconds -= 3600 * hours;
				int minutes = seconds / 60;
				seconds -= 60 * minutes;

				info.Add ("hours", hours);
				info.Add ("minutes", minutes);
				info.Add ("seconds", seconds);
			}
		}
	}
}
