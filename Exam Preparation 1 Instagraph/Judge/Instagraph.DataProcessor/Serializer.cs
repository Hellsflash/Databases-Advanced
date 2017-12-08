using System;
using System.Linq;
using Instagraph.Data;
using Newtonsoft.Json;

namespace Instagraph.DataProcessor
{
    public class Serializer
    {
        public static string ExportUncommentedPosts(InstagraphContext context)
        {
            var posts = context.Posts
                .Where(p => p.Comments.Count == 0)
                .OrderBy(p => p.Id)
                .Select(p => new
                {
                    Id = p.Id,
                    Picture = p.Picture.Path,
                    User = p.User.Username
                })
                .ToArray();

            var jsonString = JsonConvert.SerializeObject(posts, Formatting.Indented);
            return jsonString;
        }

        public static string ExportPopularUsers(InstagraphContext context)
        {
            throw new NotImplementedException();
        }

        public static string ExportCommentsOnPosts(InstagraphContext context)
        {
            throw new NotImplementedException();
        }
    }
}