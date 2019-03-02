using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicInput : Singleton<MicInput>
{
	public float MicLoudness { get; private set; }
	public float MicLoudnessinDecibels { get; private set; }

	AudioClip _clipRecord;
	AudioClip _recordedClip;
	int _sampleWindow = 128;

	string _device;
	bool _isInitialized;

	void OnEnable()
	{
		InitMic();
	}

	public void InitMic()
	{
		_clipRecord = Microphone.Start(_device ?? (_device = Microphone.devices[0]), true, 999, 44100);
		_isInitialized = true;
	}

	void Update()
	{
		// levelMax equals to the highest normalized value power 2, a small number because < 1
		// pass the value to a static var so we can access it from anywhere
		MicLoudness = MicrophoneLevelMax();
		MicLoudnessinDecibels = MicrophoneLevelMaxDecibels();
	}

	void OnDisable()
	{
		StopMicrophone();
	}

	void OnDestroy()
	{
		StopMicrophone();
	}

	void OnApplicationFocus(bool focus)
	{
		if (focus)
			if (!_isInitialized)
				InitMic();

		if (!focus)
			StopMicrophone();
	}

	public void StopMicrophone()
	{
		Microphone.End(_device);
		_isInitialized = false;
	}

	float MicrophoneLevelMax()
	{
		float levelMax = 0;
		float[] waveData = new float[_sampleWindow];
		int micPosition = Microphone.GetPosition(null) - (_sampleWindow + 1); 
		if (micPosition < 0) return 0;
		_clipRecord.GetData(waveData, micPosition);

		// Getting a peak on the last 128 samples
		for (int i = 0; i < _sampleWindow; i++)
		{
			float wavePeak = waveData[i] * waveData[i];

			if (levelMax < wavePeak)
				levelMax = wavePeak;
		}

		return levelMax;
	}

	float MicrophoneLevelMaxDecibels()
	{
		float db = 20 * Mathf.Log10(Mathf.Abs(MicLoudness));

		return db;
	}

	public float FloatLinearOfClip(AudioClip clip)
	{
		StopMicrophone();

		_recordedClip = clip;

		float levelMax = 0;
		float[] waveData = new float[_recordedClip.samples];

		_recordedClip.GetData(waveData, 0);

		// Getting a peak on the last 128 samples
		for (int i = 0; i < _recordedClip.samples; i++)
		{
			float wavePeak = waveData[i] * waveData[i];

			if (levelMax < wavePeak)
				levelMax = wavePeak;
		}

		return levelMax;
	}

	public float DecibelsOfClip(AudioClip clip)
	{
		StopMicrophone();

		_recordedClip = clip;

		float levelMax = 0;
		float[] waveData = new float[_recordedClip.samples];

		_recordedClip.GetData(waveData, 0);

		// Getting a peak on the last 128 samples
		for (int i = 0; i < _recordedClip.samples; i++)
		{
			float wavePeak = waveData[i] * waveData[i];

			if (levelMax < wavePeak)
				levelMax = wavePeak;
		}

		float db = 20 * Mathf.Log10(Mathf.Abs(levelMax));

		return db;
	}

}