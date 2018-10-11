using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

using JL.Reminders.Api.Common;
using JL.Reminders.Api.Models;
using JL.Reminders.Core.Model;
using JL.Reminders.Core.Services.Interfaces;

namespace JL.Reminders.Api.Controllers
{
	[Authorize]
	[ApiController]
	[Produces("application/json")]
	[Route("api/[controller]")]
	[EnableCors("RemindersUI")]
	public class PreferencesController : RemindersApiControllerBase
	{
		private readonly IUserPreferencesService userPreferencesService;

	    public PreferencesController(IUserPreferencesService userPreferencesService)
	    {
		    this.userPreferencesService = userPreferencesService;
	    }

		[HttpGet]
	    public async Task<IActionResult> GetUserPreferences()
		{
			var preferences = await this.userPreferencesService.GetUserPreferencesAsync(CurrentUserId);

			return Ok(preferences);
		}

		public async Task<IActionResult> SetUserPreferences([FromBody] PatchUserPreferencesModel userPreferences)
		{
			await this.userPreferencesService.SetUserPreferencesAsync(CurrentUserId, new UserPreferences
			{
				UrgencyConfiguration = new UrgencyConfiguration
				{
					ImminentDays = userPreferences.ImminentDays,
					SoonDays = userPreferences.SoonDays
				}
			});

			return Ok();
		}
    }
}