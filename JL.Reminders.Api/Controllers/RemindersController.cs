using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using AutoMapper;

using JL.Reminders.Api.Models;
using JL.Reminders.Core.Model;
using JL.Reminders.Core.Repositories;

namespace JL.Reminders.Api.Controllers
{
	[ApiController]
	[Produces("application/json")]
    [Route("api/[controller]")]
    public class RemindersController : Controller
    {
	    private readonly IRemindersRepository remindersRepository;

	    public RemindersController(IRemindersRepository remindersRepository)
	    {
		    this.remindersRepository = remindersRepository;
	    }

		/// <summary>
		/// Fetch all reminders for the current user.
		/// </summary>
		/// <returns>An array of Reminder entities, or an empty array if none exist.</returns>
		[HttpGet]
		public async Task<IActionResult> GetAllReminders()
		{
			var reminders = await this.remindersRepository.GetRemindersByUserIdAsync(1);

			return Ok(reminders);
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
			var reminder = await this.remindersRepository.GetReminderByIdAsync(1, id);

		    if (reminder == null)
		    {
			    return NotFound();
		    }

			return Ok(reminder);
	    }

		/// <summary>
		/// Add a new reminder for the current user.
		/// </summary>
		/// <param name="reminder">The <see cref="ReminderDetailsModel"/> to add as a new reminder.</param>
		/// <returns>The location of the new reminder as a Location header.</returns>
	    [HttpPost]
	    public async Task<IActionResult> PostReminder([FromBody] ReminderDetailsModel reminder)
	    {
			var reminderId = await this.remindersRepository.AddReminderAsync(1, Mapper.Map<Reminder>(reminder));

		    return Created($"{Request.Path.ToString()}/{reminderId}", null);
		}

		/// <summary>
		/// Update an existing reminder.
		/// </summary>
		/// <param name="id">The ID of the reminder to update.</param>
		/// <param name="reminder">A <see cref="ReminderDetailsModel"/> containing changes to apply to the given reminder.</param>
		/// <returns>200 if changes were applied, otherwise 404.</returns>
	    [HttpPut]
		[Route("{id}")]
	    public async Task<IActionResult> UpdateReminder(int id, [FromBody] ReminderDetailsModel reminder)
	    {
		    var obj = Mapper.Map<Reminder>(reminder);
		    obj.ID = id;

			var success = await this.remindersRepository.UpdateReminderAsync(1, obj);

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
	    [Route("id")]
	    public async Task<IActionResult> DeleteReminder(long id)
	    {
		    var success = await this.remindersRepository.DeleteReminderAsync(1, id);

		    if (!success)
		    {
			    return NotFound();
		    }

		    return Ok();
	    }
    }
}