using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveRenderer : MonoBehaviour
{
	public float Amplitude = 1f;
	public float Frequency = 2f;
	public float lifetime = 5f;

	public float minimumVertexDistance = 0.01f;

	public Vector3 velocity;

	public AnimationCurve curve;

	Vector3 markerPosition;

	LineRenderer line;
	List<Vector3> points;
	Queue<float> spawnTimes = new Queue<float>();

	void Awake()
	{
		markerPosition = transform.position;
		line = GetComponent<LineRenderer>();
		line.useWorldSpace = false;
		points = new List<Vector3>() { markerPosition };
		line.positionCount = points.Count;
		line.SetPositions(points.ToArray());
	}

	void AddPoint(Vector3 position)
	{
		points.Insert(1, position);
		spawnTimes.Enqueue(Time.time);
	}

	void RemovePoint()
	{
		spawnTimes.Dequeue();
		points.RemoveAt(points.Count - 1);
	}

	void Update()
	{
		markerPosition = Vector3.up * curve.Evaluate(Time.time * Frequency) * Amplitude;

		while (spawnTimes.Count > 0 && spawnTimes.Peek() + lifetime < Time.time)
		{
			RemovePoint();
		}

		Vector3 diff = -velocity * Time.deltaTime;
		for (int i = 1; i < points.Count; i++)
		{
			points[i] += diff;
		}

		if (points.Count < 2 || Vector3.Distance(markerPosition, points[1]) > minimumVertexDistance)
		{
			AddPoint(markerPosition);
		}

		points[0] = markerPosition;

		line.positionCount = points.Count;
		line.SetPositions(points.ToArray());
	}


	public void OnFrequencyChange(float value)
	{
		Frequency = value;
	}
	public void OnAmplitudeChange(float value)
	{
		Amplitude = value;
	}
}

