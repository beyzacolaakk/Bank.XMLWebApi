using Bank.Core.DataAccess;
using Bank.DataAccess.Abstract;
using Bank.DataAccess.EntityFramework.Context;
using Bank.Entity.Concrete;
using Bank.Entity.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.DataAccess.EntityFramework
{
    public class EfSupportRequestDal : EfEntityRepositoryBase<SupportRequest, BankContext>, ISupportRequestDal
    {
        private readonly BankContext _context;
        public EfSupportRequestDal(BankContext context)
        {
            _context = context;
        }

        public async Task UpdateStatus(int id, string newStatus)
        {
            using (var context = new BankContext())
            {
                var entity = new SupportRequest { Id = id };
                context.SupportRequests.Attach(entity);
                entity.Status = newStatus;
                context.Entry(entity).Property(x => x.Status).IsModified = true;
                await context.SaveChangesAsync();
            }
        }

        public async Task<bool> UpdateSupportRequestStatus(int id, string newStatus, string response)
        {
            using (var context = new BankContext())
            {
                var supportRequest = await context.SupportRequests.FindAsync(id);
                if (supportRequest == null)
                {
                    return false;
                }

                supportRequest.Status = newStatus;
                supportRequest.Response = response;
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<List<SupportRequestDto>> GetSupportRequests()
        {
            using (var context = new BankContext())
            {
                var result = await (from request in context.SupportRequests
                                    join user in context.Users
                                    on request.UserId equals user.Id
                                    select new SupportRequestDto
                                    {
                                        Date = request.CreatedDate,
                                        FullName = user.FullName,
                                        Status = request.Status,
                                        Category = request.Category,
                                        Subject = request.Subject,
                                        Message = request.Message,
                                        UserId = user.Id,
                                        Id = request.Id,
                                        Response = request.Response,
                                    }).ToListAsync();

                return result;
            }
        }

        public async Task<SupportRequestDto?> GetSupportRequestById(int id)
        {
            using (var context = new BankContext())
            {
                var result = await (from request in context.SupportRequests
                                    join user in context.Users
                                    on request.UserId equals user.Id
                                    where request.Id == id
                                    select new SupportRequestDto
                                    {
                                        Date = request.CreatedDate,
                                        FullName = user.FullName,
                                        Status = request.Status,
                                        Category = request.Category,
                                        Subject = request.Subject,
                                        Message = request.Message,
                                        UserId = user.Id,
                                        Id = request.Id,
                                        Response = request.Response,
                                    }).FirstOrDefaultAsync();

                return result;
            }
        }
    }

}
