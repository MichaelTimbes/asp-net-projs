using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Demo.Models
{
    public class UserProfile
    {
        public int ID { get; set; }
        public int UserModelID { get; set; }
        public string UserProfileSummary { get; set; }
        //public ICollection<UserModel> UserFriends { get; set; }
        public List<int> UserFriends;
        public string UserProfileStatusUpdate { get; set; }

        public void AddFriend(int FriendID)
        {
            UserFriends.Add(FriendID);
        }

    }

}
