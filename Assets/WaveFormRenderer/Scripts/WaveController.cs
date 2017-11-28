using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaveFormRenderer
{
	public class WaveController : MonoBehaviour
	{
		public WaveFormRenderer Wave0;
		public WaveFormRenderer Wave1;

		public Gradient WaveGradientColor;

		public float Amplitude = 1f;
		public float Frequency = 2f;
		public float Speed = 1f;
		public float Noise = 0.01f;

		public AnimationCurve curve;

		private float lifetime;
		private float lasTime;
		private bool is0rendering = true;

		// Use this for initialization
		void Start()
		{
			Wave0.SetColor(WaveGradientColor);
			Wave1.SetColor(WaveGradientColor);
			Wave0.curve = curve;
			Wave1.curve = curve;
			Wave0.ResetWave(is0rendering);
			Wave1.ResetWave(!is0rendering);
			lasTime = Time.time;
			SetSpeed(Speed);
			SetAmplitude(Amplitude);
			SetFrequency(Frequency);
			SetNoise(Noise);
		}

		// Update is called once per frame
		void Update()
		{
			if (Time.time - lasTime > lifetime)
			{
				lasTime = Time.time;
				is0rendering = !is0rendering;
				Wave0.ResetWave(is0rendering);
				Wave1.ResetWave(!is0rendering);
			}
		}

		public void SetFrequency(float value)
		{
			Frequency = value;
			Wave0.SetFrequency(value);
			Wave1.SetFrequency(value);
		}
		public void SetAmplitude(float value)
		{
			Amplitude = value;
			Wave0.SetAmplitude(value);
			Wave1.SetAmplitude(value);
		}
		public void SetSpeed(float value)
		{
			Speed = value;
			Wave0.SetSpeed(value);
			Wave1.SetSpeed(value);
			lifetime = 1f / Speed;
		}
		public void SetNoise(float value)
		{
			Noise = value;
			Wave0.SetNoise(value);
			Wave1.SetNoise(value);
		}
	}
}
