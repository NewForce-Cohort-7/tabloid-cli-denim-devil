using System;
using System.Collections.Generic;
using TabloidCLI.Models;
using TabloidCLI.Repositories;

namespace TabloidCLI.UserInterfaceManagers
{
    internal class BlogDetailManager : IUserInterfaceManager
    {
        private IUserInterfaceManager _parentUI;
        private BlogRepository _blogRepository;
        private PostRepository _postRepository;
        private TagRepository _tagRepository;
        private int _blogId;

        public BlogDetailManager(IUserInterfaceManager parentUI, string connectionString, int blogId)
        {
            _parentUI = parentUI;
            _blogRepository = new BlogRepository(connectionString);
            _postRepository = new PostRepository(connectionString);
            _tagRepository = new TagRepository(connectionString);
            _blogId = blogId;
        }

        public IUserInterfaceManager Execute()
        {
            Blog blog = _blogRepository.Get(_blogId);
            Console.WriteLine($"{blog.Title} Details: ");
            Console.WriteLine(" 1) View Blog");
            Console.WriteLine(" 2) Add Tag");
            Console.WriteLine(" 3) Remove Tag");
            Console.WriteLine(" 4) View Blog Posts");
            Console.WriteLine(" 5) View Blog Tags");
            Console.WriteLine(" 0) Go Back");

            Console.Write("> ");
            string Choice = Console.ReadLine();
            switch (Choice)
            {
                case "1":
                    View(blog);
                    return this;
                case "2":
                    AddTag(blog);
                    return this;
                case "3":
                    RemoveTag(blog);
                    return this;
                case "4":
                    ViewBlogPosts(blog);
                    return this;
                case "5":
                    ViewBlogTags(blog);
                    return this;
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }

        private void View(Blog blog)
        {
            Console.WriteLine($"{blog.Title} - {blog.Url} ");
        }

        private void ViewBlogTags(Blog blog)
        {
            Console.WriteLine("---------------------------------------------------------");
            Console.WriteLine("Blog Tags: ");
            foreach (Tag tag in blog.Tags)
            {
                Console.WriteLine(tag);
            }
            Console.WriteLine("---------------------------------------------------------");

        }

        private void AddTag(Blog blog)
        {
            Console.WriteLine($"Which tag would you like to add to {blog.Title}?");
            List<Tag> tags = _tagRepository.GetAll();
            for (int i = 0; i < tags.Count; i++)
            {
                Tag tag = tags[i];
                Console.WriteLine($" {i + 1}) {tag.Name}");
            }
            Console.Write(">");

            int choice = int.Parse(Console.ReadLine());
            try
            {
                Tag tag = tags[choice - 1];
                _blogRepository.InsertTag(blog, tag);

            }
            catch (Exception)
            {
                Console.WriteLine("Invalid Selection. Won't add any tags");
            }
        }

        private void RemoveTag(Blog blog)
        {
            Console.WriteLine($"Which tag would you like to remove from {blog.Title}");
            List<Tag> tags = blog.Tags;
            for (int i = 0; i < tags.Count; i++)
            {
                Tag tag = tags[i];
                Console.WriteLine($"{i + 1}) {tag.Name}");
            }
            Console.Write("> ");

            int choice = int.Parse(Console.ReadLine());
            try
            {
                Tag tag = tags[choice - 1];
                _blogRepository.DeleteTag(blog, tag);
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid Selection. Won't remove any tags.");
            }
        }

        private void ViewBlogPosts(Blog blog)
        {
            List<Post> posts = _postRepository.GetByBlog(_blogId);
            foreach (Post post in posts)
            {
                Console.WriteLine($"{post.Title}");
            }
            Console.WriteLine();
        }

    }
}