using System;
using AutoMapper;

using JL.Reminders.Api.Automapper;

namespace JL.Reminders.Tests.Api
{
	public class ControllerTestsBase : IDisposable
	{
		public ControllerTestsBase()
		{
			Mapper.Initialize(config =>
			{
				config.AddProfiles(typeof(ApiAutomapperProfile));
			});
		}

		public void Dispose()
		{
			Mapper.Reset();
		}
	}
}
