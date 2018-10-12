using System;

using AutoMapper;

using JL.Reminders.Core.Entities;
using JL.Reminders.Core.Model;

namespace JL.Reminders.Services.Automapper
{
	public class ServicesAutomapperProfile : Profile
	{
		public ServicesAutomapperProfile()
		{
			CreateMap<ReminderEntity, Reminder>(MemberList.Source);

			CreateMap<Reminder, ReminderEntity>(MemberList.Destination);
		}
	}
}
