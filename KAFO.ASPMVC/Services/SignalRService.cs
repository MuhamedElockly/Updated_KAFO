using KAFO.Domain.ViewModels;
using KAFO.Domain.Services;
using Microsoft.AspNetCore.SignalR;
using KAFO.ASPMVC.Hubs;
using KAFO.BLL.Managers;

namespace KAFO.ASPMVC.Services
{
	public class SignalRService : IStatisticsNotificationService
	{
		private readonly IHubContext<StatisticsHub> _hubContext;
		private readonly ReportManager _reportManager;

		public SignalRService(IHubContext<StatisticsHub> hubContext, ReportManager reportManager)
		{
			_hubContext = hubContext;
			_reportManager = reportManager;
		}

		public async Task SendStatisticsUpdateAsync()
		{
			try
			{

				var statisticsMessages = new StatisticsMessages
				{
					TotalProfitToday = _reportManager.GetTodayTotalProfit().ToString("N2"),
					TotalSellsToday = _reportManager.GetTotalSellsToday().ToString("N2"),
					TotalProductSoldToday = _reportManager.GetTotalProductSoldToday().ToString()
				};

				await _hubContext.Clients.All.SendAsync("ReceiveStatisticsUpdate", statisticsMessages);
			}
			catch (Exception ex)
			{

			}
		}
	}
}