using Data.IServices;
using Data.Models;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class UserService : IUserService
    {
        SqlConnection connection;
        SqlServerCompiler compiler;
        QueryFactory db;

        public UserService()
        {
            this.connection = new SqlConnection("Server=DESKTOP-U1O9HJN;Database=SaleManagementSystemDb;Trusted_Connection=True;TrustServerCertificate=true;");
            this.compiler = new SqlServerCompiler();
            this.db = new QueryFactory(connection, compiler);
            connection.Open();
        }

        public IEnumerable<User> Get()
        {
            var users = db.Query("User").Where("IsDeleted", false).Get<User>();
            return users;
        }

        public User GetById(int id)
        {
            var user = db.Query("User").Where("Id", id).First<User>();
            return user;
        }

        public User GetByGuid(Guid guid)
        {
            var user = db.Query("User").Where("Guid", guid).First<User>();
            return user;
        }
    }
}
