using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;

using Newtonsoft.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

using Instagraph.Data;
using Instagraph.DataProcessor.Dtos;
using Instagraph.Models;

namespace Instagraph.DataProcessor
{

    public class Deserializer
    {
        private static string errorMsg = "Error: Invalid data.";
        private static string succsessMSg = "Successfully imported {0}.";

        public static string ImportPictures(InstagraphContext context, string jsonString)
        {
            var pictures = JsonConvert.DeserializeObject<Picture[]>(jsonString).ToArray();

            var sb = new StringBuilder();

            var validatedPics = new List<Picture>();

            foreach (var p in pictures)
            {
                bool isValid = !String.IsNullOrWhiteSpace(p.Path) && p.Size > 0;
                bool pictureExists = context.Pictures
                    .Any(pic => pic.Path == p.Path) || validatedPics
                    .Any(pic => pic.Path == p.Path);

                if (!isValid || pictureExists)
                {
                    sb.AppendLine(errorMsg);
                    continue;
                }

                validatedPics.Add(p);
                sb.AppendLine(String.Format(succsessMSg, $"Picture {p.Path}"));
            }

            context.Pictures.AddRange(validatedPics);
            context.SaveChanges();
            return sb.ToString().Trim();
        }

        public static string ImportUsers(InstagraphContext context, string jsonString)
        {
            var deserializedUsers = JsonConvert.DeserializeObject<UserDto[]>(jsonString);

            var sb = new StringBuilder();

            var users = new List<User>();

            foreach (var userDto in deserializedUsers)
            {
                bool IsValid = !String.IsNullOrWhiteSpace(userDto.Username) &&
                               userDto.Username.Length <= 30 &&
                               !String.IsNullOrWhiteSpace(userDto.Password) &&
                               userDto.Password.Length <= 20 &&
                               !String.IsNullOrWhiteSpace(userDto.ProfilePicture);

                var picture = context.Pictures.FirstOrDefault(p => p.Path == userDto.ProfilePicture);

                bool userExists = users.Any(u => u.Username == userDto.Username);

                if (!IsValid || picture == null || userExists)
                {
                    sb.AppendLine(errorMsg);
                    continue;
                }

                var user = Mapper.Map<User>(userDto);
                user.ProfilePicture = picture;

                users.Add(user);
                sb.AppendLine(String.Format(succsessMSg, $"User {user.Username}"));
            }
            context.Users.AddRange(users);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportFollowers(InstagraphContext context, string jsonString)
        {
            var deserializedFollowers = JsonConvert.DeserializeObject<FollowerDto[]>(jsonString);

            var sb = new StringBuilder();

            var followers = new List<UserFollowers>();

            foreach (var dto in deserializedFollowers)
            {
                int? userId = context.Users.FirstOrDefault(u => u.Username == dto.User)?.Id;
                int? followerId = context.Users.FirstOrDefault(u => u.Username == dto.Follower)?.Id;

                if (userId == null || followerId == null)
                {
                    sb.AppendLine(errorMsg);
                    continue;
                }

                bool allreadyFollowing = followers.Any(f => f.UserId == userId
                                                    && f.FollowerId == followerId);

                if (allreadyFollowing)
                {
                    sb.AppendLine(errorMsg);
                    continue;
                }
                var userFollower = new UserFollowers()
                {
                    UserId = userId.Value,
                    FollowerId = followerId.Value
                };

                followers.Add(userFollower);
                sb.AppendLine(String.Format(succsessMSg, $"Follower {dto.Follower} to User {dto.User}"));
            }

            context.UsersFollowers.AddRange(followers);
            context.SaveChanges();
            return sb.ToString().Trim();
        }

        public static string ImportPosts(InstagraphContext context, string xmlString)
        {
            var xDoc = XDocument.Parse(xmlString);

            var sb = new StringBuilder();

            var posts = new List<Post>();

            foreach (var element in xDoc.Root.Elements())
            {
                var caption = element.Element("caption")?.Value;
                var username = element.Element("user")?.Value;
                var picturePath = element.Element("picture")?.Value;

                bool IsValid = !String.IsNullOrWhiteSpace(caption) &&
                               !String.IsNullOrWhiteSpace(username) &&
                               !String.IsNullOrWhiteSpace(picturePath);

                if (!IsValid)
                {
                    sb.AppendLine(errorMsg);
                    continue;
                }

                int? userId = context.Users.FirstOrDefault(u => u.Username == username)?.Id;
                int? pictureId = context.Pictures.FirstOrDefault(p => p.Path == picturePath)?.Id;

                if (userId == null || pictureId == null)
                {
                    sb.AppendLine(errorMsg);
                    continue;
                }

                var post = new Post()
                {
                    Caption = caption,
                    UserId = userId.Value,
                    PictureId = pictureId.Value
                };

                posts.Add(post);
                sb.AppendLine(String.Format(succsessMSg, $"Post {caption}"));
            }

            context.AddRange(posts);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportComments(InstagraphContext context, string xmlString)
        {
            var xDox = XDocument.Parse(xmlString);

            var elements = xDox.Root.Elements();

            var sb = new StringBuilder();

            var comments = new List<Comment>();

            foreach (var element in elements)
            {
                var content = element.Element("content")?.Value;
                var user = element.Element("user")?.Value;
                var postString = element.Element("post")?.Attribute("id")?.Value;

                bool nullCheck = !String.IsNullOrWhiteSpace(content) &&
                                 !String.IsNullOrWhiteSpace(user) &&
                                 !String.IsNullOrWhiteSpace(postString);

                if (!nullCheck)
                {
                    sb.AppendLine(errorMsg);
                    continue;
                }

                var postId = int.Parse(postString);

                var userId = context.Users.FirstOrDefault(u => u.Username == user)?.Id;
                var postExists = context.Posts.Any(p => p.Id == postId);

                if (userId == null || !postExists)
                {
                    sb.AppendLine(errorMsg);
                    continue;
                }

                var comment = new Comment()
                {
                    Content = content,
                    UserId = userId.Value,
                    PostId = postId
                };

                comments.Add(comment);
                sb.AppendLine(String.Format(succsessMSg, $"Comment {content}"));
            }

            context.Comments.AddRange(comments);
            context.SaveChanges();

            return sb.ToString().Trim();
        }
    }
}