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

/// <summary>
/// The API to be used to interact with the L20n Localization System.
/// You could translate your entire game just using this class.
/// All components are there as an extra helper,
/// but are just yet another high level layer on this main layer,
/// which is already high level on its own.
/// </summary>
/// <remarks>
/// This API wraps around the L20nCore library (A custom L20n implementation in C#) that's available as
/// an opensource C# Library at GitHub and developed by Glen De Cauwsemaecker.
/// See for more information @ https://github.com/glendc/l20n.cs
/// </remarks>
public static class L20n
{
	/// <summary>
	/// A simple function that is used as a callback when an event occurs.
	/// </summary>
	public delegate void EventAction ();

	/// <summary>
	/// Methods that need to be called when the Locale changes,
	/// have to register to this EventAction.
	/// Don't forget to unregister when your object goes out of scope.
	/// </summary>
	public static event EventAction OnLocaleChange;

	/// <summary>
	/// The current locale that's to be used by translations.
	/// This is either a specifically set locale or the default locale.
	/// This value can be <c>null</c> when L20n has not be initialized.
	/// </summary>
	public static string CurrentLocale {
		get { return GetCore ().CurrentLocale; }
	}

	/// <summary>
	/// The default locale that's be used as a fallback by translations.
	/// This value can be <c>null</c> when L20n has not be initialized.
	/// </summary>
	public static string DefaultLocale {
		get { return GetCore ().DefaultLocale; }
	}

	/// <summary>
	/// A list of all locales as defined by the manifest given during initialization of L20n.
	/// This list will be empty when L20n isn't initialized yet.
	/// </summary>
	public static List<string> Locales {
		get { return GetCore ().Locales; }
	}

	/// <summary>
	/// Gets a value indicating L20n is initialized.
	/// This property should /always/ be checked before trying to access another property.
	/// L20n's Static Methods will handle an unitialized state gracefully and therefore don't require this.
	/// </summary>
	/// <value><c>true</c> if is initialized; otherwise, <c>false</c>.</value>
	public static bool IsInitialized {
		get { return GetCore ().IsInitialized; }
	}

	/// <summary>
	/// Gets the font that's to be used as the default font for text components based on the CurrentLocale.
	/// This value can be null in case the current locale and default locale both have no Font set.
	/// </summary>
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

	/// <summary>
	/// Initialize L20n, giving the <c>game_id</c>, <c>manifest_path</c> and an optional
	/// <c>overrideLocale</c>, which should only be used for development purposes.
	/// Note that this function can only be called once.
	/// </summary>
	/// <param name="game_id">
	/// An id that should be unique to your game.
	/// This prevents namespace clashes on development environments.
	/// </param>
	/// <param name="manifest_path">The /resource/ path to the manifest file, starting in the resource root folder.</param>
	/// <param name="overrideLocale">
	/// an optional parameter that can be used to override the locale to be set at start-up,
	/// ignoring the user-selected locale-preference, if one would have been available
	/// </param>
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

	/// <summary>
	/// Set the current locale, using the given <c>id</c>.
	/// Note that this can't be set if L20n isn't initialized yet, or
	/// if the locale isn't listed in the given L20n manifest.
	/// <c>OnLocaleChange</c> will be triggered IFF the requested locale has been set successfully.
	/// </summary>
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

	/// <summary>
	/// Get the translation (entity) for the current locale, with the default locale as a fallback.
	/// This method will return the given id if it could not be translated or found.
	/// </summary>
	public static string Translate (string id)
	{
		return GetCore ().Translate (id);
	}

	/// <summary>
	/// Get the translation (entity) for the current locale, with the default locale as a fallback.
	/// This method will return the given id if it could not be translated or found.
	/// The given <c>values</c> will get bound with the given <c>keys</c> as external values ($-syntax),
	/// and will be available during the translation of the given <c>id</c>.
	/// </summary>
	/// <remarks>
	/// Note that the <c>id</c> can be a simple identifier or a property-expression.
	/// Meaning that you can have something like `foo` but also `foo.bar` or `foo.bar.baz`.
	/// A property expression uses the dot `.` syntax and allows you to specify values within your
	/// hash-tables used as values in your Entity. The default or index will be used in case a property
	/// identifier couldn't be found.
	/// For string-values this extra property identifier will simply be ignored.
	/// Meaning that something like `foo.bar` would be equal to `foo`.
	/// </remarks>
	public static string Translate (string id, string[] keys, L20nCore.Objects.L20nObject[] values)
	{
		return GetCore ().Translate (id, keys, values);
	}

