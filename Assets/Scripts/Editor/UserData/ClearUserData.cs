using UnityEditor;
using UnityEngine;

namespace Editor.UserData
{
    public class ClearUserData
    {
        [MenuItem("OpenMyGame/UserData/Delete UserData")]
        private static void DeleteUserData()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}