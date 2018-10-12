using System;

using Moq;
using Xunit;

using JL.Reminders.Core.Entities;
using JL.Reminders.Core.Model;
using JL.Reminders.Core.Services;
using JL.Reminders.Services;

namespace JL.Reminders.Tests.Services
{
	public class ReminderHydrationServiceTests : ServiceTestsBase
	{
		private ReminderHydrationService GetUut(DateTime fakeNow)
		{
			Mock<IDateTimeService> dateTimeServiceMock = new Mock<IDateTimeService>();
			dateTimeServiceMock.Setup(m => m.GetCurrentDateTime()).Returns(fakeNow);
			dateTimeServiceMock.Setup(m => m.GetCurrentDate()).Returns(fakeNow.Date);

			return new ReminderHydrationService(dateTimeServiceMock.Object);
		}

		[Fact]
		public void Next_Due_In_Future_Calculates_As_Positive_Days_To_Go()
		{
			DateTime reminderFor = new DateTime(2018, 6, 15);
			DateTime now = new DateTime(2018, 6, 1);

			ReminderEntity r = new ReminderEntity
			{
				Recurrence = Recurrence.Annual,
				ForDate = reminderFor,
				LastActioned = null
			};

			var hydrated = GetUut(now).HydrateReminder(r);

			Assert.Equal(14, hydrated.DaysToGo);
		}

		[Fact]
		public void Next_Due_In_Past_Calculates_As_Negative_Days_To_Go()
		{
			DateTime reminderFor = new DateTime(2018, 5, 1);
			DateTime now = new DateTime(2018, 6, 1);

			ReminderEntity r = new ReminderEntity
			{
				Recurrence = Recurrence.Annual,
				ForDate = reminderFor,
				LastActioned = null
			};

			var hydrated = GetUut(now).HydrateReminder(r);

			Assert.Equal(-31, hydrated.DaysToGo);
		}

		[Theory]
		[InlineData(Recurrence.Annual)]
		[InlineData(Recurrence.Monthly)]
		[InlineData(Recurrence.OneOff)]
		[InlineData(Recurrence.Quarterly)]
		[InlineData(Recurrence.SixMonthly)]
		public void Next_Due_Date_Is_Same_As_ForDate_When_Reminder_Never_Actioned(Recurrence recurrence)
		{
			DateTime forDate = new DateTime(2018, 09, 05);

			var hydrated = GetUut(new DateTime(2018, 1, 1)).HydrateReminder(new ReminderEntity
			{
				Recurrence = recurrence,
				ForDate = forDate,
				LastActioned = null
			});

			Assert.Equal(hydrated.ForDate, hydrated.NextDueDate);
		}

		[Theory]
		[InlineData(2018, 8, 1)]
		[InlineData(2018, 9, 5)]
		[InlineData(2018, 10, 1)]
		public void Next_Due_Date_Is_Same_As_ForDate_When_Recurrence_Is_OneOff_Regardless_Of_Actioned_Status(
			int lastActionedYear,
			int lastActionedMonth,
			int lastActionedDay)
		{
			DateTime forDate = new DateTime(2018, 09, 05);

			var hydrated = GetUut(new DateTime(2018, 10, 1)).HydrateReminder(new ReminderEntity
			{
				Recurrence = Recurrence.OneOff,
				ForDate = forDate,
				LastActioned = new DateTime(lastActionedYear, lastActionedMonth, lastActionedDay)
			});

			Assert.Equal(hydrated.ForDate, hydrated.NextDueDate);
		}

		[Theory]
		[InlineData(Recurrence.Annual, 2019, 9, 5)]
		[InlineData(Recurrence.Monthly, 2018, 10, 5)]
		[InlineData(Recurrence.Quarterly, 2018, 12, 5)]
		[InlineData(Recurrence.SixMonthly, 2019, 3, 5)]
		[InlineData(Recurrence.OneOff, 2018, 9, 5)]
		public void Next_Due_Date_Is_Plus_Correct_Interval_When_LastActioned_Is_Equal_To_ForDate(
			Recurrence recurrence,
			int expectedNextDueYear,
			int expectedNextDueMonth,
			int expectedNextDueDay)
		{
			DateTime forDate = new DateTime(2018, 09, 05);

			var hydrated = GetUut(new DateTime(2018, 1, 1)).HydrateReminder(new ReminderEntity
			{
				Recurrence = recurrence,
				ForDate = forDate,
				LastActioned = forDate
			});

			Assert.Equal(new DateTime(expectedNextDueYear, expectedNextDueMonth, expectedNextDueDay), hydrated.NextDueDate);
		}

