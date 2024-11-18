using System;
using System.Collections.Generic;

namespace User
{
    [Serializable]
    public class CaneHistoryData
    {
        public string timeStamp;
        public int fallCount;
        public int sumfallCount;
        public int footStep;
        public int useTime;
        public string distance;
    }

    [Serializable]
    public class CaneInformationData
    {
        public string caneID;
        public bool hasConnected;
    }
    
    [Serializable]
    public class UserData
    {
        public string name;
        public string birthday;
        public string sex;
        public int height;
        public int width;
        public string phoneNumber;
        public string contactName;
        public string contactPhoneNumber;
        public string userCaneId;
    }

    [Serializable]
    public class AppUserData
    {
        public string email;
        public string password;
        public string caneId;
    }
}