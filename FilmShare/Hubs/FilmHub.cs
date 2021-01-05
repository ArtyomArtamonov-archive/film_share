using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace FilmShare.Hubs{
    public class FilmHub : Hub{
        public async Task SendTiming(string timing){
            await Clients.All.SendAsync("ReceiveFilmTiming", timing);
        }

        public async Task SendStart(){
            await Clients.All.SendAsync("ReceiveStart");
        }
        
        public async Task SendPause(){
            await Clients.All.SendAsync("ReceivePause");
        }
    }
}