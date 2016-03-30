using UnityEngine;
using System;

public class User : L20nUnity.HashValueBehaviour
{
	[SerializeField]
	Gender m_Gender;

	public enum Gender
	{
		Masculine,
		Feminine,
		Default,
	}

	public override void Collect (L20nCore.External.InfoCollector info)
	{
		switch (m_Gender) {
		case Gender.Default:
			info.Add ("gender", "default");
			info.Add ("name", () => L20n.Translate ("neutral_user_name"));
			break;

		case Gender.Feminine:
			info.Add ("gender", "feminine");
			info.Add ("name", () => L20n.Translate ("feminine_user_name"));
			break;
			
		case Gender.Masculine:
			info.Add ("gender", "masculine");
			info.Add ("name", () => L20n.Translate ("masculine_user_name"));
			break;
		}
	}
}
