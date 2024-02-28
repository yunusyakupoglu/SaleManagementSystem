using Data.Common;
using Data.IServices;
using Data.Models;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Data.SqlClient;

namespace Data.Services
{
    public class AccountService : IAccountService
    {
        SqlConnection connection;
        SqlServerCompiler compiler;
        QueryFactory db;

        public AccountService()
        {
            this.connection = new SqlConnection("Server=DESKTOP-U1O9HJN;Database=SaleManagementSystemDb;Trusted_Connection=True;TrustServerCertificate=true;");
            this.compiler = new SqlServerCompiler();
            this.db = new QueryFactory(connection, compiler);
            connection.Open();
        }

        public Login Login(string Username, string Password)
        {
            Login login = new Login();
            string hashedPassword = PasswordHasher.HashPassword(Password);

            var user = db.Query("User").Where("Username", Username).FirstOrDefault<User>();

            if (user != null)
            {
                if (PasswordHasher.VerifyPassword(Password, user.Password))
                {
                    login.Username = user.Username;
                    login.Email = user.Username;
                    login.NameSurname = user.FirstName + " " + user.LastName;
                    login.Role = user.Role;
                    return login;
                }
                else
                {
                    login = null;
                    return login;
                }
            }
            else
            {
                login = null;
                return login;
            }
        }

        public User GetUser(string email, string password)
        {
            var user = db.Query("User").Where("Email", email)
             .Where("Password", password).First<User>();
            return user;
        }

        public User GetCookieUser(string username)
        {
            var user = db.Query("User").Where("Username", username).FirstOrDefault<User>();
            return user;
        }

        public void Register(User user)
        {
            var count = db.Query("User").Count<int>();
            if (count != 0)
            {
                string hashedPassword = PasswordHasher.HashPassword(user.Password);
                string hashedPasswordConfirm = PasswordHasher.HashPassword(user.PasswordConfirm);
                if (hashedPassword.Equals(hashedPasswordConfirm))
                {
                    db.Query("User").Insert(new
                    {
                        Username = user.Username,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Password = hashedPassword,
                        PasswordConfirm = hashedPasswordConfirm,
                        EmailConfirm = false,
                        Role = user.Role,
                        IsActive = true,
                        IsDeleted = false,
                        Guid = Guid.NewGuid(),
                        CreatedDate = DateTime.Now
                    });
                }
            }
            else
            {
                var guid = db.Query("Role")
             .Where("RoleName", "Admin")
             .Select("Guid")
             .FirstOrDefault<Guid>();

                string password = "super@beekod";
                string hashedPassword = PasswordHasher.HashPassword(password);

                db.Query("User").Insert(new
                {
                    Username = "superadmin",
                    FirstName = "Super",
                    LastName = "Admin",
                    Email = "superadmin@beekod.com",
                    Password = hashedPassword,
                    PasswordConfirm = hashedPassword,
                    EmailConfirm = true,
                    Role = guid,
                    IsActive = true,
                    IsDeleted = false,
                    Guid = Guid.NewGuid(),
                    CreatedDate = DateTime.Now
                });

                var userdt = db.Query("User")
.Where("Username", "superadmin")
.Select("Guid")
.FirstOrDefault<Guid>();

                db.Query("Permission").Insert(new
                {
                    CanEdit = true,
                    CanInsert = false,
                    CanView = true,
                    CanDelete = true,
                    IsActive = true,
                    IsDeleted = false,
                    Guid = Guid.NewGuid(),
                    UserGuid = userdt,
                    CreatedDate = DateTime.Now
                });
            }
        }

        public CheckAuthEntities Check()
        {
            var userCount = db.Query("User").Count<int>();
            var roleCount = db.Query("Role").Count<int>();
            CheckAuthEntities check = new CheckAuthEntities
            {
                UserCount = userCount,
                RoleCount = roleCount
            };

            return check;
        }
    }
}
