using Bank.Business.Abstract;
using Bank.Business.Constant;
using Bank.Core.Utilities.Results;
using Bank.DataAccess.Abstract;
using Bank.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Business.Concrete
{
    public class LoginTokenService : ILoginTokenService
    {
        private readonly ILoginTokenDal _loginTokenDal;

        public LoginTokenService(ILoginTokenDal loginTokenDal)
        {
            _loginTokenDal = loginTokenDal;
        }

        public async Task<IResult> Add(LoginToken loginToken)
        {
            await _loginTokenDal.Add(loginToken);
            return new SuccessResult(Messages.AddSuccessful);
        }

        public async Task<IResult> Update(LoginToken loginToken)
        {
            await _loginTokenDal.Update(loginToken);
            return new SuccessResult(Messages.UpdateSuccessful);
        }

        public async Task<IResult> Delete(LoginToken loginToken)
        {
            await _loginTokenDal.Delete(loginToken);
            return new SuccessResult(Messages.DeleteSuccessful);
        }

        public async Task<IDataResult<List<LoginToken>>> GetAll()
        {
            var list = await _loginTokenDal.GetAll();
            return new SuccessDataResult<List<LoginToken>>(list, Messages.GetAllSuccessful);
        }

        public async Task<IDataResult<LoginToken>> GetById(int id)
        {
            var data = await _loginTokenDal.Get(g => g.Id == id);
            return new SuccessDataResult<LoginToken>(data, Messages.GetByIdSuccessful);
        }
    }

}
