using ESSCommon.Core.Services.Notification;
using ESSDataAccess.DbContext;
using ESSDataAccess.Models;
using ESSWebApi.Dtos.Request.Expense;
using ESSWebApi.Dtos.Result;
using ESSWebApi.Dtos.Result.Expense;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace ESSWebApi.Services.Expense
{
    public class ExpenseService : IExpense
    {
        private readonly AppDbContext _context;
        private readonly INotification _notification;

        public ExpenseService(AppDbContext context,
            INotification notification)
        {
            _context = context;
            _notification = notification;
        }


        public async Task<BaseResult> CreateExpenseRequest(int userId, CreateExpenseRequest request)
        {
            DateTime date = DateTime.Now;
            try
            {
                date = DateTime.ParseExact(request.Date!, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                return new BaseResult
                {
                    Message = "Date parse failed"
                };
            }
            ExpenseRequestModel model = new ExpenseRequestModel
            {
                Amount = Convert.ToDecimal(request.Amount.Value),
                ForDate = date,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Status = ESSDataAccess.Enum.ExpenseStatusEnum.Pending,
                ExpenseTypeId = request.TypeId!.Value,
                Remarks = request.Comments,
                UserId = userId,
            };

            await _context.AddAsync(model);
            await _context.SaveChangesAsync();

            await _notification.PostExpenseRequest(userId, model.Amount.ToString());

            return new BaseResult
            {
                IsSuccess = true
            };

        }

        public async Task<int?> GetLastRequestId(int userId)
        {
            return await _context.ExpenseRequests
                 .Where(x => x.UserId == userId)
                 .OrderByDescending(x => x.CreatedAt)
                 .Select(x => x.Id)
                 .FirstOrDefaultAsync();
        }

        public async Task<BaseResult> DeleteExpenseRequest(int id)
        {
            var expenseRequest = await _context.ExpenseRequests.FindAsync(id);

            if (expenseRequest == null)
            {
                return new BaseResult
                {
                    Message = "Request not exists"
                };
            }

            _context.Remove(expenseRequest);
            await _context.SaveChangesAsync();

            return new BaseResult
            {
                IsSuccess = true,
            };
        }

        public async Task<BaseResult> GetExpenseRequests(int userId)
        {
            List<ExpenseRequestDto> expenseRequests = new List<ExpenseRequestDto>();

            expenseRequests = await _context.ExpenseRequests
                 .Where(x => x.UserId == userId)
                 .OrderByDescending(x => x.CreatedAt)
                 .Include(x => x.ExpenseType)
                 .Take(20)
                 .Select(x => new ExpenseRequestDto
                 {
                     Id = x.Id,
                     Date = x.ForDate.ToString("dd/MM/yyyy"),
                     Type = x.ExpenseType.Name,
                     ApprovedBy = string.Empty,
                     CreatedAt = x.CreatedAt.ToString("dd/MM/yyyy"),
                     Status = x.Status.ToString(),
                     ActualAmount = x.Amount,
                     ApprovedAmount = x.ApprovedAmount,
                 }).ToListAsync();

            return new BaseResult
            {
                IsSuccess = true,
                Data = expenseRequests
            };
        }

        public async Task<BaseResult> UpdateDocument(int requestId, string fileName)
        {
            var request = await _context.ExpenseRequests.FindAsync(requestId);
            if (request == null)
            {
                return new BaseResult
                {
                    IsSuccess = false,
                };
            }

            request.ImgUrl = fileName;
            _context.Update(request);
            await _context.SaveChangesAsync();

            return new BaseResult
            {
                IsSuccess = true,
            };

        }

        public async Task<BaseResult> GetExpenseTypes()
        {
            var result = await _context.ExpenseTypes
                .Select(x => new ExpenseTypeDto
                {
                    Id = x.Id,
                    IsImgRequired = x.IsImgRequired,
                    Name = x.Name
                }).ToListAsync();

            return new BaseResult
            {
                IsSuccess = true,
                Data = result
            };
        }
    }
}
