using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using Stripe;
using UC2Api.Controllers;

namespace UC2Api.UnitTests;

public class BalanceControllerTests
{
	private BalanceController _controller;
	private Mock<IConfiguration> _configuration;

	private const string StripeTokenConfigPath = "ApiSettings:StripeToken";
	private const string StripeTokenValue = "sk_test_4eC39HqLyjWDarjtT1zdp7dc";
	private const int PageNo = 2;
	private const int ItemsOnPage = 3;

	[SetUp]
	public void Setup()
	{
		_configuration = new Mock<IConfiguration>();

		_configuration.Setup(q => q.GetSection(StripeTokenConfigPath).Value).Returns(StripeTokenValue);

		_controller = new BalanceController(_configuration.Object);
	}

	[Test]
	public void BalanceControllerTests_Controller()
	{
		_configuration.Verify(q => q.GetSection(StripeTokenConfigPath).Value, Times.Once);
	}

	[Test]
	public async Task BalanceControllerTests_Get_HappyPath()
	{
		var response = await _controller.Get();
		response.Should().NotBeNull();

		var result = (OkObjectResult)response;
		result.StatusCode.Should().Be(200);

		var resultValue = (List<BalanceTransaction>)result.Value!;
		resultValue.Should().NotBeNull();
		resultValue.Count.Should().Be(100);
	}

	[Test]
	public async Task BalanceControllerTests_Get_StripeApiIssue()
	{
		_configuration.Setup(q => q.GetSection(StripeTokenConfigPath).Value).Returns("WrongStripeTokenValue");
		_controller = new BalanceController(_configuration.Object);

		var response = await _controller.Get();
		response.Should().NotBeNull();

		var result = (StatusCodeResult)response;
		result.StatusCode.Should().Be(401);
	}

	[Test]
	public async Task BalanceControllerTests_GetPage_HappyPath()
	{
		var response = await _controller.GetPage(PageNo, ItemsOnPage);
		response.Should().NotBeNull();

		var result = (OkObjectResult)response;
		result.StatusCode.Should().Be(200);

		var resultValue = (List<BalanceTransaction>)result.Value!;
		resultValue.Should().NotBeNull();
		resultValue.Count.Should().Be(ItemsOnPage);


		var getResult = (OkObjectResult)await _controller.Get();
		var getValue = (List<BalanceTransaction>)getResult.Value!;
		var totalIndex = PageNo * ItemsOnPage;

		for (int i = 0; i < ItemsOnPage; i++)
		{
			resultValue[i].Id.Should().Be(getValue[totalIndex + i].Id);
		}
	}


	[Test]
	public async Task BalanceControllerTests_GetPage_StripeApiIssue()
	{
		_configuration.Setup(q => q.GetSection(StripeTokenConfigPath).Value).Returns("WrongStripeTokenValue");
		_controller = new BalanceController(_configuration.Object);

		var response = await _controller.GetPage(PageNo, ItemsOnPage);
		response.Should().NotBeNull();

		var result = (StatusCodeResult)response;
		result.StatusCode.Should().Be(401);
	}
}