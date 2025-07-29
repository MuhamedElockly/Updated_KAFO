using KAFO.Domain.ViewModels;
using Microsoft.AspNetCore.SignalR;
namespace KAFO.ASPMVC.Hubs
{
	public class StatisticsHub : Hub
	{
		public async Task SendStatisticsUpdate(StatisticsMessages statisticsMessages)
		{
			// Broadcast the message to all connected clients
			await Clients.All.SendAsync("ReceiveStatisticsUpdate", statisticsMessages);
		}

		public override Task OnConnectedAsync()
		{
			// Logic when a client connects, if needed
			return base.OnConnectedAsync();
		}

		public override Task OnDisconnectedAsync(Exception exception)
		{
			// Logic when a client disconnects, if needed
			return base.OnDisconnectedAsync(exception);
		}
	}
}
