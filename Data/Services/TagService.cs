using Data.IServices;
using Data.Models.Project;
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
    public class TagService : ITagService
    {
        SqlConnection connection;
        SqlServerCompiler compiler;
        QueryFactory db;

        public TagService()
        {
            this.connection = new SqlConnection("Server=DESKTOP-U1O9HJN;Database=SaleManagementSystemDb;Trusted_Connection=True;TrustServerCertificate=true;");
            this.compiler = new SqlServerCompiler();
            this.db = new QueryFactory(connection, compiler);
            connection.Open();
        }

        public IEnumerable<Tag> Get()
        {
            var tags = db.Query("Tag").Where("IsDeleted", false).Get<Tag>();
            return tags;
        }

        public IEnumerable<Tag> GetbyProductGuid(Guid guid)
        {
            var tags = db.Query("Tag").Where("IsDeleted", false).Where("Product", guid).Get<Tag>();
            return tags;
        }

        public Tag GetById(int id)
        {
            var tag = db.Query("Tag").Where("Id", id).First<Tag>();
            return tag;
        }

        public Tag GetByGuid(Guid guid)
        {
            var tag = db.Query("Tag").Where("Guid", guid).FirstOrDefault<Tag>();
            return tag;
        }

        public void Insert(Tag tag)
        {
            db.Query("Tag").Insert(new
            {
                TagName = tag.TagName,
                TagColor = tag.TagColor,
                Product = tag.Product,
                CreatedDate = DateTime.Now,
                IsDeleted = false,
                IsActive = true,
                Guid = Guid.NewGuid(),
            });
        }

        public void Update(Tag tag)
        {
            db.Query("Tag").Where("Guid", tag.Guid).Update(new
            {
                TagName = tag.TagName,
                TagColor = tag.TagColor,
                Product = tag.Product,
            });
        }

        public void Delete(Guid guid)
        {
            db.Query("Tag").Where("Guid", guid).Delete();
        }

        public void UpdateIsActive(Guid guid, bool isActive)
        {
            db.Query("Tag").Where("Guid", guid).Update(new
            {
                IsActive = isActive
            });
        }

        public void UpdateDeleted(Guid guid)
        {
            db.Query("Tag").Where("Guid", guid).Update(new
            {
                IsDeleted = true
            });
        }
    }
}
