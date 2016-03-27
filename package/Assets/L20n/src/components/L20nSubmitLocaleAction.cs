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

namespace L20nUnity
{
	namespace Components
	{
		[AddComponentMenu("L20n/SubmitLocaleAction")]
		public sealed class L20nSubmitLocaleAction : MonoBehaviour
		{
			[SerializeField]
			private string m_LocaleIdentifier = null;

			void Start ()
			{
				Debug.Assert (m_LocaleIdentifier != null && m_LocaleIdentifier != "",
				             "<L20nSubmitLocale> requires a local identifier to be specified");
			}

			public void OnSubmit ()
			{
				L20n.SetLocale (m_LocaleIdentifier);
			}
		}
	}
}
