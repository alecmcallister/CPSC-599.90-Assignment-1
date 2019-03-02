using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Balloon : MonoBehaviour
{
	public event Action DidTheThing = new Action(() => { });

	public Gradient PopGradient;
	public GameObject Confetti;

	float minScale = 0.05f;
	float maxScale = 0.2f;
	float eventThreshold = 0.17f;

	float eventSeconds = 1f;

	float scaleTarget = 0.05f;

	float lerpSpeed = 2f;
	float deflationSpeed = 0.05f;

	bool checkingEventThreshold = false;

	float micThreshold = -55f;

	Vector3 basePos => Vector3.up * 0.5f * transform.localScale.y;

	Vector3 posTarget = Vector3.zero;

	Renderer m_renderer;
	public Renderer Renderer => m_renderer ?? (m_renderer = GetComponent<Renderer>());

	void Awake()
	{
		DidTheThing += () => { Instantiate(Confetti, transform.position, Quaternion.identity); Destroy(gameObject, 0.001f); };
		posTarget = basePos;
	}

	void Update()
	{
		if (Input.GetKey(KeyCode.Space))
			AddPos(Vector3.up * 0.1f);

		AddAir(GetMicInput() / 2f);
		AddPos(GetAccelInput());

		UpdateScale();
		UpdatePos();
		UpdateColor();
	}

	float GetMicInput()
	{
		float volume = Mathf.Min(MicInput.Instance.MicLoudnessinDecibels, -0.01f);

		if (volume < micThreshold)
			return 0f;

		else
			return Mathf.Min(1f / Mathf.Abs(volume), 1f);
	}

	Vector3 GetAccelInput()
	{
		Vector3 d = new Vector3(Input.acceleration.x, -Input.acceleration.y, Input.acceleration.z) * 0.05f;
		return d;
	}

	void UpdateScale()
	{
		scaleTarget = Mathf.Clamp(scaleTarget - (Time.smoothDeltaTime * deflationSpeed), minScale, maxScale);

		if (scaleTarget > eventThreshold && !checkingEventThreshold)
			StartCoroutine(EventThreshold());

		transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * scaleTarget, Time.smoothDeltaTime * lerpSpeed);
	}

	void UpdatePos()
	{
		posTarget = Vector3.Lerp(posTarget, posTarget + (basePos - posTarget), Time.smoothDeltaTime);

		transform.localPosition = posTarget;
	}

	void UpdateColor()
	{
		float time = (scaleTarget - minScale) / (maxScale - minScale);
		Renderer.material.color = PopGradient.Evaluate(time);
	}

	IEnumerator EventThreshold()
	{
		checkingEventThreshold = true;

		float timeGoal = Time.time + eventSeconds;

		while (scaleTarget > eventThreshold)
		{
			if (Time.time > timeGoal)
			{
				DidTheThing();
				break;
			}
			yield return null;
		}

		checkingEventThreshold = false;
	}

	public void AddAir(float air)
	{
		scaleTarget = Mathf.Clamp(scaleTarget + (Time.smoothDeltaTime * air), minScale, maxScale);
	}

	public void AddPos(Vector3 delta)
	{
		posTarget += delta * Time.smoothDeltaTime * lerpSpeed;
	}

}
