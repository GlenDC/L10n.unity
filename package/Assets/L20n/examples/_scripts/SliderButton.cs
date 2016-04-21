using UnityEngine;
using UnityEngine.UI;

public class SliderButton : MonoBehaviour {

	[SerializeField] SliderLocale m_Slider;
	[SerializeField] int m_Offset;

	public void OnSubmit()
	{
		m_Slider.UpdateValue(m_Offset);
	}
}
