using System;
using System.Collections;
using UnityEngine;

namespace Gianni.Helper
{
	public class CountDownTimer : MonoBehaviour
	{
		public event Action OnCountDown;
		public bool IsRunning;
		float time;
		float remainingTime;
		public void ResetCountDown()
		{
			remainingTime = time;
		}
		public void StartCountDown(float time, Action OnDown)
		{
			this.time = time;
			remainingTime = time;
			OnCountDown = OnDown;
			StartCoroutine(Timer());
		}

		private IEnumerator Timer()
		{
			IsRunning = true;
			while (remainingTime >= 0)
			{
				remainingTime -= Time.deltaTime;
				yield return null;
			}
			OnCountDown();
			IsRunning = false;
		}
	}
}
