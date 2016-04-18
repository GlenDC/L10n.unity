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
using System;
using System.Collections.Generic;

using UnityEngine;

using L20nCore;
using L20nCore.Objects;
using L20nCore.External;

public static class L20n
{
	public delegate void EventAction ();

	public static event EventAction OnLocaleChange;

	public static string CurrentLocale {
		get { return GetCore ().CurrentLocale; }
	}

	public static string DefaultLocale {
		get { return GetCore ().DefaultLocale; }
	}
	
	public static List<string> Locales {
		get { return GetCore ().Locales; }
	}

	public static bool IsInitialized {
		get { return GetCore ().IsInitialized; }
	}

	public static Font CurrentFont {
		get {
			Font font;
			string locale = CurrentLocale;
			if (locale != null && s_Fonts.TryGetValue(locale, out font))
				return font;
			locale = DefaultLocale;
			if (locale != null && s_Fonts.TryGetValue(locale, out font))
				return font;

			return null;
		}
	}

	public static void Initialize (string game_id, string manifest_path, string overrideLocale = null)
	{
		var core = GetCore ();
		if (core.IsInitialized) {
			Debug.LogWarningFormat (
				"can't initialize L20n with manifest `{0}` as it is already initialized",
				manifest_path);
			return;
		}

		s_GameID = game_id;

		core.ImportManifest (manifest_path);

		string localeType = "dev";
		string locale = overrideLocale;
		if (locale == null) {
			locale = LoadSetting (SETTING_USER_LOCAL);
			if (locale == null) {
				locale = SystemLocale;
				localeType = "system";
			} else {
				localeType = "default";
			}
		}

		try {
			if (locale != null && locale != "") {
				core.SetLocale (locale);
			}
		} catch (L20nCore.Exceptions.ImportException) {
			Debug.LogWarningFormat (
				"tried to load {0}-locale `{1}` but this is not supported by your manifest, " +
				"default locale will be used instead as been specified by your manifest.", localeType, locale);
		}

		if (OnLocaleChange != null)
			OnLocaleChange ();
	}

	public static void SetLocale (string id)
	{
		try {
			var core = GetCore ();
			core.SetLocale (id);

			if (OnLocaleChange != null)
				OnLocaleChange ();

			SaveSetting (SETTING_USER_LOCAL, core.CurrentLocale);
		} catch (L20nCore.Exceptions.ImportException e) {
			Debug.LogWarningFormat (
				"tried to load locale `{0}` but something went wrong. More Info: \n{1}",
				id, e.ToString ());
		}
	}
	
	public static string Translate (string id)
	{
		return GetCore ().Translate (id);
	}
	
	public static string Translate (string id, string[] keys, L20nCore.Objects.L20nObject[] values)
	{
		return GetCore ().Translate (id, keys, values);
	}
	
	public static string Translate (string id,
	                        string parameter_key, int parameter_value)
	{
		return GetCore ().Translate (id, parameter_key, parameter_value);
	}
	
	public static string Translate (string id,
	                               string parameter_key, string parameter_value)
	{
		return GetCore ().Translate (id, parameter_key, parameter_value);
	}
	
	public static string Translate (string id,
	                               string parameter_key, L20nCore.External.IHashValue parameter_value)
	{
		return GetCore ().Translate (id, parameter_key, parameter_value);
	}
	
	public static string Translate (string id,
	                               string parameter_key_a, int parameter_value_a,
	                               string parameter_key_b, int parameter_value_b)
	{
		var keys = new string[] { parameter_key_a, parameter_key_b };
		var values = new L20nObject[] {
			new Literal (parameter_value_a), new Literal (parameter_value_b)
		};

		return GetCore ().Translate (id, keys, values);
	}
	
	public static string Translate (string id,
	                               string parameter_key_a, int parameter_value_a,
	                               string parameter_key_b, string parameter_value_b)
	{
		var keys = new string[] { parameter_key_a, parameter_key_b };
		var values = new L20nObject[] {
			new Literal (parameter_value_a), new StringOutput (parameter_value_b)
		};
		
		return GetCore ().Translate (id, keys, values);
	}
	
	public static string Translate (string id,
	                               string parameter_key_a, int parameter_value_a,
	                               string parameter_key_b, IHashValue parameter_value_b)
	{
		var keys = new string[] { parameter_key_a, parameter_key_b };
		var values = new L20nObject[] {
			new Literal (parameter_value_a), new Entity (parameter_value_b)
		};
		
		return GetCore ().Translate (id, keys, values);
	}
	
	public static string Translate (string id,
	                               string parameter_key_a, string parameter_value_a,
	                               string parameter_key_b, string parameter_value_b)
	{
		var keys = new string[] { parameter_key_a, parameter_key_b };
		var values = new L20nObject[] {
			new StringOutput (parameter_value_a), new StringOutput (parameter_value_b)
		};
		
		return GetCore ().Translate (id, keys, values);
	}
	
	public static string Translate (string id,
	                               string parameter_key_a, string parameter_value_a,
	                               string parameter_key_b, int parameter_value_b)
	{
		var keys = new string[] { parameter_key_a, parameter_key_b };
		var values = new L20nObject[] {
			new StringOutput (parameter_value_a), new Literal (parameter_value_b)
		};
		
		return GetCore ().Translate (id, keys, values);
	}
	