	/// <summary>
	/// Get the translation (entity) for the current locale, with the default locale as a fallback.
	/// This method will return the given id if it could not be translated or found.
	/// The given <c>parameter_value</c> will get bound with the given <c>parameter_key</c> as an external value ($-syntax),
	/// and will be available during the translation of the given <c>id</c>.
	/// </summary>
	/// <remarks>
	/// Note that the <c>id</c> can be a simple identifier or a property-expression.
	/// Meaning that you can have something like `foo` but also `foo.bar` or `foo.bar.baz`.
	/// A property expression uses the dot `.` syntax and allows you to specify values within your
	/// hash-tables used as values in your Entity. The default or index will be used in case a property
	/// identifier couldn't be found.
	/// For string-values this extra property identifier will simply be ignored.
	/// Meaning that something like `foo.bar` would be equal to `foo`.
	/// </remarks>
	public static string Translate (string id,
	                        string parameter_key, int parameter_value)
	{
		return GetCore ().Translate (id, parameter_key, parameter_value);
	}

	/// <summary>
	/// Get the translation (entity) for the current locale, with the default locale as a fallback.
	/// This method will return the given id if it could not be translated or found.
	/// The given <c>parameter_value</c> will get bound with the given <c>parameter_key</c> as an external value ($-syntax),
	/// and will be available during the translation of the given <c>id</c>.
	/// </summary>
	/// <remarks>
	/// Note that the <c>id</c> can be a simple identifier or a property-expression.
	/// Meaning that you can have something like `foo` but also `foo.bar` or `foo.bar.baz`.
	/// A property expression uses the dot `.` syntax and allows you to specify values within your
	/// hash-tables used as values in your Entity. The default or index will be used in case a property
	/// identifier couldn't be found.
	/// For string-values this extra property identifier will simply be ignored.
	/// Meaning that something like `foo.bar` would be equal to `foo`.
	/// </remarks>
	public static string Translate (string id,
	                               string parameter_key, string parameter_value)
	{
		return GetCore ().Translate (id, parameter_key, parameter_value);
	}

	/// <summary>
	/// Get the translation (entity) for the current locale, with the default locale as a fallback.
	/// This method will return the given id if it could not be translated or found.
	/// The given <c>parameter_value</c> will get bound with the given <c>parameter_key</c> as an external value ($-syntax),
	/// and will be available during the translation of the given <c>id</c>.
	/// </summary>
	/// <remarks>
	/// Note that the <c>id</c> can be a simple identifier or a property-expression.
	/// Meaning that you can have something like `foo` but also `foo.bar` or `foo.bar.baz`.
	/// A property expression uses the dot `.` syntax and allows you to specify values within your
	/// hash-tables used as values in your Entity. The default or index will be used in case a property
	/// identifier couldn't be found.
	/// For string-values this extra property identifier will simply be ignored.
	/// Meaning that something like `foo.bar` would be equal to `foo`.
	/// </remarks>
	public static string Translate (string id,
	                               string parameter_key, L20nCore.External.IHashValue parameter_value)
	{
		return GetCore ().Translate (id, parameter_key, parameter_value);
	}

	/// <summary>
	/// Get the translation (entity) for the current locale, with the default locale as a fallback.
	/// This method will return the given id if it could not be translated or found.
	/// The given <c>parameter_values</c> will get bound with the given <c>parameter_kesy</c> as external values ($-syntax),
	/// and will be available during the translation of the given <c>id</c>.
	/// </summary>
	/// <remarks>
	/// Note that the <c>id</c> can be a simple identifier or a property-expression.
	/// Meaning that you can have something like `foo` but also `foo.bar` or `foo.bar.baz`.
	/// A property expression uses the dot `.` syntax and allows you to specify values within your
	/// hash-tables used as values in your Entity. The default or index will be used in case a property
	/// identifier couldn't be found.
	/// For string-values this extra property identifier will simply be ignored.
	/// Meaning that something like `foo.bar` would be equal to `foo`.
	/// </remarks>
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

