using UnityEngine;
using UnityEditor;

public class L20nControlPanel : EditorWindow {
	[MenuItem ("L20n/Control Panel")]
	static void Init () {
		GetWindow <L20nControlPanel>();
	}

	void OnGUI () {
		GUILayout.Label ("L20n Control Panel", EditorStyles.boldLabel);
	}
}
