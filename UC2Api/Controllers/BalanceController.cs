using Microsoft.AspNetCore.Mvc;
using System.Linq;
using UC2Api.Dto;

namespace UC2Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class BalanceController : ControllerBase
	{
		private readonly HttpClient _httpClient;
		private readonly string? _apiUrl;
		private readonly string? _stripeToken;

		private const string BalanceTransactionsPath = "balance_transactions";
		private const int MaxLimit = 100;
		private const int DefaultItemsPerPage = 10;
		


		public BalanceController(HttpClient httpClient, IConfiguration configuration)
		{
			_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
			_apiUrl = configuration.GetSection("ApiSettings:StripeApiUrl")?.Value;
			_stripeToken = configuration.GetSection("ApiSettings:StripeToken")?.Value;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			try
			{
				var callUrl = $"{_apiUrl}/{BalanceTransactionsPath}";

				_httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_stripeToken}");

				var response = await _httpClient.GetAsync(callUrl);

				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadFromJsonAsync<BalanceTransactionsResponse>();

					return Ok(result?.Data);
				}
				else
				{
					return StatusCode((int)response.StatusCode);
				}
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
				var callUrl = $"{_apiUrl}/{BalanceTransactionsPath}?limit={MaxLimit}";

				_httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_stripeToken}");

				var response = await _httpClient.GetAsync(callUrl);

				if (response.IsSuccessStatusCode)
				{
					var btResponse = await response.Content.ReadFromJsonAsync<BalanceTransactionsResponse>();

					//Pagination
					var pageNoVal = pageNo ?? 0;
					var itemsPerPageVal = itemsPerPage ?? DefaultItemsPerPage;
					var skipCount = itemsPerPageVal * pageNoVal;

					var result = btResponse?.Data?.Skip(skipCount).Take(itemsPerPageVal);
					
					return Ok(result);
				}
				else
				{
					return StatusCode((int)response.StatusCode);
				}
			}
			catch
			{
				return StatusCode(500);
			}
		}
	}
}