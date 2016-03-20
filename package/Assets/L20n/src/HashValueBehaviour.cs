using UnityEngine;

using L20nCore.External;

namespace L20nUnity
{
	public abstract class HashValueBehaviour : MonoBehaviour, IHashValue {
		public abstract void Collect(InfoCollector info);
	}
}