	public static string Translate (string id,
	                               string parameter_key_a, string parameter_value_a,
	                               string parameter_key_b, IHashValue parameter_value_b)
	{
		var keys = new string[] { parameter_key_a, parameter_key_b };
		var values = new L20nObject[] {
			new StringOutput (parameter_value_a), new Entity (parameter_value_b)
		};
		
		return GetCore ().Translate (id, keys, values);
	}
	
	public static string Translate (string id,
	                               string parameter_key_a, IHashValue parameter_value_a,
	                               string parameter_key_b, IHashValue parameter_value_b)
	{
		var keys = new string[] { parameter_key_a, parameter_key_b };
		var values = new L20nObject[] {
			new Entity (parameter_value_a), new Entity (parameter_value_b)
		};
		
		return GetCore ().Translate (id, keys, values);
	}
	
	public static string Translate (string id,
	                               string parameter_key_a, IHashValue parameter_value_a,
	                               string parameter_key_b, int parameter_value_b)
	{
		var keys = new string[] { parameter_key_a, parameter_key_b };
		var values = new L20nObject[] {
			new Entity (parameter_value_a), new Literal (parameter_value_b)
		};
		
		return GetCore ().Translate (id, keys, values);
	}
	
	public static string Translate (string id,
	                               string parameter_key_a, IHashValue parameter_value_a,
	                               string parameter_key_b, string parameter_value_b)
	{
		var keys = new string[] { parameter_key_a, parameter_key_b };
		var values = new L20nObject[] {
			new Entity (parameter_value_a), new StringOutput (parameter_value_b)
		};
		
		return GetCore ().Translate (id, keys, values);
	}

	public static void SetFont (string locale, Font font)
	{
		if (!Locales.Contains (locale)) {
			Debug.LogErrorFormat("can't set font as '{0}' is not a supported local", locale);
			return;
		}

		if (!s_Fonts.ContainsKey (locale)) {
			s_Fonts.Add (locale, font);
		} else {
			Debug.LogWarningFormat("locale '{0}' already has a font assigned to it", locale);
		}
	}
	
	private delegate Translator GetCoreDelegate ();

	private static string s_GameID;
	private static Translator s_Core;
	private static Dictionary<string, Font> s_Fonts;
	private static GetCoreDelegate GetCore = CreateCore;

	private static Translator CreateCore ()
	{
		L20nCore.IO.StreamReaderFactory.SetCallback (CreateStreamReader);

		s_Core = new Translator ();
		s_Core.SetWarningDelegate (Debug.LogWarning);
		GetCore = (() => s_Core);

		s_Fonts = new Dictionary<string, Font> ();

		AddUnityGlobals ();

		return s_Core;
	}

	private static void AddUnityGlobals ()
	{
		var core = GetCore ();

		// expose platform info
		core.AddGlobal ("os", CurrentOS);
		core.AddGlobal ("platform", CurrentPlatform);

		// expose screen info
		core.AddGlobal ("screen", new ScreenInfo ());
	}

	private static string LoadSetting (string id)
	{
		return PlayerPrefs.GetString (SETTING_PREFIX + s_GameID + id, null);
	}
	
	private static void SaveSetting (string id, string value)
	{
		PlayerPrefs.SetString (SETTING_PREFIX + s_GameID + id, value);
	}

	public static System.IO.StreamReader CreateStreamReader (
		string path, System.Text.Encoding encoding, bool detectEncoding)
	{
		path = System.IO.Path.ChangeExtension (path, null);
		var parts = path.Split ('/');
		var newParts = new List<string> (path.Length);
		for (int i = 0; i < parts.Length; ++i) {
			if (parts [i] == "..") {
				if (parts.Length == 0) {
					throw new L20nCore.Exceptions.ImportException (
						String.Format ("couldn't load resource '{0}', " +
						"you can't go outside the Resources folder", path));
				}
				
				newParts.RemoveAt (newParts.Count - 1);
			} else if (parts [i] != ".") {
				newParts.Add (parts [i]);
			}
		}

		if (parts.Length == 0) {
			throw new L20nCore.Exceptions.ImportException (
				String.Format ("couldn't load resource '{0}', " +
				"path results in an empty path", path));
		}
		
		path = String.Join ("/", newParts.ToArray ());
		var data = Resources.Load<TextAsset> (path);
		if (data == null) {
			throw new L20nCore.Exceptions.ImportException (
				String.Format ("couldn't load resource '{0}', " +
				"ensure that the path exists", path));
		}
		
		var stream = new System.IO.MemoryStream (data.bytes);
		return new System.IO.StreamReader (
			stream, encoding, detectEncoding);
	}

