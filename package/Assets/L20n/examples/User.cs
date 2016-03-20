using UnityEngine;
using System.Collections;

public class User : L20nUnity.HashValueBehaviour {
	public string userName;
	public Gender gender;
	public int followers;

	public override void Collect(L20nCore.External.InfoCollector info)
	{
		info.Add("gender", getGenderString());
		info.Add("name", userName);
		info.Add("followers", followers);
	}

	private string getGenderString()
	{
		switch (gender) {
		case Gender.Feminine: return "feminine";
		case Gender.Masculine: return "masculine";
		default: return "default";
		}
	}

	public enum Gender
	{
		Masculine,
		Feminine,
		Unknown,
	}
}
