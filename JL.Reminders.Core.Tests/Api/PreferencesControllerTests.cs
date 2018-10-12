using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;

using JL.Reminders.Api.Controllers;
using JL.Reminders.Api.Models;
using JL.Reminders.Core.Model;
using JL.Reminders.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace JL.Reminders.Tests.Api
{
	public class PreferencesControllerTests : ControllerTestsBase
	{
		[Fact]
		public async Task GetUserPreferences_Calls_The_Preferences_Service_With_The_Current_User_ID()
		{
			Mock<IUserPreferencesService> prefsServiceMock = new Mock<IUserPreferencesService>();

			prefsServiceMock
				.Setup(m => m.GetUserPreferencesAsync(It.IsAny<string>()))
				.ReturnsAsync(new UserPreferences());

			PreferencesController uut = new PreferencesController(prefsServiceMock.Object);

			string userId = "someUserId|1234";
			uut.SetUserContext(userId);

			await uut.GetUserPreferences();

			prefsServiceMock.Verify(m => m.GetUserPreferencesAsync(userId));
		}

		[Fact]
		public async Task GetUserPreferences_Returns_The_User_Preferences_From_The_Preferences_Service()
		{
			Mock<IUserPreferencesService> prefsServiceMock = new Mock<IUserPreferencesService>();

			var expectedUserPreferences = new UserPreferences
			{
				UrgencyConfiguration = new UrgencyConfiguration
				{
					ImminentDays = 123,
					SoonDays = 321
				}
			};

			prefsServiceMock
				.Setup(m => m.GetUserPreferencesAsync(It.IsAny<string>()))
				.ReturnsAsync(expectedUserPreferences);

			PreferencesController uut = new PreferencesController(prefsServiceMock.Object);

			string userId = "someUserId|1234";
			uut.SetUserContext(userId);

			var result = await uut.GetUserPreferences();

			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Equal(expectedUserPreferences, okResult.Value);
		}

		[Fact]
		public async Task PatchUserPreferences_Does_Not_Call_Preferences_Service_When_Model_Not_Valid()
		{
			Mock<IUserPreferencesService> prefsServiceMock = new Mock<IUserPreferencesService>();

			PreferencesController uut = new PreferencesController(prefsServiceMock.Object);

			uut.ModelState.AddModelError("error", "error");

			await uut.PatchUserPreferences(new PatchUserPreferencesModel());

			prefsServiceMock.Verify(m => m.SetUserPreferencesAsync(It.IsAny<string>(), It.IsAny<UserPreferences>()), Times.Never);
		}

		[Fact]
		public async Task PatchUserPreferences_Calls_The_Preferences_Service_With_The_Current_User_ID()
		{
			string userId = "someuser|54321";

			Mock<IUserPreferencesService> prefsServiceMock = new Mock<IUserPreferencesService>();

			prefsServiceMock.Setup(m => m.SetUserPreferencesAsync(It.IsAny<string>(), It.IsAny<UserPreferences>()))
				.ReturnsAsync(true);

			PreferencesController uut = new PreferencesController(prefsServiceMock.Object);

			uut.SetUserContext(userId);

			await uut.PatchUserPreferences(new PatchUserPreferencesModel());

			prefsServiceMock.Verify(m => m.SetUserPreferencesAsync(userId, It.IsAny<UserPreferences>()), Times.Once);
		}

		[Fact]
		public async Task PatchUserPreferences_Calls_The_Preferences_Service_With_The_Expected_Preference_Values()
		{
			Mock<IUserPreferencesService> prefsServiceMock = new Mock<IUserPreferencesService>();

			UserPreferences preferencesPassedToService = null;

			prefsServiceMock.Setup(m => m.SetUserPreferencesAsync(It.IsAny<string>(), It.IsAny<UserPreferences>()))
				.ReturnsAsync(true)
				.Callback((string uid, UserPreferences prefs) => { preferencesPassedToService = prefs; });

			PreferencesController uut = new PreferencesController(prefsServiceMock.Object);

			uut.SetUserContext("userid");

			await uut.PatchUserPreferences(new PatchUserPreferencesModel
			{
				ImminentDays = 333,
				SoonDays = 444
			});

			prefsServiceMock.Verify(m => m.SetUserPreferencesAsync(It.IsAny<string>(), It.IsAny<UserPreferences>()), Times.Once);
			Assert.Equal(333, preferencesPassedToService.UrgencyConfiguration.ImminentDays);
			Assert.Equal(444, preferencesPassedToService.UrgencyConfiguration.SoonDays);
		}
	}
}
