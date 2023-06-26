using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace UC2Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class BalanceController : ControllerBase
	{
		private readonly string? _stripeToken;

		private const int MaxLimit = 100;
		private const int DefaultItemsPerPage = 10;

		public BalanceController(IConfiguration configuration)
		{
			_stripeToken = configuration.GetSection("ApiSettings:StripeToken")?.Value;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			try
			{
				StripeConfiguration.ApiKey = _stripeToken;

				var options = new BalanceTransactionListOptions
				{
					Limit = MaxLimit
				};

				var service = new BalanceTransactionService();
				var balanceTransactions = await service.ListAsync(options);

				return Ok(balanceTransactions.Data);
			}
			catch (StripeException ex)
			{
				return StatusCode((int)ex.HttpStatusCode);
			}
			catch  
			{
				return StatusCode(500);
			}
		}

		[HttpGet]
		[Route("GetPage")]
		public async Task<IActionResult> GetPage(int? pageNo, int? itemsPerPage)
		{
			try
			{
				StripeConfiguration.ApiKey = _stripeToken;

				var options = new BalanceTransactionListOptions
				{
					Limit = MaxLimit
				};

				var service = new BalanceTransactionService();
				var balanceTransactions = await service.ListAsync(options);

				//Pagination
				var pageNoVal = pageNo ?? 0;
				var itemsPerPageVal = itemsPerPage ?? DefaultItemsPerPage;
				var skipCount = itemsPerPageVal * pageNoVal;

				var result = balanceTransactions.Data.Skip(skipCount).Take(itemsPerPageVal);

				return Ok(result);
			}
			catch (StripeException ex)
			{
				return StatusCode((int)ex.HttpStatusCode);
			}
			catch
			{
				return StatusCode(500);
			}
		}
	}
}