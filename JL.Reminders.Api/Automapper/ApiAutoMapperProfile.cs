using System;

using AutoMapper;

using JL.Reminders.Api.Models;
using JL.Reminders.Core.Model;

namespace JL.Reminders.Api.Automapper
{
	public class ApiAutomapperProfile : Profile
	{
		public ApiAutomapperProfile()
		{
			CreateMap<PostNewReminderModel, Reminder>();

			CreateMap<PostNewActionModel, ReminderAction>()
				.ForMember(m => m.ReminderId, opt => opt.MapFrom(vm => vm.ReminderId))
				.ForMember(m => m.Notes, opt => opt.MapFrom(vm => vm.Notes));
		}
	}
}
