using UnityEngine;
using System.Collections;

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

	// It's important that you initialize L20n before you start localizing anything.
	// Doing the initialization in Awake is a good option, or simply
	// making sure that your initialization script is ran first is a possibility as well.
	void Awake ()
	{
		// Make sure that L20n isn't already initialized
		if (!L20n.IsInitialized) {
			// Initialize L20n, setting the Game ID and the Manifest Path
			L20n.Initialize ("L20nExample", "L20n/examples/manifest.json");

			// Optinially you can also set the default fonts to use for Text Components
			L20n.SetFont ("jp", m_JapaneseFont);
			L20n.SetFont (m_DefaultFont);

			// Game-Specific Globals can be added as well
			// These variables are avaiable for all translations
			L20n.AddStaticGlobal("temperature", 19);
			L20n.AddComplexGlobal("user", m_User);
		}
	}
}
