using UnityEditor;
using UnityEngine;
using System;

public class FullExport : MonoBehaviour
{
	const string version = "1.0";
	[MenuItem("Tools/Full Export")]
	private static void NewMenuOption ()
	{
		var fileName = String.Format("l20n_v{0}.unitypackage", version);
		var files = new String[] { "Assets/L20n" };
		
		AssetDatabase.ExportPackage (
			files, fileName,
			ExportPackageOptions.Interactive | ExportPackageOptions.Recurse | 
			ExportPackageOptions.IncludeDependencies);
	}
}