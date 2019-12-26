using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Database;
using Database.Interfaces;
using Database.Models;
using Messenger.Services;
using Messenger.ViewModels.User;

namespace Messenger.Managers
{
    public class UserManager
    {
        private IUserRepository userRepository;
        private SHA256 hasher;
        private EmailService emailService;

        private const string PassSalt = "432532532523";
        private const string EmailSalt = "532532fdsgas";

        public UserManager(IUserRepository userRepository,EmailService emailService)
        {
            this.userRepository = userRepository;
            this.emailService = emailService;
            hasher = SHA256.Create();
        }

        public async Task Register(UserRegisterViewModel model, UriBuilder callback)
        {
            if (model == null)
            {
                throw new Exception("model is null");
            }

            var user = await userRepository.GetByEmail(model.Email);
            if (user != null && user.IsConfirmed)
            {
                throw new Exception("User email is already registered");
            }

            if (user == null)
            {
                var newUser = model.ToUser();
                newUser.PasswordHash = ComputeHash(model.Password + PassSalt);
                newUser.RegistrationDate = DateTime.Now;
                newUser.ConfirmationToken = ComputeHash(newUser.Email + EmailSalt);
                await userRepository.Create(newUser);
                SendConfirmationEmail(newUser, callback);
            }
            else
            {
                user.Email = model.Email;
                user.FirstName = model.FirstName;
                user.SecondName = model.SecondName;
                user.Username = model.Username;
                user.PasswordHash = ComputeHash(model.Password + PassSalt);
                user.RegistrationDate = DateTime.Now;
                user.ConfirmationToken = ComputeHash(user.Email + EmailSalt);
                await userRepository.Update(user);
                SendConfirmationEmail(user, callback);
            }
        }

        public async Task ConfirmRegistration(string token)
        {
            if (token == null)
            {
                throw new Exception("token is null");
            }

            var user = await userRepository.GetByConfirmationToken(token);
            if (user == null)
            {
                throw new Exception("Invalid token");
            }

            if (user.IsConfirmed)
            {
                throw new Exception("User email is already registered");
            }

            user.IsConfirmed = true;

            await userRepository.Update(user);
        }

        public async Task<User> CheckPassword(string email, string password)
        {
            var user = await userRepository.GetByEmail(email);
            if (user != null)
            {
                return VerifyHash(password+PassSalt, user.PasswordHash) ? user : null;
            }

            return null;
        }

        private string ComputeHash(string data)
        {
            var bytes = hasher.ComputeHash(Encoding.UTF8.GetBytes(data));

            var builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }

        private bool VerifyHash(string password, string hash) => hash == ComputeHash(password);

        private async void SendConfirmationEmail(User user, UriBuilder callback)
        {
            callback.Query = $"token={user.ConfirmationToken}";
            var message = "Для завершения регистрации перейдите по ссылке: <a href=\""
                          + callback.Uri + "\">завершить регистрацию</a>";
            await emailService.SendEmailAsync(user.Email, "Complete your registration", message);
        }
    }
}
