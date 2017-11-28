using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;


namespace WaveFormRenderer
{
	public class WaveFormRenderer : MonoBehaviour
	{
		[HideInInspector]
		public AnimationCurve curve;

		private float Amplitude = 1f;
		private float Frequency = 2f;
		private float Speed = 0.5f;
		private float height;
		private float width;
		private float lifetime = 1f;
		private float minimumVertexDistance = 0.01f;
		private float positionx;
		private float lastPositionx;
		private float timex;
		private float lastTimex;
		private float maxTime;
		private float t, a, b;
		private float noise = 0.01f;

		private Vector2 velocity;
		private Vector2 markerPosition;
		private Vector2 tempPosition;

		private UIWaveRenderer line;
		private List<Vector2> points;
		private List<float> pointsLifetime;
		private Queue<float> spawnTimes = new Queue<float>();
		private RectTransform rectTransform;

		void Awake()
		{
			markerPosition = transform.position;
			line = GetComponent<UIWaveRenderer>();
			line.RelativeSize = false;
			points = new List<Vector2>() { markerPosition };
			pointsLifetime = new List<float>() { Time.time };
			line.Points = points.ToArray();
			rectTransform = GetComponent<RectTransform>();
			width = rectTransform.rect.size.x;
			height = rectTransform.rect.size.y / 2f;
			minimumVertexDistance = (width / 2000f);
			minimumVertexDistance = minimumVertexDistance > 0.01f ? 0.01f : minimumVertexDistance;
			line.CrossFadeColor(Color.green, lifetime, false, true);
			SetSpeed(Speed);
			timex = 0f;
			lastTimex = 0f;
			maxTime = curve.keys[curve.keys.Length - 1].time;
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
			positionx += Time.deltaTime * Speed * width;
			timex += Time.deltaTime * Frequency;
			if (timex > maxTime)
			{
				timex -= maxTime;
			}
			if (positionx > width)
			{
				positionx = width;
				timex = maxTime;
			}
			else
			{
				markerPosition = Vector2.up * height * curve.Evaluate(timex) * Amplitude;
				markerPosition.y += 0.1f * height * Random.Range(1f - noise, 1f + noise);
				markerPosition.x += positionx;
			}

			while (spawnTimes.Count > 0 && spawnTimes.Peek() + lifetime < Time.time)
			{
				RemovePoint();
			}
			if (positionx < width)
			{
				for (int i = 0; i < curve.keys.Length; i++)
				{
					if (curve.keys[i].time > lastTimex && curve.keys[i].time < timex)
					{
						t = timex - lastTimex;
						a = curve.keys[i].time - lastTimex;
						tempPosition = Vector2.up * height * curve.Evaluate(timex) * Amplitude;
						tempPosition.y += 0.1f * height * Random.Range(1f - noise, 1f + noise);
						tempPosition.x = lastPositionx + ((markerPosition.x - lastPositionx) * (a / t));
						AddPoint(tempPosition);
					}
				}
			}
			if (points.Count < 2 || Vector2.Distance(markerPosition, points[1]) > minimumVertexDistance)
			{
				AddPoint(markerPosition);
			}


			points[0] = markerPosition;

			line.Points = points.ToArray();
			line.Lifetime = pointsLifetime.ToArray();
			lastTimex = timex;
			lastPositionx = markerPosition.x;
		}


		public void SetFrequency(float value)
		{
			Frequency = value;
		}
		public void SetAmplitude(float value)
		{
			Amplitude = value;
		}
		public void SetSpeed(float value)
		{
			Speed = value;
			lifetime = 1f / Speed;
			line.LineLifetime = lifetime;
			velocity.x = width * Speed;
		}

		public void ResetWave(bool value)
		{
			if (!value)
				return;
			positionx = 0f;
			markerPosition.x = 0;
			spawnTimes.Clear();
			points.Clear();
			pointsLifetime.Clear();
			points.Add(markerPosition);
			pointsLifetime.Add(Time.deltaTime);
			line.Points = points.ToArray();
		}
		public void SetColor(UnityEngine.Gradient gradient)
		{
			line.lineColor = gradient;
		}
		public void SetNoise(float value)
		{
			noise = value;
		}
	}
}