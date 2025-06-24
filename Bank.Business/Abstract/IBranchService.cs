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
    public interface IBranchService
    {
        Task<IDataResult<List<Branch>>> GetAll();

        Task<IResult> Add(Branch branch);

        Task<IResult> Update(Branch branch);

        Task<IResult> Delete(Branch branch);

        Task<IDataResult<Branch>> GetById(int id);

        Task<IDataResult<List<BranchDto>>> GetBranches();
    }

}
