namespace UC2Api.Dto;

public class BalanceTransaction
{
	public string? Id { get; set; }
	public string? Object { get; set; }
	public decimal? Amount { get; set; }
	public int? Available_on { get; set; }
	public int? Created { get; set; }
	public string? Currency { get; set; }
	public string? Description { get; set; }
	public decimal? Exchange_rate { get; set; }
	public decimal? fee { get; set; }
	public List<FeeDetails>? fee_details { get; set; }
	public decimal? Net { get; set; }
	public string? reporting_category { get; set; }
	public string? source { get; set; }
	public string? status { get; set; }
	public string? type { get; set; }
}