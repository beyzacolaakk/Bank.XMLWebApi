using Bank.Business.Abstract;
using Bank.Business.Constant;
using Bank.Core.Entities.Concrete;
using Bank.Core.Utilities.Results;
using Bank.DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Business.Concrete
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleDal _userRoleDal;

        public UserRoleService(IUserRoleDal userRoleDal)
        {
            _userRoleDal = userRoleDal;
        }

        public async Task<IResult> Add(UserRole userRole)
        {
            await _userRoleDal.Add(userRole);
            return new SuccessResult(Messages.AddSuccessful);
        }

        public async Task<IResult> Update(UserRole userRole)
        {
            await _userRoleDal.Update(userRole);
            return new SuccessResult(Messages.UpdateSuccessful);
        }

        public async Task<IDataResult<List<UserRole>>> GetAll()
        {
            var roles = await _userRoleDal.GetAll();
            return new SuccessDataResult<List<UserRole>>(roles, Messages.GetAllSuccessful);
        }

        public async Task<IDataResult<UserRole>> GetById(int id)
        {
            var role = await _userRoleDal.Get(r => r.Id == id);
            return new SuccessDataResult<UserRole>(role, Messages.GetByIdSuccessful);
        }

        public async Task<IResult> Delete(UserRole userRole)
        {
            await _userRoleDal.Delete(userRole);
            return new SuccessResult(Messages.DeleteSuccessful);
        }
    }

}
