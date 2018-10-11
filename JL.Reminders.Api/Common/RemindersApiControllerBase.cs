using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace JL.Reminders.Api.Common
{
	public class RemindersApiControllerBase : ControllerBase
	{
		protected string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier);
	}
}
