using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Moq;
using Xunit;

using JL.Reminders.Core.Entities;
using JL.Reminders.Core.Model;
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

		[Fact]
		public async Task GetRemindersByUserIdAsync_Passes_User_ID_And_Status_To_Repository()
		{
			var userId = "user|4321";
			var status = ReminderStatus.Archived;

			await uut.GetRemindersByUserIdAsync(userId, status);

			repositoryMock.Verify(m => m.GetRemindersByUserIdAsync(userId, status));
		}

		[Fact]
		public async Task GetRemindersByUserIdAsync_Maps_And_Returns_All_Reminders_From_Repository()
		{
			List<ReminderEntity> mockReminders = new List<ReminderEntity>
			{
				GetMockReminderEntity(1),
				GetMockReminderEntity(2),
				GetMockReminderEntity(3)
			};

			repositoryMock.Setup(m => m.GetRemindersByUserIdAsync(It.IsAny<string>(), It.IsAny<ReminderStatus>()))
				.ReturnsAsync(mockReminders);

			var reminders = (await uut.GetRemindersByUserIdAsync("userid", ReminderStatus.Active)).ToList();

			foreach (var mockReminder in mockReminders)
			{
				var resultingReminder = reminders.FirstOrDefault(r => r.Id == mockReminder.Id);

				Assert.NotNull(resultingReminder);
				Assert.True(AreEquivalentReminderObjects(mockReminder, resultingReminder));
			}
		}

		[Fact]
		public async Task GetReminderByIdAsync_Passes_User_ID_And_Reminder_ID_To_Repository()
		{
			await uut.GetReminderByIdAsync("user|1234", 123);

			repositoryMock.Verify(m => m.GetReminderByIdAsync("user|1234", 123), Times.Once);
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
		public async Task GetReminderByIdAsync_Returns_A_Reminder_Mapped_From_The_Reminder_Entity()
		{
			var mockReminderEntity = GetMockReminderEntity(999);

			repositoryMock
				.Setup(m => m.GetReminderByIdAsync(It.IsAny<string>(), It.IsAny<long>()))
				.ReturnsAsync(mockReminderEntity);

			var reminder = await uut.GetReminderByIdAsync("1", 1);

			Assert.True(AreEquivalentReminderObjects(mockReminderEntity, reminder));
		}

		[Fact]
		public async Task GetReminderOptionsAsync_Returns_All_Recurrence_Types()
		{
			var options = await uut.GetReminderOptionsAsync();

			foreach (int r in Enum.GetValues(typeof(Recurrence)))
			{
				Assert.True(options.Recurrences.ContainsKey(r));
			}
		}

		[Fact]
		public async Task GetReminderOptionsAsync_Returns_All_Importance_Types()
		{
			var options = await uut.GetReminderOptionsAsync();

			foreach (int r in Enum.GetValues(typeof(Importance)))
			{
				Assert.True(options.Importances.ContainsKey(r));
			}
		}

		[Fact]
		public async Task AddReminderAsync_Saves_The_Reminder_For_The_Correct_User_Id()
		{
			var userId = "user|6789";

			var result = await uut.AddReminderAsync(userId, GetMockReminder(111));

			repositoryMock.Verify(m => m.AddReminderAsync(userId, It.IsAny<ReminderEntity>()));
		}

		[Fact]
		public async Task AddReminderAsync_Maps_To_An_Equivalent_Entity_And_Sends_It_To_The_Repository()
		{
			var mockReminder = GetMockReminder(456);
			ReminderEntity mappedReminder = null;

			repositoryMock.Setup(m => m.AddReminderAsync(It.IsAny<string>(), It.IsAny<ReminderEntity>()))
				.ReturnsAsync(123)
				.Callback((string uid, ReminderEntity re) => mappedReminder = re);

			var result = await uut.AddReminderAsync("1", mockReminder);

			Assert.True(AreEquivalentReminderObjects(mappedReminder, mockReminder));
		}

		[Fact]
		public async Task AddReminderAsync_Returns_The_Reminder_Id_From_The_Repository()
		{
			repositoryMock.Setup(m => m.AddReminderAsync(It.IsAny<string>(), It.IsAny<ReminderEntity>()))
				.ReturnsAsync(123);

			var result = await uut.AddReminderAsync("1", GetMockReminder(111));

			Assert.Equal(123, result);
		}

		[Fact]
		public async Task DeleteReminderAsync_Calls_The_Repository_With_The_Correct_User_And_Reminder_Ids()
		{
			string userId = "user|999";
			long reminderId = 321;

			await uut.DeleteReminderAsync(userId, reminderId);

			repositoryMock.Verify(m => m.DeleteReminderAsync(userId, reminderId));
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public async Task DeleteReminderAsync_Returns_The_Result_From_The_Repository(bool repositoryResult)
		{
			repositoryMock.Setup(m => m.DeleteReminderAsync(It.IsAny<string>(), It.IsAny<long>()))
				.ReturnsAsync(repositoryResult);

			var result = await uut.DeleteReminderAsync("1", 1);

			Assert.Equal(repositoryResult, result);
		}

		[Fact]
		public async Task UpdateReminderAsync_Updates_The_Reminder_For_The_Correct_User_Id()
		{
			var userId = "user|6789";

			var result = await uut.UpdateReminderAsync(userId, GetMockReminder(111));

			repositoryMock.Verify(m => m.UpdateReminderAsync(userId, It.IsAny<ReminderEntity>()));
		}

		[Fact]
		public async Task UpdateReminderAsync_Maps_To_An_Equivalent_Entity_And_Sends_It_To_The_Repository()
		{
			var mockReminder = GetMockReminder(456);
			ReminderEntity mappedReminder = null;

			repositoryMock.Setup(m => m.UpdateReminderAsync(It.IsAny<string>(), It.IsAny<ReminderEntity>()))
				.ReturnsAsync(true)
				.Callback((string uid, ReminderEntity re) => mappedReminder = re);

			var result = await uut.UpdateReminderAsync("1", mockReminder);

			Assert.True(AreEquivalentReminderObjects(mappedReminder, mockReminder));
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public async Task UpdateReminderAsync_Returns_The_Result_From_The_Repository(bool repositoryResult)
		{
			repositoryMock.Setup(m => m.UpdateReminderAsync(It.IsAny<string>(), It.IsAny<ReminderEntity>()))
				.ReturnsAsync(repositoryResult);

			var result = await uut.UpdateReminderAsync("1", GetMockReminder(1));

			Assert.Equal(repositoryResult, result);
		}



		private ReminderEntity GetMockReminderEntity(long id)
		{
			return new ReminderEntity
			{
				Created = new DateTime(2018, 1, 1, 1, 1, 1),
				Recurrence = Recurrence.Monthly,
				ForDate = new DateTime(2018, 2, 2, 2, 2, 2),
				LastActioned = new DateTime(2018, 3, 3, 3, 3, 3, 3),
				Description = "Some description " + id,
				Importance = Importance.Important,
				Title = "Some title " + id,
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
				LastActioned = new DateTime(2018, 3, 3, 3, 3, 3, 3),
				Description = "Some description " + id,
				Importance = Importance.Important,
				Title = "Some title " + id,
				Status = ReminderStatus.Active,
				SoonDaysPreference = 321,
				ImminentDaysPreference = 123,
				Id = id
			};
		}

		private bool AreEquivalentReminderObjects(ReminderEntity mockReminderEntity, Reminder reminder)
		{
			return
				mockReminderEntity != null &&
				reminder != null &&
				mockReminderEntity.Created == reminder.Created &&
				mockReminderEntity.Recurrence == reminder.Recurrence &&
				mockReminderEntity.ForDate == reminder.ForDate &&
				mockReminderEntity.LastActioned == reminder.LastActioned &&
				mockReminderEntity.Description == reminder.Description &&
				mockReminderEntity.Importance == reminder.Importance &&
				mockReminderEntity.Title == reminder.Title &&
				mockReminderEntity.Status == reminder.Status &&
				mockReminderEntity.SoonDaysPreference == reminder.SoonDaysPreference &&
				mockReminderEntity.ImminentDaysPreference == reminder.ImminentDaysPreference &&
				mockReminderEntity.Id == reminder.Id;
		}
	}
}
