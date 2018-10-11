using Microsoft.Extensions.DependencyInjection;

using JL.Reminders.Core.Repositories;
using JL.Reminders.Core.Services;
using JL.Reminders.Core.Services.Interfaces;
using JL.Reminders.Data;

namespace JL.Reminders.Api
{
	public static class RemindersDependencyInjectionExtension
	{
		public static void AddRemindersApp(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddSingleton<IDateTimeService, DateTimeService>();
			serviceCollection.AddSingleton<IConnectionStringFactory, ConnectionStringFactory>();
			serviceCollection.AddSingleton<IRemindersRepository, RemindersRepository>();
			serviceCollection.AddSingleton<IReminderHydrationService, ReminderHydrationService>();
			serviceCollection.AddSingleton<IRemindersService, RemindersService>();
			serviceCollection.AddSingleton<IUserPreferencesRepository, UserPreferencesRepository>();
			serviceCollection.AddSingleton<IUserPreferencesService, UserPreferencesService>();
		}
	}
}