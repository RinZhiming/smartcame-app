using System;
using System.Collections.Generic;
using User;

public static class UserDataApp
{
    public static string Email {  get; set; }
    public static string CaneId { get; set; }
    public static Dictionary<DateTime, CaneHistoryData> History { get; set; } = new();
}