using UnityEngine;
using UnityEngine.UI;

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
		var text = string.Format("{0} ({1})", L20n.Translate (locale), locale);
		GetComponent<Text> ().text = text;
	}
}
