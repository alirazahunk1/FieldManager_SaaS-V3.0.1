using CZ.Api.Base;
using ESSWebApi.Dtos.Request.Expense;
using ESSWebApi.Routes;
using ESSWebApi.Services.Expense;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESSWebApi.Controllers
{
    [Authorize, ApiController]
    public class ExpenseController : BaseController
    {
        private readonly IExpense _expense;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ExpenseController(IExpense expense,
            IWebHostEnvironment webHostEnvironment)
        {
            _expense = expense;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet(APIRoutes.Expense.GetExpenseTypes)]
        public async Task<IActionResult> GetExpenseTypes()
        {
            var result = await _expense.GetExpenseTypes();

            if (!result.IsSuccess) return BadRequest(result.Message);

            return Ok(result.Data);
        }

        [HttpPost(APIRoutes.Expense.CreateExpenseRequest)]
        public async Task<IActionResult> CreateExpenseRequest([FromBody] CreateExpenseRequest request)
        {
            if (!request.Amount.HasValue) return BadRequest("Amount is required");

            if (!request.TypeId.HasValue) return BadRequest("TypeId is required");

            if (string.IsNullOrEmpty(request.Date)) return BadRequest("Date is required");

            var result = await _expense.CreateExpenseRequest(GetUserId(), request);

            if (!result.IsSuccess) return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [HttpGet(APIRoutes.Expense.GetExpenseRequests)]
        public async Task<IActionResult> GetExpenseRequests()
        {
            var result = await _expense.GetExpenseRequests(GetUserId());

            return Ok(result.Data);
        }

        [HttpPost(APIRoutes.Expense.DeleteExpenseRequest)]
        public async Task<IActionResult> DeleteExpenseRequest([FromBody] int id)
        {
            var result = await _expense.DeleteExpenseRequest(id);

            if (!result.IsSuccess) return BadRequest(result.Message);

            return Ok("Success");
        }

        [HttpPost(APIRoutes.Expense.UploadExpenseDocument)]
        public async Task<IActionResult> UploadDocument(IFormFile file)
        {

            if (file == null)
                return BadRequest("Invalid file");

            var userId = GetUserId();

            var lastRequestId = await _expense.GetLastRequestId(userId);

            if (lastRequestId == null)
                BadRequest("No recent request found");


            try
            {
                string path = _webHostEnvironment.WebRootPath + "\\ExpenseRequestDocuments\\";


                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string extension = Path.GetExtension(file.FileName);
                var uniqueFileName = $@"{userId}_{DateTime.Now.Ticks}" + extension;
                var filePath = $"{path}\\{uniqueFileName}";

                // Full path to file in temp location
                if (file.Length > 0)
                    using (var stream = new FileStream(filePath, FileMode.Create))
                        await file.CopyToAsync(stream);


                await _expense.UpdateDocument(lastRequestId.Value, "ExpenseRequestDocuments" + "/" + uniqueFileName);

                return Ok("Document added");
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong!");
            }
        }
    }
}
