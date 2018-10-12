using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

using JL.Reminders.Api.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JL.Reminders.Tests.Api
{
	static class RemindersApiControllerBaseTestExtensions
	{
		/// <summary>
		/// WARNING: do this before any other arranging (eg. model invalidation), as
		/// it makes little effort not to disturb any previously-set properties on the
		/// controller context.
		/// </summary>
		/// <param name="controller"></param>
		/// <param name="userId">The user ID to set in the controller's context.</param>
		public static void SetUserContext(this RemindersApiControllerBase controller, string userId)
		{
			controller.ControllerContext.HttpContext = new DefaultHttpContext
			{
				User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.NameIdentifier, userId)
				}))
			};
		}
	}
}