	/// <summary>
	/// Get the translation (entity) for the current locale, with the default locale as a fallback.
	/// This method will return the given id if it could not be translated or found.
	/// The given <c>parameter_values</c> will get bound with the given <c>parameter_kesy</c> as external values ($-syntax),
	/// and will be available during the translation of the given <c>id</c>.
	/// </summary>
	/// <remarks>
	/// Note that the <c>id</c> can be a simple identifier or a property-expression.
	/// Meaning that you can have something like `foo` but also `foo.bar` or `foo.bar.baz`.
	/// A property expression uses the dot `.` syntax and allows you to specify values within your
	/// hash-tables used as values in your Entity. The default or index will be used in case a property
	/// identifier couldn't be found.
	/// For string-values this extra property identifier will simply be ignored.
	/// Meaning that something like `foo.bar` would be equal to `foo`.
	/// </remarks>
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

	/// <summary>
	/// Get the translation (entity) for the current locale, with the default locale as a fallback.
	/// This method will return the given id if it could not be translated or found.
	/// The given <c>parameter_values</c> will get bound with the given <c>parameter_kesy</c> as external values ($-syntax),
	/// and will be available during the translation of the given <c>id</c>.
	/// </summary>
	/// <remarks>
	/// Note that the <c>id</c> can be a simple identifier or a property-expression.
	/// Meaning that you can have something like `foo` but also `foo.bar` or `foo.bar.baz`.
	/// A property expression uses the dot `.` syntax and allows you to specify values within your
	/// hash-tables used as values in your Entity. The default or index will be used in case a property
	/// identifier couldn't be found.
	/// For string-values this extra property identifier will simply be ignored.
	/// Meaning that something like `foo.bar` would be equal to `foo`.
	/// </remarks>
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

	/// <summary>
	/// Get the translation (entity) for the current locale, with the default locale as a fallback.
	/// This method will return the given id if it could not be translated or found.
	/// The given <c>parameter_values</c> will get bound with the given <c>parameter_kesy</c> as external values ($-syntax),
	/// and will be available during the translation of the given <c>id</c>.
	/// </summary>
	/// <remarks>
	/// Note that the <c>id</c> can be a simple identifier or a property-expression.
	/// Meaning that you can have something like `foo` but also `foo.bar` or `foo.bar.baz`.
	/// A property expression uses the dot `.` syntax and allows you to specify values within your
	/// hash-tables used as values in your Entity. The default or index will be used in case a property
	/// identifier couldn't be found.
	/// For string-values this extra property identifier will simply be ignored.
	/// Meaning that something like `foo.bar` would be equal to `foo`.
	/// </remarks>
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

	/// <summary>
	/// Get the translation (entity) for the current locale, with the default locale as a fallback.
	/// This method will return the given id if it could not be translated or found.
	/// The given <c>parameter_values</c> will get bound with the given <c>parameter_kesy</c> as external values ($-syntax),
	/// and will be available during the translation of the given <c>id</c>.
	/// </summary>
	/// <remarks>
	/// Note that the <c>id</c> can be a simple identifier or a property-expression.
	/// Meaning that you can have something like `foo` but also `foo.bar` or `foo.bar.baz`.
	/// A property expression uses the dot `.` syntax and allows you to specify values within your
	/// hash-tables used as values in your Entity. The default or index will be used in case a property
	/// identifier couldn't be found.
	/// For string-values this extra property identifier will simply be ignored.
	/// Meaning that something like `foo.bar` would be equal to `foo`.
	/// </remarks>
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

