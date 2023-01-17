using System;
using GellosGames;
using UnityEngine;
using UnityEngine.Events;

namespace GellosGames
{
    public enum UIPageStage
    {
        Start_Menue = 1,
        //Start_btn_perCouch = 2,
        //Start_btn_AppStore = 3,
        //Start_btn_ForWhat = 4,

        Start_Page_SignIn = 2,
        Start_UserWelcome = 3,
        Start_ForWhatIsTheApp = 4,
        Start_ForWhatIsTheApp_more = 5,
        Start_UserOptions = 8,
        Start_Menue_Group = 9,

        Main_Menue = 10,
        Main_Menue_LernStages = 11,
        Main_Menue_SchoolSub = 15,
        Main_Menue_extremeSchooling = 16,
        Main_Menue_LearnGoals = 17,
        Main_Menue_Group = 100,

        Phase1_form_EnterSchoolSubjects = 20,
        Phase1_form_Homework = 21,
        Phase1_form_LernTime = 22,
        Phase1_form_Concentration = 23,
        Phase1_form_WorkActivity = 24,
        Phase1_form_Interruptions = 25,
        Phase1_selfAssessment_Done = 26,
        Phase1_form_Group = 200,   // As Group

        ExtremeSchooling_SetDate = 31,
        ExtremeSchooling_DailyOverview = 32,
        ExtremeSchooling_WeeklyGoals = 33,
        ExtremeSchooling_Calender = 34,
        ExtremeSchooling_Notes = 35,
        ExtremeSchooling_Group = 300, // As Group

        Phase2_Homework = 41,
        Phase2_DailyLernTime = 42,
        Phase2_Concentration = 43,
        Phase2_YourCollaboration = 44,
        Phase2_Interruptions = 45,
        Phase2_selfAssessment_Done = 46,
        Phase2_DailyTrackingIsDone = 47,
        Phase2_Group = 400,

        DailyOverview_Homework = 51,
        DailyOverview_LernTime = 52,
        DailyOverview_YourCollaboration = 53,
        DailyOverview_Concentration_Home = 54,
        DailyOverview_Concentration_School = 56,
        DailyOverview_Interruptions = 55,       
        DalyOverview_Group = 500,

        // Phase3_DailyTracker = 61,
        // Phase3_WeeklyGoals = 62,
        // Phase3_Group = 600,

        Phase3_Homework = 71,
        Phase3_DailyLernTime = 72,
        Phase3_Concentration = 73,
        Phase3_YourCollaboration = 74,
        Phase3_Interruptions = 75,
        Phase3_DailyTrackingIsDone = 76,
        Phase3_SetDate = 77,
        Phase3_selfAssessment_Done = 78,

        Tutorial_Phase1_Done_MainMenue = 91,
        Tutorial_Phase2_Done_MainMenue = 92,

        None = 0,
        None_Group = 10000,
        LastPage = 10001,
    }
    public class PageEvents : UIStageLocator<UIPageStage>, IService
    {
        public UIPageStage CurrentPageStage { private set; get; } = UIPageStage.Start_Menue;
        public UIPageStage LastPageStage { private set; get; }
        /// <summary>
        /// For specific Single Event Listener which do not need a call on each event ( Filtered ) 
        /// </summary>
        public event StageDelegate<UIPageStage> broadcastEventChangedListener;

        /// <summary>
        /// UIStage enum as String
        /// </summary>
        public string CurrentStageName => Enum.GetName(typeof(UIPageStage), CurrentPageStage);

        /// <summary>
        /// Set new or next Stage
        /// </summary>
        /// <param name="NextStage"></param>
        /// <returns>true if State Change accepted</returns>
        public override void RunEvent(MonoBehaviour sender, Event<UIPageStage> e)
        {
            LastPageStage = CurrentPageStage;
            // Invoke ChangeState Command an all Listener
            broadcastEventChangedListener(sender, e);
            // invoke on Spesific Listener
            base.RunSpecificEvent(sender, e);

            CurrentPageStage = e.StageNow;
            Debug.Log("UIStage: " + CurrentStageName);

        }
    }

    public class CallBackStageEvent : Event<UIPageStage>
    {
        public UnityAction AafterEventCallBack;
        public CallBackStageEvent(UIPageStage targetStage, UnityAction afterEventCallBack) : base(targetStage)
        {
            this.AafterEventCallBack = afterEventCallBack;
        }
    }
}