using UnityEngine;
using UnityEngine.UI;
using System;

public class ButtonToggleList : MonoBehaviour
{
	[SerializeField]
	Button m_CurrentButton;

	void Start ()
	{
		ToggleButtons ();
	}

	public void SetButton (Button btn)
	{
		m_CurrentButton = btn;
		ToggleButtons ();
	}

	void ToggleButtons ()
	{
		foreach (var btn in gameObject.GetComponentsInChildren<Button>()) {
			btn.interactable = btn != m_CurrentButton;
		}
	}
}
