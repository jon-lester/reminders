using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using JL.Reminders.Api.Models;
using JL.Reminders.Core.Model;
using JL.Reminders.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace JL.Reminders.Api.Controllers
{
	[Authorize]
	[ApiController]
	[Produces("application/json")]
    [Route("api/[controller]")]
	[EnableCors("RemindersUI")]
    public class RemindersController : Controller
    {
	    private readonly IRemindersService remindersService;

	    public RemindersController(IRemindersService remindersService)
	    {
		    this.remindersService = remindersService;
	    }

	    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

		/// <summary>
		/// Fetch all reminders for the current user.
		/// </summary>
		/// <returns>An array of Reminder entities, or an empty array if none exist.</returns>
		[HttpGet]
		public async Task<IActionResult> GetAllReminders()
		{
			var reminders = await remindersService.GetRemindersByUserIdAsync(CurrentUserId, ReminderStatus.Active);

			return Ok(reminders);
		}

		/// <summary>
		/// Fetch the valid values for adding or editing
		/// </summary>
		/// <returns></returns>
	    [HttpGet]
	    [Route("options")]
	    public async Task<IActionResult> GetOptions()
	    {
		    var reminderOptions = await remindersService.GetReminderOptions();

		    return Ok(reminderOptions);
	    }

		/// <summary>
		/// Fetch a single reminder by its ID.
		/// </summary>
		/// <param name="id">The ID of the reminder to fetch.</param>
		/// <returns>The reminder identified by the ID, or 404 if not found.</returns>
	    [HttpGet]
	    [Route("{id}")]
		public async Task<IActionResult> GetReminder(long id)
	    {
			var reminder = await remindersService.GetReminderByIdAsync(CurrentUserId, id);

		    if (reminder == null)
		    {
			    return NotFound();
		    }

			return Ok(reminder);
	    }

		/// <summary>
		/// Add a new reminder for the current user.
		/// </summary>
		/// <param name="postNewReminder">The <see cref="PostNewReminderModel"/> to add as a new reminder.</param>
		/// <returns>The location of the new reminder as a Location header.</returns>
	    [HttpPost]
	    public async Task<IActionResult> PostReminder([FromBody] PostNewReminderModel postNewReminder)
	    {
			var reminderId = await remindersService.AddReminderAsync(CurrentUserId, Mapper.Map<Reminder>(postNewReminder));

		    return Created($"{Request.Path.ToString()}/{reminderId}", null);
		}

		/// <summary>
		/// Add an 'actioned' item for the current user's given reminder.
		/// </summary>
		/// <param name="postNewAction">The <see cref="PostNewActionModel"/> describing the action to post for the given reminder.</param>
		/// <returns>200 if the action was posted, or 404 if the reminder was not found.</returns>
	    [HttpPost]
	    [Route("{id}/actions")]
	    public async Task<IActionResult> PostAction([FromBody] PostNewActionModel postNewAction)
	    {
		    try
		    {
			    var success =
				    await remindersService.ActionReminderAsync(CurrentUserId, Mapper.Map<ReminderAction>(postNewAction));

			    return success ? (IActionResult)Ok() : NotFound();
			}
		    catch (Exception)
		    {
			    return StatusCode(500);
		    }
	    }

		/// <summary>
		/// Update an existing reminder.
		/// </summary>
		/// <param name="id">The ID of the reminder to update.</param>
		/// <param name="postNewReminder">A <see cref="PostNewReminderModel"/> containing changes to apply to the given reminder.</param>
		/// <returns>200 if changes were applied, otherwise 404.</returns>
	    [HttpPut]
		[Route("{id}")]
	    public async Task<IActionResult> UpdateReminder(int id, [FromBody] PostNewReminderModel postNewReminder)
	    {
		    var obj = Mapper.Map<Reminder>(postNewReminder);
		    obj.Id = id;

			var success = await remindersService.UpdateReminderAsync(CurrentUserId, obj);

		    if (!success)
		    {
			    return NotFound();
		    }

		    return Ok();
	    }

		/// <summary>
		/// Set the active/archived status of an existing reminder.
		/// </summary>
		/// <param name="id">The ID of the reminder to set status for.</param>
		/// <param name="patchReminderStatus">A <see cref="PatchReminderStatusModel"/> containing the new status.</param>
		/// <returns>200 if success, otherwise 404.</returns>
	    [HttpPatch]
	    [Route("{id}")]
	    public async Task<IActionResult> UpdateReminderStatus(int id, [FromBody] PatchReminderStatusModel patchReminderStatus)
	    {
		    var success =
			    await remindersService.SetReminderStatusAsync(CurrentUserId, id, patchReminderStatus.Status);

		    if (!success)
		    {
			    return NotFound();
		    }

		    return Ok();
	    }

		/// <summary>
		/// Delete a given reminder by ID.
		/// </summary>
		/// <param name="id">The ID of the reminder to delete.</param>
		/// <returns>200 if a reminder was deleted, otherwise 404.</returns>
	    [HttpDelete]
	    [Route("{id}")]
	    public async Task<IActionResult> DeleteReminder(long id)
	    {
		    var success = await remindersService.DeleteReminderAsync(CurrentUserId, id);

		    if (!success)
		    {
			    return NotFound();
		    }

		    return Ok();
	    }
    }
}