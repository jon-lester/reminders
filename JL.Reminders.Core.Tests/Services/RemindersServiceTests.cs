using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JL.Reminders.Core.Entities;
using JL.Reminders.Core.Model;
using Moq;
using Xunit;

using JL.Reminders.Core.Repositories;
using JL.Reminders.Core.Services;
using JL.Reminders.Services;

namespace JL.Reminders.Tests.Services
{
	public class RemindersServiceTests
	{
		private ReminderEntity GetMockReminderEntity(long id)
		{
			return new ReminderEntity
			{
				Created = new DateTime(2018, 1, 1, 1, 1, 1),
				Recurrence = Recurrence.Monthly,
				ForDate = new DateTime(2018, 2, 2, 2, 2, 2),
				LastActioned = new DateTime(2018, 3, 3, 3, 3, 3, 3),
				Description = "Some description",
				Importance = Importance.Important,
				Title = "Some title",
				Status = ReminderStatus.Active,
				SoonDaysPreference = 321,
				ImminentDaysPreference = 123,
				Id = id
			};
		}

		private Reminder GetMockReminder(long id)
		{
			return new Reminder
			{
				Created = new DateTime(2018, 1, 1, 1, 1, 1),
				Recurrence = Recurrence.Monthly,
				ForDate = new DateTime(2018, 2, 2, 2, 2, 2),
				LastActioned = new DateTime(2018, 3, 3, 3, 3, 3),
				Description = "Some description",
				Importance = Importance.Important,
				Title = "Some title",
				Status = ReminderStatus.Active,
				SoonDaysPreference = 321,
				ImminentDaysPreference = 123,
				Id = id,
				DaysToGo = 444,
				NextDueDate = new DateTime(2018, 4, 4, 4, 4, 4),
				SubTitle = "Some subtitle"
			};
		}

		private void AssertEqualEntityAndObject(ReminderEntity theReminderEntity, Reminder theReminderObject)
		{
			Assert.Equal(theReminderEntity.Created, theReminderObject.Created);
			Assert.Equal(theReminderEntity.Recurrence, theReminderObject.Recurrence);
			Assert.Equal(theReminderEntity.ForDate, theReminderObject.ForDate);
			Assert.Equal(theReminderEntity.LastActioned, theReminderObject.LastActioned);
			Assert.Equal(theReminderEntity.Description, theReminderObject.Description);
			Assert.Equal(theReminderEntity.Importance, theReminderObject.Importance);
			Assert.Equal(theReminderEntity.Title, theReminderObject.Title);
			Assert.Equal(theReminderEntity.Status, theReminderObject.Status);
			Assert.Equal(theReminderEntity.SoonDaysPreference, theReminderObject.SoonDaysPreference);
			Assert.Equal(theReminderEntity.ImminentDaysPreference, theReminderObject.ImminentDaysPreference);
			Assert.Equal(theReminderEntity.Id, theReminderObject.Id);
		}

		[Fact]
		public async Task GetRemindersByUserIdAsync_Passes_User_ID_And_Reminder_ID_To_Repository()
		{
			Mock<IRemindersRepository> repoMock = new Mock<IRemindersRepository>();
			Mock<IReminderHydrationService> hydrationServiceMock = new Mock<IReminderHydrationService>();

			RemindersService service = new RemindersService(repoMock.Object, hydrationServiceMock.Object);

			await service.GetReminderByIdAsync("user|1234", 123);

			repoMock.Verify(m => m.GetReminderByIdAsync("user|1234", 123), Times.Once);
		}

		[Fact]
		public async Task GetRemindersByUserIdAsync_Passes_The_Reminder_From_The_Repository_To_The_Hydration_Service()
		{
			var mockReminderEntity = GetMockReminderEntity(999);

			Mock<IRemindersRepository> repoMock = new Mock<IRemindersRepository>();
			repoMock.Setup(m => m.GetReminderByIdAsync(It.IsAny<string>(), It.IsAny<long>()))
				.ReturnsAsync(mockReminderEntity);

			ReminderEntity reminderReceivedByHydrationService = null;
			Mock<IReminderHydrationService> hydrationServiceMock = new Mock<IReminderHydrationService>();
			hydrationServiceMock.Setup(m => m.HydrateReminder(It.IsAny<ReminderEntity>()))
				.Callback((ReminderEntity reminder) => reminderReceivedByHydrationService = reminder);

			RemindersService service = new RemindersService(repoMock.Object, hydrationServiceMock.Object);

			await service.GetReminderByIdAsync("1", 1);

			Assert.Equal(mockReminderEntity, reminderReceivedByHydrationService);
		}

		[Fact]
		public async Task GetRemindersByUserIdAsync_Returns_The_Reminder_Provided_By_The_Hydration_Service()
		{
			var mockReminder = GetMockReminder(999);

			Mock<IRemindersRepository> repoMock = new Mock<IRemindersRepository>();
			repoMock.Setup(m => m.GetReminderByIdAsync(It.IsAny<string>(), It.IsAny<long>()))
				.ReturnsAsync(GetMockReminderEntity(999));

			Mock<IReminderHydrationService> hydrationServiceMock = new Mock<IReminderHydrationService>();
			hydrationServiceMock.Setup(m => m.HydrateReminder(It.IsAny<ReminderEntity>()))
				.Returns(mockReminder);

			RemindersService service = new RemindersService(repoMock.Object, hydrationServiceMock.Object);

			var theReminderObject = await service.GetReminderByIdAsync("1", 1);

			Assert.Equal(mockReminder, theReminderObject);
		}
	}
}
