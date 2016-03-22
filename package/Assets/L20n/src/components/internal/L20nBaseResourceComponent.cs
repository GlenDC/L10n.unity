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

using System;
using L20nCore.Utils;

namespace L20nUnity
{
	namespace Components
	{
		namespace Internal
		{
			public abstract class L20nBaseResourceComponent<T, U, V>
				: L20nBaseResource<U, V>
					where V: L20nResourceCollection<U>
			{
				protected Option<T> m_Component;
				protected Option<T> Component
				{
					get
					{
						if(!m_Component.IsSet) {
							var text = GetComponent<T>();
							if(text != null) {
								m_Component.Set(text);
							}
						}
						
						return m_Component;
					}
				}
				
				public L20nBaseResourceComponent()
				{
					m_Component = new Option<T>();
				}
				
				protected override void Initialize()
				{
					Debug.Assert(Component.IsSet,
						"{0} requires a {1} to be attached",
					    GetType(), typeof(T));
				}
			}
		}
	}
}
