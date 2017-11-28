using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;


public class WaveRenderer : MonoBehaviour
{
	public float Amplitude = 1f;
	public float Frequency = 2f;
	public float Speed = 1f;
	public AnimationCurve curve;

	float height;
	float width;
	float lifetime = 1f;
	float minimumVertexDistance = 0.01f;

	Vector3 velocity;
	Vector3 markerPosition;

	UILineRenderer line;
	List<Vector2> points;
	List<float> pointsLifetime;
	Queue<float> spawnTimes = new Queue<float>();
	RectTransform rectTransform;

	void Awake()
	{
		markerPosition = transform.position;
		line = GetComponent<UILineRenderer>();
		line.RelativeSize = false;
		points = new List<Vector2>() { markerPosition };
		pointsLifetime = new List<float>() { Time.time };
		line.Points = points.ToArray();
		rectTransform = GetComponent<RectTransform>();
		width = rectTransform.rect.size.x;
		height = rectTransform.rect.size.y / 2f;
		minimumVertexDistance = (width / 2000f);
		minimumVertexDistance = minimumVertexDistance > 0.01f ? 0.01f : minimumVertexDistance;
		OnSpeedChange(Speed);
	}

	void AddPoint(Vector3 position)
	{
		points.Insert(1, position);
		pointsLifetime.Insert(1, Time.time);
		spawnTimes.Enqueue(Time.time);
	}

	void RemovePoint()
	{
		spawnTimes.Dequeue();
		points.RemoveAt(points.Count - 1);
		pointsLifetime.RemoveAt(pointsLifetime.Count - 1);
	}

	void Update()
	{
		markerPosition = Vector2.up * height * curve.Evaluate(Time.time * Frequency) * Amplitude;

		while (spawnTimes.Count > 0 && spawnTimes.Peek() + lifetime < Time.time)
		{
			RemovePoint();
		}

		Vector2 diff = -velocity * Time.deltaTime;
		for (int i = 1; i < points.Count; i++)
		{
			points[i] += diff;
		}

		if (points.Count < 2 || Vector2.Distance(markerPosition, points[1]) > minimumVertexDistance)
		{
			AddPoint(markerPosition);
		}

		points[0] = markerPosition;

		//line.positionCount = points.Count;
		line.Points = points.ToArray();
		line.Lifetime = pointsLifetime.ToArray();
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
		lifetime = 1f / Speed;
		line.LineLifetime = lifetime;
		velocity.x = width * Speed;
	}
}

