using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Globalization;
using UnityEngine.UI;

namespace GellosGames
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class DebugStateView : PageEventListener
    {
        TextMeshProUGUI textView;
        protected override void OnPageChanged(MonoBehaviour sender, Event<UIPageStage> e)
		{
            textView.text = "Page Id: "+ (int)e.StageNow +" Name: "+ Enum.GetName(typeof(UIPageStage), e.StageNow);
        }

		// Start is called before the first frame update
		void Start()
        {
            textView = GetComponent<TextMeshProUGUI>();
        }
        void OnEnable()
        {
            Application.logMessageReceived += LogMessage;
        }

        void OnDisable()
        {
            Application.logMessageReceived -= LogMessage;
        }

        // Button Event von Debug Buttons on Option Menue Page
        public void DebugModeForceFinishingPhase(int days)
        {

        }
        
        public void LogMessage(string message, string stackTrace, LogType type)
        {
            if (enabled && type == LogType.Error || type == LogType.Exception)
            {
                message = "!Er! " + message;
                textView.color = Color.red;
                textView.text = message;
            }
            
        }
    }
}
