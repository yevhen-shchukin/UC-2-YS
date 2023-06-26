using Microsoft.AspNetCore.Mvc;
using UC2Api.Dto;

namespace UC2Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class BalanceController : ControllerBase
	{
		private readonly HttpClient _httpClient;
		private readonly string? _apiUrl;
		private readonly string _balanceTransactionsPath = "balance_transactions";
		private readonly string? _stripeToken;


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
				var callUrl = $"{_apiUrl}/{_balanceTransactionsPath}";

				_httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_stripeToken}");

				var response = await _httpClient.GetAsync(callUrl);

				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadFromJsonAsync<BalanceTransactionsResponse>();

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