		[Theory]
		[InlineData(Recurrence.Annual, 2019, 9, 5, 2020, 9, 5)]
		[InlineData(Recurrence.Monthly, 2018, 10, 5, 2018, 11, 5)]
		[InlineData(Recurrence.Quarterly, 2018, 12, 5, 2019, 3, 5)]
		[InlineData(Recurrence.SixMonthly, 2019, 3, 5, 2019, 9, 5)]
		[InlineData(Recurrence.OneOff, 2018, 9, 5, 2018, 9, 5)]
		public void Next_Due_Date_Is_Plus_Two_Intervals_When_LastActioned_Is_Equal_To_Plus_One_Interval(
			Recurrence recurrence,
			int lastActionedYear,
			int lastActionedMonth,
			int lastActionedDay,
			int expectedNextDueYear,
			int expectedNextDueMonth,
			int expectedNextDueDay)
		{
			DateTime forDate = new DateTime(2018, 09, 05);

			var hydrated = GetUut(new DateTime(2018, 1, 1)).HydrateReminder(new ReminderEntity
			{
				Recurrence = recurrence,
				ForDate = forDate,
				LastActioned = new DateTime(lastActionedYear, lastActionedMonth, lastActionedDay)
			});

			Assert.Equal(new DateTime(expectedNextDueYear, expectedNextDueMonth, expectedNextDueDay), hydrated.NextDueDate);
		}

		[Theory]
		[InlineData(2018, 10, 2, 00, 00, 2018, 10, 2, 00, 00)]
		[InlineData(2018, 10, 2, 23, 59, 2018, 10, 2, 00, 00)]
		[InlineData(2018, 10, 2, 00, 00, 2018, 10, 2, 23, 59)]
		[InlineData(2018, 10, 2, 23, 59, 2018, 10, 2, 23, 59)]
		public void Days_To_Go_Is_Zero_When_Next_Due_Is_Today_Regardless_Of_Time_Of_Day(
			int todayYear, int todayMonth, int todayDay, int todayHour, int todayMin,
			int forYear, int forMonth, int forDay, int forHour, int forMin)
		{
			DateTime forDate = new DateTime(forYear, forMonth, forDay, forHour, forMin, 0);
			DateTime today = new DateTime(todayYear, todayMonth, todayDay, todayHour, todayMin, 0);

			foreach (Recurrence recurrence in Enum.GetValues(typeof(Recurrence)))
			{
				var hydrated = GetUut(today).HydrateReminder(new ReminderEntity
				{
					Recurrence = recurrence,
					ForDate = forDate,
					LastActioned = null
				});

				Assert.Equal(0, hydrated.DaysToGo);
			}
		}

		[Theory]
		[InlineData(2018, 10, 2, 00, 00, 2018, 10, 3, 00, 00)]
		[InlineData(2018, 10, 2, 23, 59, 2018, 10, 3, 00, 00)]
		[InlineData(2018, 10, 2, 00, 00, 2018, 10, 3, 23, 59)]
		[InlineData(2018, 10, 2, 23, 59, 2018, 10, 3, 23, 59)]
		public void Days_To_Go_Is_One_When_Next_Due_Is_Tomorrow_Regardless_Of_Time_Of_Day(
			int todayYear, int todayMonth, int todayDay, int todayHour, int todayMin,
			int forYear, int forMonth, int forDay, int forHour, int forMin)
		{
			DateTime forDate = new DateTime(forYear, forMonth, forDay, forHour, forMin, 0);
			DateTime today = new DateTime(todayYear, todayMonth, todayDay, todayHour, todayMin, 0);

			foreach (Recurrence recurrence in Enum.GetValues(typeof(Recurrence)))
			{
				var hydrated = GetUut(today).HydrateReminder(new ReminderEntity
				{
					Recurrence = recurrence,
					ForDate = forDate,
					LastActioned = null
				});

				Assert.Equal(1, hydrated.DaysToGo);
			}
		}

		[Theory]
		[InlineData(2018, 10, 2, 00, 00, 2018, 10, 1, 00, 00)]
		[InlineData(2018, 10, 2, 23, 59, 2018, 10, 1, 00, 00)]
		[InlineData(2018, 10, 2, 00, 00, 2018, 10, 1, 23, 59)]
		[InlineData(2018, 10, 2, 23, 59, 2018, 10, 1, 23, 59)]
		public void Days_To_Go_Is_Minus_One_When_Next_Due_Is_Yesterday_Regardless_Of_Time_Of_Day(
			int todayYear, int todayMonth, int todayDay, int todayHour, int todayMin,
			int forYear, int forMonth, int forDay, int forHour, int forMin)
		{
			DateTime forDate = new DateTime(forYear, forMonth, forDay, forHour, forMin, 0);
			DateTime today = new DateTime(todayYear, todayMonth, todayDay, todayHour, todayMin, 0);

			foreach (Recurrence recurrence in Enum.GetValues(typeof(Recurrence)))
			{
				var hydrated = GetUut(today).HydrateReminder(new ReminderEntity
				{
					Recurrence = recurrence,
					ForDate = forDate,
					LastActioned = null
				});

				Assert.Equal(-1, hydrated.DaysToGo);
			}
		}
	}
}
