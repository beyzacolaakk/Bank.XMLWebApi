using Bank.Core.Utilities.Results;
using Bank.Entity.Concrete;
using Bank.Entity.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Business.Abstract
{
    public interface ISupportRequestService
    {
        Task<IDataResult<List<SupportRequest>>> GetAll();

        Task<IResult> Add(SupportRequest supportRequest);

        Task<IResult> Update(SupportRequest supportRequest);

        Task<IResult> Delete(int id);

        Task<IResult> CreateSupportRequest(SupportRequestDto supportRequestCreateDto);

        Task<IDataResult<SupportRequest>> GetById(int id);


        Task<IDataResult<List<SupportRequest>>> GetAllByUserId(int userId);

        Task<IDataResult<List<SupportRequestDto>>> GetSupportRequests();

        Task<IResult> UpdateSupportRequestStatus(SupportRequestUpdateDto dto);

        Task<IDataResult<SupportRequestDto>> GetSupportRequestById(int id);

        Task<IDataResult<List<SupportRequest>>> FilterSupportRequests(int userId, string status, string search);
    }

}
