using Bank.Core.DataAccess;
using Bank.Core.Entities.Concrete;
using Bank.DataAccess.Abstract;
using Bank.DataAccess.EntityFramework.Context;
using Bank.Entity.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.DataAccess.EntityFramework
{
    public class EfUserDal : EfEntityRepositoryBase<User, BankContext>, IUserDal
    {
        public async Task<List<Role>> GetRoles(User user)
        {
            using (var context = new BankContext())
            {
                var result = from role in context.Roles
                             join userRole in context.UserRoles
                                 on role.Id equals userRole.RoleId
                             where userRole.UserId == user.Id
                             select new Role { Id = role.Id, RoleName = role.RoleName };

                return await result.ToListAsync();
            }
        }

        public async Task<User> AddAndReturnIdAsync(User user)
        {
            using (var context = new BankContext())
            {
                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();
                return user;
            }
        }

        public async Task<string> GetPhoneByUserIdAsync(int id)
        {
            using (var context = new BankContext())
            {
                return await context.Users
                                    .Where(u => u.Id == id)
                                    .Select(u => u.Phone)
                                    .FirstOrDefaultAsync();
            }
        }

        public async Task<UserInfoDto> GetUser(int id)
        {
            using (var context = new BankContext())
            {
                return await (from u in context.Users
                              join b in context.Branches on u.BranchId equals b.Id
                              where u.Id == id
                              select new UserInfoDto
                              {
                                  FullName = u.FullName,
                                  Email = u.Email,
                                  Phone = u.Phone,
                                  Branch = b.BranchName
                              }).FirstOrDefaultAsync();
            }
        }

    }

}
