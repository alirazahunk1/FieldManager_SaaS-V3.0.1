
using ESSDataAccess.DbContext;
using ESSDataAccess.Models;
using ESSWebApi.Dtos.Request.Client;
using ESSWebApi.Dtos.Result;
using ESSWebApi.Dtos.Result.Client;
using Microsoft.EntityFrameworkCore;

namespace ESSWebApi.Services.Client
{
    public class ClientService : IClient
    {
        private readonly AppDbContext _context;

        public ClientService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BaseResult> Create(CreateUpdateRequest Request, int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);



            ClientModel client = new ClientModel
            {
                Name = Request.Name,
                Address = Request.Address,
                PhoneNumber = Request.PhoneNumber,
                Longitude = Request.Longitude.Value,
                Latitude = Request.Latitude.Value,
                City = Request.City,
                ContactPerson = Request.ContactPerson,
                Radius = Request.Radius,
                Remarks = Request.Remarks,
                Email = Request.Email,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = userId,
                UpdatedBy = userId,
            };

            _context.Add(client);
            await _context.SaveChangesAsync();

            return new BaseResult
            {
                IsSuccess = true
            };
        }

        public async Task<BaseResult> Search(string query)
        {
            query = query.ToLower();

            var clients = await _context.Clients
                  .Where(x => x.Name.ToLower().Contains(query) ||
                  x.PhoneNumber.ToLower().Contains(query) ||
                  x.City.ToLower().Contains(query) ||
                  x.ContactPerson.ToLower().Contains(query) ||
                  x.Email.ToLower().Contains(query))
                  .OrderBy(x => x.Name)
                  .Take(10)
                  .Select(x => new ClientDto
                  {
                      Id = x.Id,
                      Name = x.Name,
                      Address = x.Address,
                      City = x.City,
                      ContactPerson = x.ContactPerson,
                      Email = x.Email,
                      Latitude = x.Latitude,
                      Longitude = x.Longitude,
                      PhoneNumber = x.PhoneNumber,
                      CreatedAt = x.CreatedAt.ToString("dd/MM/yyyy hh:mm tt"),
                      Status = x.Status.ToString()
                  }).Take(10)
                  .AsNoTracking()
                  .ToListAsync();

            return new BaseResult
            {
                IsSuccess = true,
                Data = clients
            };
        }

        public async Task<BaseResult> Delete(int clientId)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(x => x.Id == clientId);

            if (client == null) return new BaseResult { Message = "Invalid client id" };

            _context.Remove(client);
            await _context.SaveChangesAsync();
            return new BaseResult
            {
                IsSuccess = true,
            };
        }

        public async Task<BaseResult> GetAll()
        {
            var clients = await _context.Clients
                  .OrderBy(x => x.Name)
                  .Select(x => new ClientDto
                  {
                      Id = x.Id,
                      Name = x.Name,
                      Address = x.Address,
                      City = x.City,
                      ContactPerson = x.ContactPerson,
                      Email = x.Email,
                      Latitude = x.Latitude,
                      Longitude = x.Longitude,
                      PhoneNumber = x.PhoneNumber,
                      CreatedAt = x.CreatedAt.ToString("dd/MM/yyyy hh:mm tt"),
                      Status = x.Status.ToString()
                  }).Take(10)
                  .AsNoTracking()
                  .ToListAsync();

            return new BaseResult
            {
                IsSuccess = true,
                Data = clients
            };
        }

        public async Task<GetClientResult> GetAll(int skip = 0, int take = 10)
        {
            var clients = await _context.Clients
                  .Skip(skip)
                  .Take(take)
                  .OrderBy(x => x.Name)
                  .Select(x => new ClientDto
                  {
                      Id = x.Id,
                      Name = x.Name,
                      Address = x.Address,
                      City = x.City,
                      ContactPerson = x.ContactPerson,
                      Email = x.Email,
                      Latitude = x.Latitude,
                      Longitude = x.Longitude,
                      PhoneNumber = x.PhoneNumber,
                      CreatedAt = x.CreatedAt.ToString("dd/MM/yyyy hh:mm tt"),
                      Status = x.Status.ToString()
                  }).Take(10)
                  .AsNoTracking()
                  .ToListAsync();

            return new GetClientResult
            {
                IsSuccess = true,
                Clients = clients,
                TotalCount = await _context.Clients.CountAsync()
            };
        }
        public async Task<BaseResult> Update(CreateUpdateRequest request, int clientId, int userId)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(x => x.Id == clientId);

            if (client == null) return new BaseResult { Message = "Invalid client id" };

            client.Name = string.IsNullOrEmpty(request.Name) || client.Name == request.Name ? client.Name : request.Name;
            client.PhoneNumber = string.IsNullOrEmpty(request.PhoneNumber) || client.PhoneNumber == request.PhoneNumber ? client.PhoneNumber : request.PhoneNumber;
            client.Address = string.IsNullOrEmpty(request.Address) || client.Address == request.Address ? client.Address : request.Address;
            client.City = string.IsNullOrEmpty(request.City) || client.City == request.City ? client.City : request.City;
            client.Radius = request.Radius == 0 || client.Radius == request.Radius ? client.Radius : request.Radius;
            //client.Status = clientModel.Status == null || client.Status == clientModel.Status ? client.Status : clientModel.Status;
            client.ContactPerson = string.IsNullOrEmpty(request.ContactPerson) || client.ContactPerson == request.ContactPerson ? client.ContactPerson : request.ContactPerson;
            client.Email = string.IsNullOrEmpty(request.Email) || client.Email == request.Email ? client.Email : request.Email;
            /*client.Latitude = request.Latitude == null || client.Latitude == request.Latitude ? client.Latitude : request.Latitude;
            client.Longitude = request.Longitude == null || client.Longitude == request.Longitude ? client.Longitude : request.Longitude;*/
            client.UpdatedAt = DateTime.Now;

            _context.Update(client);
            await _context.SaveChangesAsync();

            return new BaseResult
            {
                IsSuccess = true,
            };
        }
    }
}
