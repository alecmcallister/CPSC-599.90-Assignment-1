using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class TargetButton : MonoBehaviour, IPointerClickHandler
{
	public static event Action<GameObject, bool> TargetClicked;
	public bool finish;

	public void OnPointerClick(PointerEventData eventData)
	{
		TargetClicked?.Invoke(transform.root.gameObject, finish);
	}
}
