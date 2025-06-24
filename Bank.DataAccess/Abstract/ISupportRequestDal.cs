using Bank.Core.DataAccess;
using Bank.Entity.Concrete;
using Bank.Entity.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.DataAccess.Abstract
{
    public interface ISupportRequestDal : IEntityRepository<SupportRequest>
    {
        Task UpdateStatus(int id, string newStatus);

        Task<List<SupportRequestDto>> GetSupportRequests();

        Task<bool> UpdateSupportRequestStatus(int id, string newStatus, string response);

        Task<SupportRequestDto?> GetSupportRequestById(int id);
    }

}
