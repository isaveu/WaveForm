using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveFormRenderer : MonoBehaviour
{
	public int Resolution;
	public float Size;
	public float Speed;
	public float Frequency;
	public float Amplitude;
	public Color WaveColor;

	public AnimationCurve Curve;

	float Spacing;

	LineRenderer Wave;
	Vector3[] Points;
	// Use this for initialization
	void Start()
	{
		Spacing = Size / (float)Resolution;
		Points = new Vector3[Resolution];
		for (int i = 0; i < Resolution; i++)
			Points[i] = new Vector3(i * Spacing, 0f, 0f);
		Wave = GetComponent<LineRenderer>();
		Wave.positionCount = Resolution;
		Wave.SetPositions(Points);
		Wave.startWidth = Wave.endWidth = 0.1f;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		Spacing = Size / (float)Resolution;
		for (int i = 0; i < Resolution; i++)
		{
			Points[i].x = i * Spacing;
			//Points[i].y = Mathf.Sin(Points[i].x * Frequency - Time.time * Speed) * Amplitude;
			Points[i].y = Curve.Evaluate(Points[i].x * Frequency - Time.time * Speed) * Amplitude;
		}
		Wave.SetPositions(Points);
	}

	public void OnFrequencyChange(float value)
	{
		Frequency = value;
	}
	public void OnAmplitudeChange(float value)
	{
		Amplitude = value;
	}
	public void OnSpeedChange(float value)
	{
		Speed = value;
	}
	public void OnSizeChange(float value)
	{
		Size = value;
	}
	public void OnResolutionChange(float value)
	{
		Resolution = 2 + (10*(int)value);
		Debug.Log(Resolution.ToString());
		Wave.positionCount = Resolution;
		Points = new Vector3[Resolution];
		for (int i = 0; i < Resolution; i++)
			Points[i] = new Vector3();
	}
}
