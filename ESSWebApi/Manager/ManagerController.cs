using CZ.Api.Base;
using ESSWebApi.Manager.ManagerDtos.Requests.Expense;
using ESSWebApi.Manager.ManagerDtos.Requests.Leave;
using ESSWebApi.Services.Manager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESSWebApi.Manager
{
    [Authorize, ApiController]
    public class ManagerController : BaseController
    {
        private readonly IManager _managerRepo;

        public ManagerController(IManager managerRepo)
        {
            _managerRepo = managerRepo;
        }

        [HttpGet(ManagerRoutes.GetEmployeesStatus)]
        public async Task<IActionResult> GetEmployeeStatus()
        {
            var result = await _managerRepo.GetEmployeeStatus(GetUserId());
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }

        [HttpGet(ManagerRoutes.GetDashboardData)]
        public async Task<IActionResult> GetDashboardData()
        {
            var result = await _managerRepo.GetDashboardData(GetUserId());
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }

        [HttpGet(ManagerRoutes.GetAllLeaveRequests)]
        public async Task<IActionResult> GetAllLeaveRequests()
        {
            var result = await _managerRepo.GetAllLeaveRequests(GetUserId());
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok(result.Data);

        }

        [HttpPost(ManagerRoutes.ChangeLeaveStatus)]
        public async Task<IActionResult> ChangeLeaveStatus([FromBody] ChangeLeaveStatus request)
        {
            var result = await _managerRepo.ChangeLeaveRequestStatus(request, GetUserId());
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpGet(ManagerRoutes.GetAllExpenseRequests)]
        public async Task<IActionResult> GetAllExpenseRequests()
        {
            var result = await _managerRepo.GetAllExpenseRequests(GetUserId());
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok(result.Data);

        }

        [HttpPost(ManagerRoutes.ChangeExpenseStatus)]
        public async Task<IActionResult> ChangeExpenseStatus([FromBody] ChangeExpenseStatus request)
        {
            var result = await _managerRepo.ChangeExpenseRequestStatus(request, GetUserId());
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }


    }

}
