using UnityEngine;
using UnityEngine.UI;

using L20nUnity.Components;

public class SliderLiteralValueChanger : MonoBehaviour
{
	[SerializeField]
	L20nUIText m_Text;
	[SerializeField]
	string m_VariableName;
	[SerializeField]
	Text m_LabelText;

	Slider m_Slider;

	void OnEnable () {
		m_Slider = GetComponent<Slider> ();
		m_Slider.onValueChanged.AddListener (OnSliderChange);
		OnSliderChange (m_Slider.value);
	}

	void OnDisable () {
		m_Slider.onValueChanged.RemoveListener (OnSliderChange);
	}

	void OnSliderChange (float value) {
		m_Slider.value = value;
		m_LabelText.text = ((int)value).ToString ();
		m_Text.SetVariable (m_VariableName, (int) value);
	}
}
