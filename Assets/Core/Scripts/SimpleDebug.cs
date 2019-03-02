using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleDebug : Singleton<SimpleDebug>
{
	Text m_text;
	public Text Text => m_text ?? (m_text = GetComponent<Text>());

	public void SetText(string text)
	{
		Text.text = text;
	}
}
