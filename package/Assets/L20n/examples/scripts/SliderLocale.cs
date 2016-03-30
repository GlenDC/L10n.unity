using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// An example on how you can change the locale via code.
/// </summary>
public class SliderLocale : MonoBehaviour
{
	private Slider m_Slider;
	private LocalizedLocaleText m_Text;

	void OnEnable ()
	{
		m_Slider = GetComponent<Slider> ();
		m_Text = transform.GetComponentInChildren<LocalizedLocaleText> ();

		m_Slider.maxValue = L20n.Locales.Count - 1;
		for (int i = 0; i < L20n.Locales.Count; ++i) {
			if (L20n.Locales [i] != L20n.CurrentLocale) {
				continue;
			}

			var value = (float)i;
			m_Slider.value = value;
			break;
		}
	}

	public void UpdateValue (int offset)
	{
		var value = m_Slider.value + offset;
		if (value > m_Slider.maxValue) {
			m_Slider.value = m_Slider.minValue;
		} else if (value < m_Slider.minValue) {
			m_Slider.value = m_Slider.maxValue;
		} else {
			m_Slider.value = value;
		}
	}

	public void SetLocale ()
	{	
		var locale = L20n.Locales [(int)m_Slider.value];
		L20n.SetLocale (locale);
		m_Text.SetText ();
	}
}
