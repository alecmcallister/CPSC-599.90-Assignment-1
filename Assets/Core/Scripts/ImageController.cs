using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using GoogleARCore;
using UnityEngine.UI;

public class ImageController : Singleton<ImageController>
{
	public List<ImageTracker> ImageTrackers;

	Dictionary<int, ImageTracker> Trackers = new Dictionary<int, ImageTracker>();

	void Awake()
	{
		ImageTracker.ImageClicked += i => { CharacterManager.Instance.TargetFound(i.gameObject, i.Image.DatabaseIndex == 3); };
	}

	public void Update()
	{
		if (Session.Status != SessionStatus.Tracking)
			return;

		List<AugmentedImage> m_TempAugmentedImages = new List<AugmentedImage>();

		Session.GetTrackables<AugmentedImage>(m_TempAugmentedImages, TrackableQueryFilter.Updated);

		foreach (AugmentedImage image in m_TempAugmentedImages)
		{
			ImageTracker tracker = null;
			Trackers.TryGetValue(image.DatabaseIndex, out tracker);

			if (image.TrackingState == TrackingState.Tracking && tracker == null)
			{
				// Create an anchor to ensure that ARCore keeps tracking this augmented image.
				Anchor anchor = image.CreateAnchor(image.CenterPose);
				tracker = (ImageTracker)Instantiate(ImageTrackers[image.DatabaseIndex], anchor.transform);
				tracker.Image = image;
				Trackers.Add(image.DatabaseIndex, tracker);
			}
			else if (image.TrackingState == TrackingState.Stopped && tracker != null)
			{
				Trackers.Remove(image.DatabaseIndex);
				Destroy(tracker.gameObject);
			}
		}
	}
}