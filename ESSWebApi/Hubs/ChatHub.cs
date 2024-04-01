using ESSDataAccess.DbContext;
using Microsoft.AspNetCore.SignalR;

namespace ESSWebApi.Hubs
{
    public class ChatHub : Hub
    {
        private readonly AppDbContext _context;

        public ChatHub(AppDbContext context)
        {
            _context = context;
        }

      /*  public override async Task OnConnectedAsync()
        {
            await SendMessage("", "User connected!");

            await base.OnConnectedAsync(); 
        }
*/
        public async Task Send(string name, string message)
        {

            await Clients.All.SendAsync("OnMessage", name, message+" Success");
        }

      /*  public async Task Receive()
        {

            await Clients.All.SendAsync("")
        }*/
    }
}
