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

using L20nCore.External;

namespace L20nUnity
{
	/// <summary>
	/// The <see cref="L20nUnity.HashValueBehaviour"/> class is to be inhereted from,
	/// by classes that you want to use as a <see cref="L20nCore.External.IHashValue"/>.
	/// You would inheret from HashValueBehaviour instead of just implementing IHashValue when:
	/// 	a) you would already want to inheret MonoBehaviour;
	/// 	b) you want to be able to use this hashable class within one of the components by L20nUnity;
	/// </summary>
	public abstract class HashValueBehaviour : MonoBehaviour, IHashValue
	{
		/// <summary>
		/// The method to  be overriden and to be provided for the IHashValue interface.
		/// In its body you have to collect all your class's information that is
		/// to be exposed to the L20n Language Environment within the translations where you
		/// pass this class to.
		/// </summary>
		public abstract void Collect (InfoCollector info);
	}
}
