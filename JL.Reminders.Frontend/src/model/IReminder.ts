/**
 * Represents a reminder item pulled from the API for
 * display as a reminder card.
 */
export default interface IReminder {
    id: number;
    title: string;
    daysToGo: number;
    description: string;
    forDate: Date;
    created: Date;
    recurrence: number;
    importance: number;
    lastActioned: Date | null;
}