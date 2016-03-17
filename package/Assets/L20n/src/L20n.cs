/**
 * This source file is part of the Commercial L20n Unity Plugin.
 * 
 * Copyright (c) 2016 - 2017 Glen De Cauwsemaecker (contact@glendc.com)
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

// some aliases for convience
using L20nVariable = L20nCore.External.UserHashValue;
using L20nInfoCollector = L20nCore.External.InfoCollector;

public static class L20n
{
	public static void Initialize(string manifest_path, string locale = null)
	{
		var core = GetCore();
		if (core.IsInitialized) {
			Debug.LogWarningFormat(
				"can't initialize L20n with manifest `{0}` as it is already initialized",
				manifest_path);
			return;
		}

		core.ImportManifest(manifest_path);

		string localeType = null;
		if (locale == null) {
			locale = LoadSetting(SETTING_USER_LOCAL);
			if (locale == null) {
				locale = SystemLocale;
				localeType = "system-defined";
			} else {
				localeType = "user-cached";
			}
		}

		try {
			if (locale != null && locale != "") {
				core.SetLocale(locale);
			}
		} catch(L20nCore.Exceptions.ImportException) {
			Debug.LogWarningFormat(
				"tried to load {0}-locale `{1}` but this is not supported by your manifest, " +
				"default locale will be used instead as been specified by your manifest.", localeType, locale);
		}
	}

	public static void SetLocale(string id)
	{
		try {
			var core = GetCore();
			core.SetLocale(id);
			SaveSetting(SETTING_USER_LOCAL, core.CurrentLocale);
		} catch(L20nCore.Exceptions.ImportException e) {
			Debug.LogWarningFormat(
				"tried to load locale `{0}` but something went wrong. More Info: \n{1}",
				id, e.ToString());
		}
	}
	
	public static string Translate(string id)
	{
		return GetCore().Translate(id);
	}
	
	public static string Translate(string id, UserVariables variables)
	{
		return GetCore().Translate(id, variables);
	}
	
	public static string Translate(string id,
	                        string parameter_key, UserVariable parameter_value)
	{
		return GetCore().Translate(id, parameter_key, parameter_value);
	}
	
	public static string Translate(string id,
	                        string parameter_key_a, UserVariable parameter_value_a,
	                        string parameter_key_b, UserVariable parameter_value_b)
	{
		return GetCore().Translate(id,
		                    parameter_key_a, parameter_value_a,
		                    parameter_key_b, parameter_value_b);
	}
	
	public static string Translate(string id,
	                        string parameter_key_a, UserVariable parameter_value_a,
	                        string parameter_key_b, UserVariable parameter_value_b,
	                        string parameter_key_c, UserVariable parameter_value_c)
	{
		return GetCore().Translate(id,
		                    parameter_key_a, parameter_value_a,
		                    parameter_key_b, parameter_value_b,
		                    parameter_key_c, parameter_value_c);
	}
	
	private delegate Translator GetCoreDelegate();

	private static Translator s_Core;
	private static GetCoreDelegate GetCore = CreateCore;

	private static Translator CreateCore()
	{
		s_Core = new Translator();
		s_Core.SetWarningDelegate(Debug.LogWarning);
		GetCore = (() => s_Core);
		AddUnityGlobals();
		return s_Core;
	}

	private static void AddUnityGlobals()
	{
		var core = GetCore();

		// expose platform info
		core.AddGlobal("os", CurrentOS);
		core.AddGlobal("platform", CurrentPlatform);

		// expose screen info
		core.AddGlobal("screen", new ScreenInfo());
	}

	private static string LoadSetting(string id)
	{
		return PlayerPrefs.GetString(SETTING_PREFIX + id, null);
	}
	
	private static void SaveSetting(string id, string value)
	{
		PlayerPrefs.SetString(SETTING_PREFIX + id, value);
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

	private static string SystemLocale
	{
		get { return s_LocaleTable[Application.systemLanguage]; }
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

	private sealed class ScreenInfo : L20nVariable
	{
		public override void Collect(L20nInfoCollector info)
		{
			info.Add("width", () => Screen.width);
			info.Add("height", () => Screen.height);
			info.Add("dpi", () => (int)Screen.dpi);
			info.Add("orientation", () => {
				switch(Screen.orientation) {
				case ScreenOrientation.Landscape: return "landscape";
				case ScreenOrientation.LandscapeRight: return "landscape";
				case ScreenOrientation.Portrait: return "portrait";
				case ScreenOrientation.PortraitUpsideDown: return "portrait";
				case ScreenOrientation.AutoRotation:
					return Screen.width > Screen.height ? "landscape" : "portrait";
				}

				return "unknown";
			});
		}
	}
}
