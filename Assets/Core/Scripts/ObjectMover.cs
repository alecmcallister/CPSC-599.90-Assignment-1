using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
	public Animator anim;
	List<int> tweens = new List<int>();
	public ParticleSystem woosh;

	void Awake()
	{
		if (!anim)
			enabled = false;

		anim.SetBool("Grounded", true);
	}

	public void MoveTo(Vector3 pos, bool finish = false)
	{
		tweens.ForEach(i => LeanTween.cancel(i));

		Vector3 delta = pos - transform.position;

		transform.forward = new Vector3(delta.x, 0f, delta.z).normalized;

		float time = delta.magnitude * 5f;

		tweens.Add(LeanTween.value(anim.GetFloat("MoveSpeed"), 1f, time / 4f).setOnUpdate((float val) =>
		{
			anim.SetFloat("MoveSpeed", val);
		}).setEase(LeanTweenType.easeInOutSine).uniqueId);

		tweens.Add(LeanTween.value(1f, 0f, time / 4f).setOnUpdate((float val) =>
		{
			anim.SetFloat("MoveSpeed", val);
		}).setEase(LeanTweenType.easeInOutSine).setDelay(3f * (time / 4f)).uniqueId);

		tweens.Add(LeanTween.move(gameObject, pos, time).setEase(LeanTweenType.easeInOutSine).setOnComplete(() => { if (finish) DoTheThing(); }).uniqueId);
	}

	public void DoTheThing()
	{
		anim.SetTrigger("Wave");
		woosh.Play();
	}
}
