using Data.IServices;
using Data.Models.Dto;
using Data.Models.Project;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Data.Services
{
    public class TicketProductService : ITicketProductService
    {
        SqlConnection connection;
        SqlServerCompiler compiler;
        QueryFactory db;

        public TicketProductService()
        {
            this.connection = new SqlConnection("Server=DESKTOP-U1O9HJN;Database=SaleManagementSystemDb;Trusted_Connection=True;TrustServerCertificate=true;");
            this.compiler = new SqlServerCompiler();
            this.db = new QueryFactory(connection, compiler);
            connection.Open();
        }

        public IEnumerable<TicketProduct> Get()
        {
            var ticketProducts = db.Query("TicketProduct").Where("IsDeleted", false).Get<TicketProduct>();
            return ticketProducts;
        }

        public TicketProduct GetById(int id)
        {
            var ticketProduct = db.Query("TicketProduct").Where("Id", id).First<TicketProduct>();
            return ticketProduct;
        }

        public TicketProduct GetByGuid(Guid guid)
        {
            var ticketProduct = db.Query("TicketProduct").Where("Guid", guid).FirstOrDefault<TicketProduct>();
            return ticketProduct;
        }

        public void Insert(TicketProduct ticketProduct)
        {
            db.Query("TicketProduct").Insert(new
            {
                Stock = ticketProduct.Stock,
                Quantity = ticketProduct.Quantity,
                Ticket = ticketProduct.Ticket,
                CreatedDate = DateTime.Now,
                IsDeleted = false,
                IsActive = true,
                Guid = Guid.NewGuid(),
            });
        }

        public IEnumerable<TicketProductResult> InsertRange(IEnumerable<TicketProduct> ticketProducts)
        {
            List<TicketProductResult> results = new List<TicketProductResult>();
            foreach (var item in ticketProducts)
            {
                db.Query("TicketProduct").Insert(new
                {
                    Stock = item.Stock,
                    Quantity = item.Quantity,
                    Ticket = item.Ticket,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    IsActive = true,
                    Guid = Guid.NewGuid(),
                });
                results.Add(new TicketProductResult { Stock = item.Stock, Quantity = item.Quantity });
            }

            return results;
        }

        public void Update(TicketProduct ticketProduct)
        {
            db.Query("TicketProduct").Where("Guid", ticketProduct.Guid).Update(new
            {
                Stock = ticketProduct.Stock,
                Quantity = ticketProduct.Quantity,
                Ticket = ticketProduct.Ticket
            });
        }

        public void Delete(Guid guid)
        {
            db.Query("TicketProduct").Where("Guid", guid).Delete();
        }

        public void UpdateIsActive(Guid guid, bool isActive)
        {
            db.Query("TicketProduct").Where("Guid", guid).Update(new
            {
                IsActive = isActive
            });
        }

        public void UpdateDeleted(Guid guid)
        {
            db.Query("TicketProduct").Where("Guid", guid).Update(new
            {
                IsDeleted = true
            });
        }
    }
}
