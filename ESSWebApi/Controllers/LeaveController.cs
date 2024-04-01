using CZ.Api.Base;
using ESSDataAccess.DbContext;
using ESSWebApi.Dtos.Request.Leave;
using ESSWebApi.Routes;
using ESSWebApi.Services.Leave;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ESSWebApi.Controllers
{
    [Authorize, ApiController]
    public class LeaveController : BaseController
    {
        private readonly ILeave _leave;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public LeaveController(ILeave leave,
            AppDbContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            _leave = leave;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet(APIRoutes.Leave.GetLeaveTypes)]
        public async Task<IActionResult> GetLeaveTypes()
        {
            return Ok(await _leave.GetLeaveTypes());
        }

        [HttpGet(APIRoutes.Leave.GetLeaveRequests)]
        public async Task<IActionResult> GetLeaveRequests()
        {
            var result = await _leave.GetLeaveRequests(GetUserId());

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }

        [HttpPost(APIRoutes.Leave.CreateLeaveRequest)]
        public async Task<IActionResult> CreateLeaveRequest([FromBody] CreateLeaveRequest request)
        {
            var result = await _leave.CreateLeaveRequest(GetUserId(), request);
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost(APIRoutes.Leave.UploadLeaveDocument)]
        public async Task<IActionResult> UploadDocument(IFormFile file)
        {

            if (file == null)
                return BadRequest("Invalid file");

            var userId = GetUserId();



            var lastRequestId = await _leave.GetLastRequestId(userId);

            if (lastRequestId == null)
                BadRequest("No recent request found");


            try
            {
                var leavePath = await _context.SASettings.Select(x => x.LeaveDocumentPath).FirstOrDefaultAsync();
                string path = _webHostEnvironment.WebRootPath + "/LeaveRequestDocuments/";

                if (leavePath != null && !string.IsNullOrEmpty(leavePath))
                {
                    path = leavePath;
                }

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string extension = Path.GetExtension(file.FileName);
                var UniqueFileName = $@"{userId}_{DateTime.Now.Ticks}" + extension;
                var filePath = $"{path}/{UniqueFileName}";

                // Full path to file in temp location
                if (file.Length > 0)
                    using (var stream = new FileStream(filePath, FileMode.Create))
                        await file.CopyToAsync(stream);

                await _leave.UpdateDocument(lastRequestId.Value, "LeaveRequestDocuments" + "/" + UniqueFileName);

                return Ok("Document added");
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong!");
            }
        }

        [HttpPost(APIRoutes.Leave.DeleteLeaveRequest)]
        public async Task<IActionResult> DeleteRequest([FromBody] int id)
        {
            await _leave.DeleteRequest(id);
            return Ok("Deleted");
        }
    }
}
