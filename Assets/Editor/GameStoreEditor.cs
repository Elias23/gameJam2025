using Core;
using UnityEditor;

namespace Editor
{
    public static class GameStoreEditor
    {
        [MenuItem("Tools/Reset Tutorial State")]
        private static void ResetTutorialState()
        {
            GameStore.ResetTutorialState();
            UnityEngine.Debug.Log("Tutorial state reset");
        }
    }
}