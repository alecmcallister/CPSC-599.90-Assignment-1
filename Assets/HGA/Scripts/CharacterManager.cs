using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : Singleton<CharacterManager>
{
	public ObjectMover mover;

	void Awake()
	{
		TargetButton.TargetClicked += TargetFound;
	}

	public void TargetFound(GameObject target, bool finish)
	{
		mover.MoveTo(target.transform.position, finish);
	}
}
