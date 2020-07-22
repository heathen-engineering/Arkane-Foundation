﻿using System;

namespace HeathenEngineering.Arkane.DataModel
{
    [Serializable]
    public class UserProfile
    {
        public string userId;
        public bool hasMasterPin;
        public string username;
        public string email;
        public string firstName;
        public string lastName;
    }
}
