namespace UC2Api.Dto;

public class FeeDetails
{
	public string Id { get; set; }
	public decimal? Amount { get; set; }
	public string Application { get; set; }
	public string Currency { get; set; }
	public string Description { get; set; }
	public string Type { get; set; }
}