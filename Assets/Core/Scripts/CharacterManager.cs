using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class CharacterManager : Singleton<CharacterManager>
{
	public ObjectMover mover;
	public GameObject balloonPrefab;
	public Transform balloonHolder;

	Balloon currentBalloon;

	void Awake()
	{
		Input.gyro.enabled = true;
		Permission.RequestUserPermission(Permission.Microphone);
		TargetButton.TargetClicked += TargetFound;
		MakeBalloon();
	}

	void Update()
	{
		if (currentBalloon != null)
		{
			//Input.gyro.userAcceleration;
		}
	}

	public void MakeBalloon()
	{
		currentBalloon = Instantiate(balloonPrefab, balloonHolder, false).GetComponent<Balloon>();
		currentBalloon.DidTheThing += MakeBalloon;
	}

	public void TargetFound(GameObject target, bool finish)
	{
		mover.MoveTo(target.transform.position, finish);
	}
}
