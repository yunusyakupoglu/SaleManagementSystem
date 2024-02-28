using Data.IServices;
using Data.Models.Dto;
using Data.Models.Project;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Data.Services
{
    public class TicketService : ITicketService
    {
        SqlConnection connection;
        SqlServerCompiler compiler;

        public TicketService()
        {
            this.compiler = new SqlServerCompiler();
        }

        private QueryFactory CreateQueryFactory()
        {
            this.connection = new SqlConnection("Server=DESKTOP-U1O9HJN;Database=SaleManagementSystemDb;Trusted_Connection=True;TrustServerCertificate=true;");
            return new QueryFactory(this.connection, this.compiler);
        }

        static string GenerateRandomCode(int length)
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string numbers = "0123456789";
            var random = new Random();

            var randomChars = Enumerable.Repeat(letters, length / 2)
                                        .Select(s => s[random.Next(s.Length)])
                                        .ToList();

            randomChars.AddRange(Enumerable.Repeat(numbers, length - length / 2)
                                           .Select(s => s[random.Next(s.Length)]));

            return string.Concat(randomChars.OrderBy(_ => random.Next()));
        }

        public IEnumerable<Ticket> Get()
        {
            using (var db = CreateQueryFactory())
            {
                var tickets = db.Query("Ticket").Where("IsDeleted", false).Get<Ticket>();
                return tickets;
            }
        }

        public IEnumerable<TicketView> Filter(string filter)
        {
            using (var db = CreateQueryFactory())
            {
                var tickets = db.Query("TicketView").Where("CompanyName", "LIKE", $"{filter}%")
                    .Get<TicketView>();
                return tickets;
            }
        }

        public dynamic GetPage(int page, int dataPerPage)
        {
            using (var db = CreateQueryFactory())
            {
                var tickets = db.Query("TicketView").Where("IsDeleted", false).Paginate<TicketView>(page, dataPerPage);
                return tickets;
            }
        }


        public Ticket GetById(int id)
        {
            using (var db = CreateQueryFactory())
            {
                var ticket = db.Query("Ticket").Where("Id", id).First<Ticket>();
                return ticket;
            }
        }

        public Ticket GetByGuid(Guid guid)
        {
            using (var db = CreateQueryFactory())
            {
                var ticket = db.Query("Ticket").Where("Guid", guid).FirstOrDefault<Ticket>();
                return ticket;
            }
        }

        public Guid Insert(Ticket ticket)
        {
            using (var db = CreateQueryFactory())
            {
                var guid = Guid.NewGuid();
                db.Query("Ticket").Insert(new
                {
                    Company = ticket.Company,
                    SumSellPrice = ticket.SumSellPrice,
                    SumVat = ticket.SumVat,
                    SumDiscount = ticket.SumDiscount,
                    SubSellPrice = ticket.SubSellPrice,
                    NetTotalPrice = ticket.NetTotalPrice,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    IsActive = true,
                    Guid = guid,
                    UniqueTicketCode = "#Bee-" + GenerateRandomCode(5)
                });
                return guid;
            }
        }

        public void Update(Ticket ticket)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Ticket").Where("Guid", ticket.Guid).Update(new
                {
                    Company = ticket.Company,
                    SumSellPrice = ticket.SumSellPrice,
                    SumVat = ticket.SumVat,
                    SumDiscount = ticket.SumDiscount,
                    SubSellPrice = ticket.SubSellPrice,
                    NetTotalPrice = ticket.NetTotalPrice,
                });
            }
        }

        public void Delete(Guid guid)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Ticket").Where("Guid", guid).Delete();
            }
        }

        public void UpdateIsActive(Guid guid, bool isActive)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Ticket").Where("Guid", guid).Update(new
                {
                    IsActive = isActive
                });
            }
        }

        public void UpdateDeleted(Guid guid)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Ticket").Where("Guid", guid).Update(new
                {
                    IsDeleted = true
                });
            }
        }
    }
}