	/// <summary>
	/// Get the translation (entity) for the current locale, with the default locale as a fallback.
	/// This method will return the given id if it could not be translated or found.
	/// The given <c>parameter_values</c> will get bound with the given <c>parameter_kesy</c> as external values ($-syntax),
	/// and will be available during the translation of the given <c>id</c>.
	/// </summary>
	/// <remarks>
	/// Note that the <c>id</c> can be a simple identifier or a property-expression.
	/// Meaning that you can have something like `foo` but also `foo.bar` or `foo.bar.baz`.
	/// A property expression uses the dot `.` syntax and allows you to specify values within your
	/// hash-tables used as values in your Entity. The default or index will be used in case a property
	/// identifier couldn't be found.
	/// For string-values this extra property identifier will simply be ignored.
	/// Meaning that something like `foo.bar` would be equal to `foo`.
	/// </remarks>
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

	/// <summary>
	/// Get the translation (entity) for the current locale, with the default locale as a fallback.
	/// This method will return the given id if it could not be translated or found.
	/// The given <c>parameter_values</c> will get bound with the given <c>parameter_kesy</c> as external values ($-syntax),
	/// and will be available during the translation of the given <c>id</c>.
	/// </summary>
	/// <remarks>
	/// Note that the <c>id</c> can be a simple identifier or a property-expression.
	/// Meaning that you can have something like `foo` but also `foo.bar` or `foo.bar.baz`.
	/// A property expression uses the dot `.` syntax and allows you to specify values within your
	/// hash-tables used as values in your Entity. The default or index will be used in case a property
	/// identifier couldn't be found.
	/// For string-values this extra property identifier will simply be ignored.
	/// Meaning that something like `foo.bar` would be equal to `foo`.
	/// </remarks>
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

	/// <summary>
	/// Get the translation (entity) for the current locale, with the default locale as a fallback.
	/// This method will return the given id if it could not be translated or found.
	/// The given <c>parameter_values</c> will get bound with the given <c>parameter_kesy</c> as external values ($-syntax),
	/// and will be available during the translation of the given <c>id</c>.
	/// </summary>
	/// <remarks>
	/// Note that the <c>id</c> can be a simple identifier or a property-expression.
	/// Meaning that you can have something like `foo` but also `foo.bar` or `foo.bar.baz`.
	/// A property expression uses the dot `.` syntax and allows you to specify values within your
	/// hash-tables used as values in your Entity. The default or index will be used in case a property
	/// identifier couldn't be found.
	/// For string-values this extra property identifier will simply be ignored.
	/// Meaning that something like `foo.bar` would be equal to `foo`.
	/// </remarks>
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

	/// <summary>
	/// Get the translation (entity) for the current locale, with the default locale as a fallback.
	/// This method will return the given id if it could not be translated or found.
	/// The given <c>parameter_values</c> will get bound with the given <c>parameter_kesy</c> as external values ($-syntax),
	/// and will be available during the translation of the given <c>id</c>.
	/// </summary>
	/// <remarks>
	/// Note that the <c>id</c> can be a simple identifier or a property-expression.
	/// Meaning that you can have something like `foo` but also `foo.bar` or `foo.bar.baz`.
	/// A property expression uses the dot `.` syntax and allows you to specify values within your
	/// hash-tables used as values in your Entity. The default or index will be used in case a property
	/// identifier couldn't be found.
	/// For string-values this extra property identifier will simply be ignored.
	/// Meaning that something like `foo.bar` would be equal to `foo`.
	/// </remarks>
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

