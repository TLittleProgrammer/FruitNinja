using UnityEditor;
using UnityEngine;

namespace Editor.UserData
{
    public class ClearUserData
    {
        [MenuItem("OpenMyGame/UserData/Delete UserData")]
        private void DeleteUserData()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}