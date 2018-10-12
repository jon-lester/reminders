using System;
using AutoMapper;

using JL.Reminders.Services.Automapper;

namespace JL.Reminders.Tests.Services
{
	public class ServiceTestsBase : IDisposable
	{
		public ServiceTestsBase()
		{
			Mapper.Initialize(config =>
			{
				config.AddProfiles(typeof(ServicesAutomapperProfile));
			});
		}

		public void Dispose()
		{
			Mapper.Reset();
		}
	}
}
