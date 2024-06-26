﻿using Runtime.Extensions;
using UnityEngine;

namespace Runtime.Infrastructure.UserData
{
    public sealed class UserDataSaveLoadService : IUserDataSaveLoadService
    {
        private const string UserDataKey = "UserData";
        
        private UserData _userData;

        public UserDataSaveLoadService(UserData userData)
        {
            _userData = userData;

            _userData.UserDataWasUpdated += Save;
        }
        
        public void Save()
        {
            PlayerPrefs.SetString(UserDataKey, _userData.ToJson());
            PlayerPrefs.Save();
        }

        public void Load()
        {
            _userData.UpdateParams(PlayerPrefs.GetString(UserDataKey).ToDeserialized<UserData>() ?? new UserData());
        }
    }
}