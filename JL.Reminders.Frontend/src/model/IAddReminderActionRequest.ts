/**
 * Represents a request to action a reminder.
 */
export default interface IAddReminderActionRequest {
    reminderId: number;
    notes: string;
}