using UnityEngine;
using System.Collections;
using System;

public class Countdown : L20nUnity.HashValueBehaviour
{

	[SerializeField]
	int
		m_TotalSeconds;
	DateTime m_StartTime;

	void Start ()
	{
		m_StartTime = DateTime.Now;
	}

	public override void Collect (L20nCore.External.InfoCollector info)
	{
		var span = DateTime.Now - m_StartTime;
		int seconds = m_TotalSeconds - ((int)span.TotalSeconds % m_TotalSeconds);

		int hours = seconds / 3600;
		seconds -= 3600 * hours;
		int minutes = seconds / 60;
		seconds -= 60 * minutes;

		info.Add("hours", hours);
		info.Add("minutes", minutes);
		info.Add("seconds", seconds);
	}
}
