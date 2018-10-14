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
	public class RemindersServiceTests : ServiceTestsBase
	{
		private Mock<IRemindersRepository> repositoryMock;
		private Mock<IRemindersUtilityService> utilityServiceMock;

		private RemindersService uut;
		
		public RemindersServiceTests()
		{
			this.repositoryMock = new Mock<IRemindersRepository>();
			this.utilityServiceMock = new Mock<IRemindersUtilityService>();
			this.uut = new RemindersService(this.repositoryMock.Object, this.utilityServiceMock.Object);
		}

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

		[Fact]
		public async Task GetReminderByIdAsync_Passes_User_ID_And_Reminder_ID_To_Repository()
		{
			await uut.GetReminderByIdAsync("user|1234", 123);

			repositoryMock.Verify(m => m.GetReminderByIdAsync("user|1234", 123), Times.Once);
		}

		[Fact]
		public async Task GetReminderByIdAsync_Maps_The_Reminder_Entity_To_A_Reminder_Object()
		{
			var mockReminderEntity = GetMockReminderEntity(999);

			repositoryMock
				.Setup(m => m.GetReminderByIdAsync(It.IsAny<string>(), It.IsAny<long>()))
				.ReturnsAsync(mockReminderEntity);

			var reminder = await uut.GetReminderByIdAsync("1", 1);

			Assert.Equal(mockReminderEntity.Created, reminder.Created);
			Assert.Equal(mockReminderEntity.Recurrence, reminder.Recurrence);
			Assert.Equal(mockReminderEntity.ForDate, reminder.ForDate);
			Assert.Equal(mockReminderEntity.LastActioned, reminder.LastActioned);
			Assert.Equal(mockReminderEntity.Description, reminder.Description);
			Assert.Equal(mockReminderEntity.Importance, reminder.Importance);
			Assert.Equal(mockReminderEntity.Title, reminder.Title);
			Assert.Equal(mockReminderEntity.Status, reminder.Status);
			Assert.Equal(mockReminderEntity.SoonDaysPreference, reminder.SoonDaysPreference);
			Assert.Equal(mockReminderEntity.ImminentDaysPreference, reminder.ImminentDaysPreference);
			Assert.Equal(mockReminderEntity.Id, reminder.Id);
		}

		[Fact]
		public async Task GetReminderByIdAsync_Calls_The_Utility_Service_To_Populate_The_DaysToGo_Property()
		{
			var mockReminderEntity = GetMockReminderEntity(999);

			repositoryMock
				.Setup(m => m.GetReminderByIdAsync(It.IsAny<string>(), It.IsAny<long>()))
				.ReturnsAsync(mockReminderEntity);

			utilityServiceMock
				.Setup(m => m.CalculateDaysToGo(It.IsAny<DateTime>()))
				.Returns(99)
				.Verifiable();

			var reminder = await uut.GetReminderByIdAsync("1", 1);

			utilityServiceMock.Verify();
			Assert.Equal(99, reminder.DaysToGo);
		}

		[Fact]
		public async Task GetReminderByIdAsync_Calls_The_Utility_Service_To_Populate_The_NextDueDate_Property()
		{
			DateTime nextDueDate = new DateTime(2018, 12, 25);

			repositoryMock
				.Setup(m => m.GetReminderByIdAsync(It.IsAny<string>(), It.IsAny<long>()))
				.ReturnsAsync(GetMockReminderEntity(999));

			utilityServiceMock
				.Setup(m => m.CalculateNextDueDate(It.IsAny<Recurrence>(), It.IsAny<DateTime>(), It.IsAny<DateTime?>()))
				.Returns(nextDueDate)
				.Verifiable();

			var reminder = await uut.GetReminderByIdAsync("1", 1);

			utilityServiceMock.Verify();
			Assert.Equal(nextDueDate, reminder.NextDueDate);
		}

		[Fact]
		public async Task GetReminderByIdAsync_Calls_The_Utility_Service_To_Populate_The_SubTitle_Property()
		{
			string subtitle = "This is the formatted subtitle";

			repositoryMock
				.Setup(m => m.GetReminderByIdAsync(It.IsAny<string>(), It.IsAny<long>()))
				.ReturnsAsync(GetMockReminderEntity(999));

			utilityServiceMock
				.Setup(m => m.FormatReminderSubtitle(It.IsAny<Recurrence>(), It.IsAny<DateTime>()))
				.Returns(subtitle)
				.Verifiable();

			var reminder = await uut.GetReminderByIdAsync("1", 1);

			utilityServiceMock.Verify();
			Assert.Equal(subtitle, reminder.SubTitle);
		}

		[Fact]
		public async Task GetReminderOptions_Returns_All_Recurrence_Types()
		{
			var options = await uut.GetReminderOptionsAsync();

			foreach (int r in Enum.GetValues(typeof(Recurrence)))
			{
				Assert.True(options.Recurrences.ContainsKey(r));
			}
		}

		[Fact]
		public async Task GetReminderOptions_Returns_All_Importance_Types()
		{
			var options = await uut.GetReminderOptionsAsync();

			foreach (int r in Enum.GetValues(typeof(Importance)))
			{
				Assert.True(options.Importances.ContainsKey(r));
			}
		}
	}
}
