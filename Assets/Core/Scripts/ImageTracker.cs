using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;
using GoogleARCore;
using GoogleARCoreInternal;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Renderer), typeof(Collider))]
public class ImageTracker : MonoBehaviour
{
	public static event Action<ImageTracker> ImageClicked;

	public AugmentedImage Image;

	Renderer m_renderer;
	public Renderer Renderer => m_renderer ?? (m_renderer = GetComponent<Renderer>());

	Collider m_collider;
	public Collider Collider => m_collider ?? (m_collider = GetComponent<Collider>());

	void Update()
	{
		if (Image == null || Image.TrackingState != TrackingState.Tracking)
			return;

		transform.localScale = Vector3.one * 0.1f * Image.ExtentX;
	}

	void OnMouseDown()
	{
		if (EventSystem.current.IsPointerOverGameObject())
			return;

		ImageClicked?.Invoke(this);
	}
}