	/// <summary>
	/// This function should be called after the L20n Initialization, but before any translation, if possible.
	/// Even though it's safe to call it at any time during runtime, it is not recommended.
	/// Fonts can't be overriden and an error will be logged if you try.
	/// </summary>
	public static void SetFont (string locale, Font font)
	{
		if (locale == null) {
			Debug.LogError("font couldn't be set with the key as a null-value");
			return;
		}

		if (font == null) {
			Debug.LogErrorFormat("a null-value can't be used as the font for locale '{0}'", locale);
			return;
		}

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

	/// <summary>
	/// This function should be called after the L20n Initialization, but before any translation, if possible.
	/// Even though it's safe to call it at any time during runtime, it is not recommended.
	/// Sets the font for the DefaultLocale, note that an error will logged if the DefaultLocale
	/// already has a font set. You can't override that font!
	/// </summary>
	public static void SetFont (Font font)
	{
		SetFont (DefaultLocale, font);
	}

	/// <summary>
	/// This function should be called after the L20n Initialization, but before any translation, if possible.
	/// Even though it's safe to call it at any time during runtime, it is not recommended.
	/// Globals are available to all locales and are accessible within the L20n Language via the @-syntax.
	/// This function stores the given <c>value</c> as a global value with the given <c>id</c> as its name. 
	/// </summary>
	public static void AddGlobal (string id, L20nObject value)
	{
		GetCore ().AddGlobal (id, value);
	}

	/// <summary>
	/// This function should be called after the L20n Initialization, but before any translation, if possible.
	/// Even though it's safe to call it at any time during runtime, it is not recommended.
	/// Globals are available to all locales and are accessible within the L20n Language via the @-syntax.
	/// This function stores the given <c>value</c> as a global value with the given <c>id</c> as its name. 
	/// </summary>
	public static void AddStaticGlobal (string id, int value)
	{
		GetCore ().AddGlobal (id, value);
	}

	/// <summary>
	/// This function should be called after the L20n Initialization, but before any translation, if possible.
	/// Even though it's safe to call it at any time during runtime, it is not recommended.
	/// Globals are available to all locales and are accessible within the L20n Language via the @-syntax.
	/// This function stores the given <c>value</c> as a global value with the given <c>id</c> as its name. 
	/// </summary>
	public static void AddStaticGlobal (string id, string value)
	{
		GetCore ().AddGlobal (id, value);
	}

	/// <summary>
	/// This function should be called after the L20n Initialization, but before any translation, if possible.
	/// Even though it's safe to call it at any time during runtime, it is not recommended.
	/// Globals are available to all locales and are accessible within the L20n Language via the @-syntax.
	/// This function stores the given <c>value</c> as a global value with the given <c>id</c> as its name. 
	/// </summary>
	public static void AddStaticGlobal (string id, bool value)
	{
		GetCore ().AddGlobal (id, value);
	}

	/// <summary>
	/// This function should be called after the L20n Initialization, but before any translation, if possible.
	/// Even though it's safe to call it at any time during runtime, it is not recommended.
	/// Globals are available to all locales and are accessible within the L20n Language via the @-syntax.
	/// This function stores the given <c>value</c> as a global value with the given <c>id</c> as its name. 
	/// </summary>
	public static void AddDelegatedGlobal (string id, DelegatedLiteral.Delegate value)
	{
		GetCore ().AddGlobal (id, value);
	}

	/// <summary>
	/// This function should be called after the L20n Initialization, but before any translation, if possible.
	/// Even though it's safe to call it at any time during runtime, it is not recommended.
	/// Globals are available to all locales and are accessible within the L20n Language via the @-syntax.
	/// This function stores the given <c>value</c> as a global value with the given <c>id</c> as its name. 
	/// </summary>
	public static void AddDelegatedGlobal (string id, DelegatedString.Delegate value)
	{
		GetCore ().AddGlobal (id, value);
	}

	/// <summary>
	/// This function should be called after the L20n Initialization, but before any translation, if possible.
	/// Even though it's safe to call it at any time during runtime, it is not recommended.
	/// Globals are available to all locales and are accessible within the L20n Language via the @-syntax.
	/// This function stores the given <c>value</c> as a global value with the given <c>id</c> as its name. 
	/// </summary>
	public static void AddDelegatedGlobal (string id, DelegatedBoolean.Delegate value)
	{
		GetCore ().AddGlobal (id, value);
	}

	/// <summary>
	/// This function should be called after the L20n Initialization, but before any translation, if possible.
	/// Even though it's safe to call it at any time during runtime, it is not recommended.
	/// Globals are available to all locales and are accessible within the L20n Language via the @-syntax.
	/// This function stores the given <c>value</c> as a global value with the given <c>id</c> as its name. 
	/// </summary>
	public static void AddComplexGlobal (string id, IHashValue value)
	{
		GetCore ().AddGlobal (id, value);
	}

	// A simple delegate to implement a more efficient Singleton implementation.
	// This allows us to make it behave more like a state machine, without
	// any need to check if the core Translator already exists.
	private delegate Translator GetCoreDelegate ();

	// The current game id to be used to prevent conflicts in L20n stored user-preferences.
	// The only preference we store at the moment is the current Locale (can be null).
	private static string s_GameID;
	// The L20nCore logic contained in this 1 static value.
	// It is crated lazely the first time it is needed.
	// This should be the moment where you call `L20n.Initialize(...)`.
	private static Translator s_Core;
	// The dictionary of fonts, each locale can have 1 font assigned to it,
	// that is than accessible via `CurrentFont` when that locale is the `CurrentLocale`.
	private static Dictionary<string, Font> s_Fonts;
	// The delegate to be used to Create and Get the L20nCore object,
	// containing all the actual core L20n logic.
	private static GetCoreDelegate GetCore = CreateCore;

	// The initial `GetCore` logic. It creates and setups the `Translator` object used in this plugin.
	// This also means that we redirect the logs to the Unity Console and set up all globals
	// that are unique to `Unity`, rather than being specific to the `.NET` environment.
	// After that it switches the `GetCore` logic to a simply returner function.
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

	// The function to be used to add all globals that are unique to the
	// `Unity` environment and could potentially be useful information for the
	// translators and add more context to a translation.
	private static void AddUnityGlobals ()
	{
		var core = GetCore ();

		// expose platform info
		core.AddGlobal ("os", CurrentOS);
		core.AddGlobal ("platform", CurrentPlatform);

		// expose screen info
		core.AddGlobal ("screen", new ScreenInfo ());
	}

	// A private function used to load a L20n user-preference based
	// on a unique id that's linked to that preference.
	private static string LoadSetting (string id)
	{
		return PlayerPrefs.GetString (SETTING_PREFIX + s_GameID + id, null);
	}

	// A private function used to safe a L20n user-preference based
	// on a unique id that's linked to that preference.
	private static void SaveSetting (string id, string value)
	{
		PlayerPrefs.SetString (SETTING_PREFIX + s_GameID + id, value);
	}

	// With unity we're not as free as in C#, as to how we can store our Localization files.
	// It is therefore needed that all files are stored in the Unity `Resources` folder.
	// This custom `CreateStreamReader` function contains all the logic that makes
	// the loading of resources (manifest + localization files) work within the `Unity` environment.
	// This also means that we can only use extensions that are recognized by Unity.
	// Which is why we need to use '.bytes' for localization files and '.json' for the manifest.
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
	/// You can also mail me in case a mistake has been made in this error-prone table.
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

	// The prefix used together with the game-id and the preference-id
	// to store any L20n-user-preference.
	private static string SETTING_PREFIX = "l20n-";
	// The preference-id used to store the locale as preferenced and set by the user.
	private static string SETTING_USER_LOCAL = "userLocale";

	// A helper private property to get the locale based
	// on the SystemLanguage exposed by Unity.
	// Note that this doesn't go into region-specific locales and
	// is therefore very high-level and perhaps not what you want.
	private static string SystemLocale {
		get { return s_LocaleTable [Application.systemLanguage]; }
	}

	// A property that exposes a string value, identifying the current host operating system.
	// This is the value that can than be checked for via `@os` within
	// all your L20n Localization resource files.
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

	// A property that exposes a string value, identifying the current platform.
	// This is the value that can than be checked for via `@platform` within
	// all your L20n Localization resource files.
	// It is very similar to `CurrentOS` but a little more high-level,
	// and perhaps more recommended than `@OS` as it indicates more the difference that
	// you in most cases will care about. (e.g. such as how the user interacts with your game)
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

	// A global HashValue accessible via `@screen` that exposes all the information
	// you may care about during your translations.
	// This allows you to have responsive design baked in your translations,
	// keeping your game logic, well uuh... about your game! Yay!
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
