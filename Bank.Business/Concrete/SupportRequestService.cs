using Bank.Business.Abstract;
using Bank.Business.Constant;
using Bank.Core.Extensions;
using Bank.Core.Utilities.Results;
using Bank.DataAccess.Abstract;
using Bank.Entity.Concrete;
using Bank.Entity.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Bank.Business.Concrete
{
    public class SupportRequestService : ISupportRequestService
    {
        private readonly ISupportRequestDal _supportRequestDal;

        public SupportRequestService(ISupportRequestDal supportRequestDal)
        {
            _supportRequestDal = supportRequestDal;
        }

        public async Task<IResult> Add(SupportRequest supportRequest)
        {
            await _supportRequestDal.Add(supportRequest);
            return new SuccessResult(Messages.AddSuccessful);
        }

        public async Task<IResult> Update(SupportRequest supportRequest)
        {
            await _supportRequestDal.Update(supportRequest);
            return new SuccessResult(Messages.UpdateSuccessful);
        }

        public async Task<IResult> Delete(int id)
        {
            var supportRequest = GetById(id).Result.Data;
            await _supportRequestDal.Delete(supportRequest);
            return new SuccessResult(Messages.DeleteSuccessful);
        }

        public async Task<IResult> CreateSupportRequest(SupportRequestDto dto)
        {
            var supportRequest = new SupportRequest
            {
                Status = "Open",
                Subject = dto.Subject,
                UserId = dto.UserId,
                CreatedDate = DateTime.Now,
                Message = dto.Message,
                Category = dto.Category
            };

            string xml = XmlConverter.Serialize(supportRequest);
            string xsdPath = @"C:\Users\fb_go\source\repos\Bank.XMLWebApi\Bank.XMLWebApi\Schemas\SupportRequest.xsd";

            if (!XmlValidator.ValidateXml(xml, xsdPath, out var errors))
            {
                return new ErrorResult("XML validation failed:\n" + string.Join("\n", errors));
            }

            await _supportRequestDal.Add(supportRequest);
            return new SuccessResult(Messages.AddSuccessful);
        }

        public async Task<IResult> UpdateStatus(int id)
        {
            await _supportRequestDal.UpdateStatus(id, "Completed");
            return new SuccessResult(Messages.UpdateSuccessful);
        }

        public async Task<IDataResult<List<SupportRequest>>> GetAll()
        {
            var list = await _supportRequestDal.GetAll();
            return new SuccessDataResult<List<SupportRequest>>(list, Messages.RetrieveSuccessful);
        }

        public async Task<IDataResult<List<SupportRequest>>> GetAllByUserId(int userId) 
        {
      
            var list = await _supportRequestDal.GetAll();


            string xml = XmlConverter.Serialize(list);


            var doc = XDocument.Parse(xml);


            var nodes = doc
                .XPathSelectElements($"/ArrayOfSupportRequest/SupportRequest[userId={userId}]");

            var filtered = nodes.Select(e => new SupportRequest
            {
                Id = (int?)e.Element("id") ?? 0,
                UserId = (int?)e.Element("userId") ?? 0,
                Subject = (string?)e.Element("subject") ?? string.Empty,
                Message = (string?)e.Element("message") ?? string.Empty,
                Status = (string?)e.Element("status") ?? string.Empty,
                Response = (string?)e.Element("response") ?? string.Empty,
                Category = (string?)e.Element("category") ?? string.Empty,
                CreatedDate = (DateTime?)e.Element("createdDate") ?? DateTime.MinValue
            }).ToList();


            return new SuccessDataResult<List<SupportRequest>>(filtered, Messages.RetrieveSuccessful);
        }

        public async Task<IDataResult<List<SupportRequestDto>>> GetSupportRequests()
        {
            var list = await _supportRequestDal.GetSupportRequests();
            return new SuccessDataResult<List<SupportRequestDto>>(list, Messages.RetrieveSuccessful);
        }

        public async Task<IDataResult<SupportRequest>> GetById(int id)
        {
            var request = await _supportRequestDal.Get(x => x.Id == id);
            return new SuccessDataResult<SupportRequest>(request, Messages.RetrieveSuccessful);
        }

        public async Task<IResult> UpdateSupportRequestStatus(SupportRequestUpdateDto dto)
        {
            var updated = await _supportRequestDal.UpdateSupportRequestStatus(dto.Id, dto.Status!, dto.Response!);
            return updated
                ? new SuccessResult(Messages.UpdateSuccessful)
                : new ErrorResult(Messages.UpdateFailed);
        }

        public async Task<IDataResult<SupportRequestDto>> GetSupportRequestById(int id)
        {
            var request = await _supportRequestDal.GetSupportRequestById(id);
            return new SuccessDataResult<SupportRequestDto>(request!, Messages.RetrieveSuccessful);
        }

        public async Task<IDataResult<List<SupportRequest>>> FilterSupportRequests(int userId, string status, string search)
        {
            var list = await _supportRequestDal.GetAll(x => x.UserId == userId);

            var filtered = list.AsQueryable();

            if (!string.Equals(status, "all", StringComparison.OrdinalIgnoreCase))
            {
                filtered = filtered.Where(t => t.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(search))
            {
                filtered = filtered.Where(t =>
                    t.Subject.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    t.Message.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            var result = filtered.OrderByDescending(t => t.CreatedDate).ToList();

            return new SuccessDataResult<List<SupportRequest>>(result);
        }


    }


}
