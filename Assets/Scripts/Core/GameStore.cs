using UnityEngine;

namespace Core
{
    public static class GameStore
    {
        private const string TUTORIAL_COMPLETED_KEY = "TutorialCompleted";

        public static bool IsTutorialCompleted
        {
            get => PlayerPrefs.GetInt(TUTORIAL_COMPLETED_KEY, 0) == 1;
            set
            {
                PlayerPrefs.SetInt(TUTORIAL_COMPLETED_KEY, value ? 1 : 0);
                PlayerPrefs.Save();
            }
        }

        public static void ResetTutorialState()
        {
            PlayerPrefs.DeleteKey(TUTORIAL_COMPLETED_KEY);
            PlayerPrefs.Save();
        }
    }
}