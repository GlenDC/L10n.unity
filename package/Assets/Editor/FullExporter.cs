using UnityEditor;
using UnityEngine;
using System;

public class FullExport : MonoBehaviour
{
	[MenuItem("Tools/Full Export")]
	private static void NewMenuOption ()
	{
		var fileName = "l20n.unitypackage";
		var files = new String[] { "Assets/L20n", "Assets/Resources" };

		AssetDatabase.ExportPackage (
			files, fileName,
			ExportPackageOptions.Interactive | ExportPackageOptions.Recurse | 
			ExportPackageOptions.IncludeDependencies);
	}
}