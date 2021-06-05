using SharedTrip.Data;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace SharedTrip.Services
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext db;

        public UsersService(ApplicationDbContext db)
        {
            this.db = db;
        }
        
        public void Create(string username, string email, string password)
        {
            var user = new User
            {
                Username = username,
                Email = email,
                Password = ComputeHash(password)
            };
            this.db.Users.Add(user);
            this.db.SaveChanges();

        }

        public bool IsEmailAvailable(string email)
        {
            return !this.db.Users.Any(x => x.Email == email);
        }

        public string GetUserId(string username, string password)
        {
            var hashedPassword = ComputeHash(password);
            var user =  this.db.Users.FirstOrDefault(x => x.Username == username && x.Password == hashedPassword);
            return user?.Id;
        }

        public bool IsUserNameAvailable(string username)
        {
            return !this.db.Users.Any(x => x.Username == username);
        }
        private static string ComputeHash(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            using var hash = SHA512.Create();
            var hashedIntputBytes = hash.ComputeHash(bytes);

            var hashedInputStringBuilder = new StringBuilder(128);
            foreach (var b in hashedIntputBytes)
            {
                hashedInputStringBuilder.Append(b.ToString("X2"));
                
            }
            return hashedInputStringBuilder.ToString();

        }
    }
}
