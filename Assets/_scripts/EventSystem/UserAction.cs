using System;
using System.Collections.Generic;
using GellosGames;
using UnityEngine;

namespace GellosGames
{
    public enum UserActions
    {
        None = 0,
        UserSignedIn = 11,
        UserLoggedOut = 12,
        SelfAssessmentDone = 21,
        ExtremeSchoolingSetDateSucceeded = 31,
        SetUpDailyTracker = 40,
        DalyTrackerIsDone = 41,
    }
    public class UserAction : UIStageLocator<UserActions>, IService
    {
        public override void RunEvent(MonoBehaviour sender, Event<UserActions> e)
        {
            RunSpecificEvent(sender, e);
        }
    }
}