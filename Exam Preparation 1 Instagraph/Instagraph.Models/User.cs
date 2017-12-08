using System.Collections.Generic;
using System.Xml.Linq;

namespace Instagraph.Models
{
    public class User
    {
        public User()
        {
            this.Followers = new List<UserFollowers>();
            this.UsersFollowing = new List<UserFollowers>();
            this.Posts = new List<Post>();
            this.Comments = new List<Comment>();
        }

        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public int ProfilePictureId { get; set; }
        public Picture ProfilePicture { get; set; }

        public ICollection<UserFollowers> Followers { get; set; }

        public ICollection<UserFollowers> UsersFollowing { get; set; }

        public ICollection<Post> Posts { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}