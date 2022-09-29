using GrandDevs.ExtremeScooling.Common;
using System;

namespace GrandDevs.ExtremeScooling
{
    public class CachedUserData
    {
        public string username,
                      password,
                      name,
                      firstname,
                      secondname,
                      email,
                      city,
                      phone;

        public int isConfirmed,
                   userId;

        public string avatarColor;

        public bool isLoggined;

        public Enumerators.Language appLanguage { get; set; }
    }

    public class User
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Username { get; set; }
        public string Status { get; set; }
        public string UserId { get; set; }
        public int GroupsAmount { get; set; }
        public int IsConfirmed { get; set; }
        public DateTime DateJoined { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public string Nationality { get; set; }
        public string CountryOfResidence { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public int AvatarId { get; set; }
        public float RedColor { get; set; }
        public float BlueColor { get; set; }
        public float GreenColor { get; set; }
        public string Sex { get; set; }

        public bool IsWasFirstAppLogin;
    }
}