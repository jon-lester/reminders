/**
 * Represents a request to add a new reminder card,
 * containing only the items which the user can set.
 */
export default interface IAddReminderRequest {
    description: string;
    forDate: Date;
    importance: number;
    recurrence: number;
    title: string;
}