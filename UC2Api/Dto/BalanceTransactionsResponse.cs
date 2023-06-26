namespace UC2Api.Dto;

public class BalanceTransactionsResponse
{
	public string? Object { get; set; }

	public List<BalanceTransaction>? Data { get; set; }
}