using System;
using System.Linq;
using Xunit;

using JL.Reminders.Core.Model;
using JL.Reminders.Core.Services;
using Moq;

namespace JL.Reminders.Core.Tests
{
	public class ReminderCalculationServiceTests
	{
		private ReminderCalculationService GetUut(DateTime fakeNow)
		{
			Mock<IDateTimeService> dateTimeServiceMock = new Mock<IDateTimeService>();
			dateTimeServiceMock.Setup(m => m.GetCurrentDateTime()).Returns(fakeNow);

			return new ReminderCalculationService(dateTimeServiceMock.Object);
		}

		[Fact]
		public void Next_Due_In_Future_Calculates_As_Positive_Days_To_Go()
		{
			DateTime reminderFor = new DateTime(2018, 6, 15);
			DateTime now = new DateTime(2018, 6, 1);

			Reminder r = new Reminder
			{
				Recurrence = Recurrence.Annual,
				ForDate = reminderFor,
				LastActioned = null
			};

			int dtg = GetUut(now).CalculateDaysToGo(r);

			Assert.Equal(14, dtg);
		}

		[Fact]
		public void Next_Due_In_Past_Calculates_As_Negative_Days_To_Go()
		{
			DateTime reminderFor = new DateTime(2018, 5, 1);
			DateTime now = new DateTime(2018, 6, 1);

			Reminder r = new Reminder
			{
				Recurrence = Recurrence.Annual,
				ForDate = reminderFor,
				LastActioned = null
			};

			int dtg = GetUut(now).CalculateDaysToGo(r);

			Assert.Equal(-31, dtg);
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

			var nextDue = GetUut(new DateTime(2018, 1, 1)).CalculateNextDueDate(new Reminder
			{
				Recurrence = recurrence,
				ForDate = forDate,
				LastActioned = null
			});

			Assert.Equal(forDate, nextDue);
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

			var nextDue = GetUut(new DateTime(2018, 10, 1)).CalculateNextDueDate(new Reminder
			{
				Recurrence = Recurrence.OneOff,
				ForDate = forDate,
				LastActioned = new DateTime(lastActionedYear, lastActionedMonth, lastActionedDay)
			});

			Assert.Equal(forDate, nextDue);
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

			var nextDue = GetUut(new DateTime(2018, 1, 1)).CalculateNextDueDate(new Reminder
			{
				Recurrence = recurrence,
				ForDate = forDate,
				LastActioned = forDate
			});

			Assert.Equal(new DateTime(expectedNextDueYear, expectedNextDueMonth, expectedNextDueDay), nextDue);
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

			var nextDue = GetUut(new DateTime(2018, 1, 1)).CalculateNextDueDate(new Reminder
			{
				Recurrence = recurrence,
				ForDate = forDate,
				LastActioned = new DateTime(lastActionedYear, lastActionedMonth, lastActionedDay)
			});

			Assert.Equal(new DateTime(expectedNextDueYear, expectedNextDueMonth, expectedNextDueDay), nextDue);
		}
	}
}
