using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Demo.Models
{
    public class FriendModel

    {
        
        public int ID { set; get; }

        // Accepted Status
        public bool UserAcceptA { get; set; }
        public bool UserAcceptB { get; set; }

        public bool Accepted()
        {
            if (UserAcceptA && UserAcceptB)
            {
                return true;
            }

            return false;
        }

        // Users That Are Friends
        public string User_NameA { set; get; }
        public string User_NameB { set; get; }

        // Profile ID's
        public int UserProfileIDA  { get; set; }
        public int UserProfileIDB  { get; set; }

    }
}

