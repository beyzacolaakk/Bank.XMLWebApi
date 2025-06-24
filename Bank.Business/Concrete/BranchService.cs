using Bank.Business.Abstract;
using Bank.Business.Constant;
using Bank.Core.Utilities.Results;
using Bank.DataAccess.Abstract;
using Bank.Entity.Concrete;
using Bank.Entity.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Business.Concrete
{
    public class BranchService : IBranchService
    {
        private readonly IBranchDal _branchDal;

        public BranchService(IBranchDal branchDal)
        {
            _branchDal = branchDal;
        }

        public async Task<IResult> Add(Branch branch)
        {
            await _branchDal.Add(branch);
            return new SuccessResult(Messages.AddSuccessful);
        }

        public async Task<IResult> Update(Branch branch)
        {
            await _branchDal.Update(branch);
            return new SuccessResult(Messages.UpdateSuccessful);
        }

        public async Task<IDataResult<List<Branch>>> GetAll()
        {
            var list = await _branchDal.GetAll();
            return new SuccessDataResult<List<Branch>>(list, Messages.GetAllSuccessful);
        }

        public async Task<IDataResult<List<BranchDto>>> GetBranches()
        {
            var list = await _branchDal.GetAll();

            var dtoList = list.Select(s => new BranchDto
            {
                Id = s.Id,
                BranchName = s.BranchName
            }).ToList();

            return new SuccessDataResult<List<BranchDto>>(dtoList, Messages.GetAllSuccessful);
        }

        public async Task<IDataResult<Branch>> GetById(int id)
        {
            var entity = await _branchDal.Get(b => b.Id == id);
            return new SuccessDataResult<Branch>(entity, Messages.GetByIdSuccessful);
        }

        public async Task<IResult> Delete(Branch branch)
        {
            await _branchDal.Delete(branch);
            return new SuccessResult(Messages.DeleteSuccessful);
        }
    }

}
