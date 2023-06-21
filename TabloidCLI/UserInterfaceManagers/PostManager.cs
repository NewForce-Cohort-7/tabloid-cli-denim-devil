using System;
using System.Collections.Generic;
using TabloidCLI.Models;
using TabloidCLI.Repositories;

namespace TabloidCLI.UserInterfaceManagers
{
    public class PostManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;
        private PostRepository _postRepository;
        private AuthorRepository _authorRepository;
        private BlogRepository _blogRepository;
        private string _connectionString;

        public PostManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _postRepository = new PostRepository(connectionString);
            _authorRepository = new AuthorRepository(connectionString);
            _blogRepository = new BlogRepository(connectionString);
            _connectionString = connectionString;
        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Post Menu");
            Console.WriteLine(" 1) List Posts");
            Console.WriteLine(" 2) Post Details");
            Console.WriteLine(" 3) Add Post");
            Console.WriteLine(" 4) Edit Post");
            Console.WriteLine(" 5) Remove Post");
            Console.WriteLine(" 0) Go Back");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    List();
                    return this;
                case "2":
                    Post post = Choose();
                    if (post == null)
                    {
                        return this;
                    }
                    else
                    {
                        return new PostDetailManager(this, _connectionString, post.Id);
                    }
                case "3":
                    Add();
                    return this;
                case "4":
                    Edit();
                    return this;
                case "5":
                    Remove();
                    return this;
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }

        private void List()
        {
            Console.WriteLine();
            List<Post> posts = _postRepository.GetAll();
            foreach (Post post in posts)
            {
                Console.WriteLine(post.Title);
            }
            Console.WriteLine();
        }



        private Post Choose(string prompt = null)
        {
            if (prompt == null)
            {
                prompt = "Please choose a Post:";
            }

            Console.WriteLine(prompt);

            List<Post> posts = _postRepository.GetAll();

            for (int i = 0; i < posts.Count; i++)
            {
                Post post = posts[i];
                Console.WriteLine($" {i + 1}) {post.Title}");
            }
            Console.Write("> ");

            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                return posts[choice - 1];
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid Selection");
                return null;
            }
        }

        private void Add()
        {
            Console.WriteLine();
            Console.WriteLine("New Post");
            Post post = new Post();

            Console.Write("Title: ");
            post.Title = Console.ReadLine();

            Console.Write("URL: ");
            post.Url = Console.ReadLine();

            Console.Write("Publication Date (MM/DD/YYYY): ");
            post.PublishDateTime = DateTime.Parse(Console.ReadLine());

            Console.WriteLine();
            Console.WriteLine("Select Author: ");

            List<Author> authors = _authorRepository.GetAll();
            for (int i = 0; i < authors.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {authors[i].FirstName} {authors[i].LastName}");
            }
            int selectedAuthorId = Convert.ToInt32(Console.ReadLine()) - 1;
            post.Author = authors[selectedAuthorId];

            Console.WriteLine();
            Console.WriteLine("Select Blog:");
            List<Blog> blogs = _blogRepository.GetAll();
            for (int i = 0; i < blogs.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {blogs[i].Title}");
            }
            int selectedBlogIndex = Convert.ToInt32(Console.ReadLine()) - 1;
            post.Blog = blogs[selectedBlogIndex];

            _postRepository.Insert(post);

            Console.WriteLine();
            Console.WriteLine("Post added successfully!");
            Console.WriteLine();
        }

        private void Edit()
        {
            Console.WriteLine();
            Post postToEdit = Choose("Which post would you like to edit?");
            if (postToEdit == null)
            {
                return;
            }

            Console.WriteLine();
            Console.Write("New title (blank to leave unchanged): ");
            string title = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(title))
            {
                postToEdit.Title = title;
            }
            Console.Write("New URL (blank to leave unchanged): ");
            string url = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(url))
            {
                postToEdit.Url = url;
            }
            Console.Write("New publication date MM/DD/YYYY (blank to leave unchanged): ");
            string date = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(date))
            {
                postToEdit.PublishDateTime = DateTime.Parse(date);
            }

            _postRepository.Update(postToEdit);

            Console.WriteLine();
            Console.WriteLine("Post updated successfully!");
            Console.WriteLine();
        }

        private void Remove()
        {
            Console.WriteLine();
            Post postToDelete = Choose("Which post would you like to remove?");
            if (postToDelete != null)
            {
                _postRepository.Delete(postToDelete.Id);
            }
            Console.WriteLine();
            Console.WriteLine("Post deleted successfully!");
            Console.WriteLine();
        }
    }
}
