using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using RoleChange.Services;

namespace RoleChange.Hubs
{
    public class MyHub: Hub
    {
        public JsonFileUserService UserService { get; }
        public MyHub(JsonFileUserService userService)
        {
            UserService = userService;
        }
        public async override Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            await Clients.Caller.SendAsync("UpdateUsers", await UserService.GetUsers());
        }

        public async Task EditUser(User user)
        {
            await Clients.All.SendAsync("SendToReact", "Role of user id " + user.id + " has been changed successfully");
            await Clients.All.SendAsync("UpdateUsers", await UserService.EditUser(user.id,user.role));
        }

    }
}