	/// <summary>
	/// A Locale Code Table based on Unity's exposed System Language
	/// </summary>
	/// <remarks>
	/// Sadly Unity doesn't expose detailed locales such as differentiating between
	/// en_US and en_UK, so that's information we can't provide automtically AFAIK.
	/// If you do know how to automatically detect this, mail me @ contact@glendc.com
	/// </remarks>
	/// <returns>the language codec of the current system language</returns>
	private static Dictionary<SystemLanguage, string> s_LocaleTable = new Dictionary<SystemLanguage, string> {
		{SystemLanguage.Afrikaans, "af"},
		{SystemLanguage.Arabic, "ar"},
		{SystemLanguage.Basque, "eu"},
		{SystemLanguage.Belarusian, "be"},
		{SystemLanguage.Bulgarian, "bg"},
		{SystemLanguage.Catalan, "ca"},
		{SystemLanguage.Chinese, "zh"},
		{SystemLanguage.Czech, "cs"},
		{SystemLanguage.Danish, "da"},
		{SystemLanguage.Dutch, "nl"},
		{SystemLanguage.Estonian, "et"},
		{SystemLanguage.Faroese, "fo"},
		{SystemLanguage.Finnish, "fi"},
		{SystemLanguage.French, "fr"},
		{SystemLanguage.German, "de"},
		{SystemLanguage.Greek, "el"},
		{SystemLanguage.Hebrew, "he"},
		{SystemLanguage.Hungarian, "hu"},
		{SystemLanguage.Icelandic, "is"},
		{SystemLanguage.Indonesian, "id"},
		{SystemLanguage.Italian, "it"},
		{SystemLanguage.Japanese, "ja"},
		{SystemLanguage.Korean, "ko"},
		{SystemLanguage.Latvian, "lv"},
		{SystemLanguage.Lithuanian, "lt"},
		{SystemLanguage.Norwegian, "nb"},
		{SystemLanguage.Polish, "pl"},
		{SystemLanguage.Portuguese, "pt"},
		{SystemLanguage.Romanian, "ro"},
		{SystemLanguage.Russian, "ru"},
		{SystemLanguage.SerboCroatian, "hr"},
		{SystemLanguage.Slovak, "sk"},
		{SystemLanguage.Slovenian, "sl"},
		{SystemLanguage.Spanish, "es"},
		{SystemLanguage.Swedish, "sv"},
		{SystemLanguage.Thai, "th"},
		{SystemLanguage.Turkish, "tr"},
		{SystemLanguage.Ukrainian, "uk"},
		{SystemLanguage.Vietnamese, "vi"},
		{SystemLanguage.ChineseSimplified, "zh_Hans"},
		{SystemLanguage.ChineseTraditional, "zh_Hant"},
		{SystemLanguage.Unknown, null},
	};
	private static string SETTING_PREFIX = "l20n-";
	private static string SETTING_USER_LOCAL = "userLocale";

	private static string SystemLocale {
		get { return s_LocaleTable [Application.systemLanguage]; }
	}

	private static string CurrentOS { get {
			#if UNITY_STANDALONE_OSX
			return "macos";
			#elif UNITY_STANDALONE_WIN
			return "windows";
			#elif UNITY_STANDALONE_LINUX
			return "linux";
			#elif UNITY_WEBPLAYER
			return "web";
			#elif UNITY_WII
			return "wii";
			#elif UNITY_IOS
			return "ios";
			#elif UNITY_ANDROID
			return "android";
			#elif UNITY_PS3
			return "ps3";
			#elif UNITY_PS4
			return "ps4";
			#elif UNITY_XBOX360
			return "xbox360";
			#elif UNITY_XBOXONE
			return "xbox360";
			#elif UNITY_WP8 || UNITY_WP8_1 || UNITY_WINRT_8_0 || UNITY_WINRT_8_1 || UNITY_WINRT_10_0
			return "winphone";
			#else
			#warning current target OS is not recognized - report this to `contact@glendc.com`
			return "unknown";
			#endif
		} }

	private static string CurrentPlatform { get {
			#if UNITY_STANDALONE
			return "desktop";
			#elif UNITY_WEBPLAYER
			return "web";
			#elif UNITY_WII || UNITY_PS3 || UNITY_PS4 || UNITY_XBOX360 || UNITY_XBOXONE
			return "console";
			#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_WP8_1 || UNITY_WINRT_8_0 || UNITY_WINRT_8_1 || UNITY_WINRT_10_0
			return "mobile";
			#else
			#warning current target platform is not recognized - report this to `contact@glendc.com`
			return "unknown";
			#endif
		} }

	private sealed class ScreenInfo : L20nCore.External.IHashValue
	{
		public void Collect (L20nCore.External.InfoCollector info)
		{
			info.Add ("width", () => Screen.width);
			info.Add ("height", () => Screen.height);
			info.Add ("dpi", () => (int)Screen.dpi);
			info.Add ("orientation", () => {
				switch (Screen.orientation) {
				case ScreenOrientation.Landscape:
					return "landscape";
				case ScreenOrientation.LandscapeRight:
					return "landscape";
				case ScreenOrientation.Portrait:
					return "portrait";
				case ScreenOrientation.PortraitUpsideDown:
					return "portrait";
				case ScreenOrientation.AutoRotation:
					return Screen.width > Screen.height ? "landscape" : "portrait";
				}

				return "unknown";
			});
		}
	}
}
