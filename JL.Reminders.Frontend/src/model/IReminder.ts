/**
 * Represents a reminder item pulled from the API.
 */
export default interface IReminder {
    id: number;
    title: string;
    subTitle: string;
    description: string;

    forDate: Date;
    created: Date;
    lastActioned: Date | null;
    nextDueDate: Date;
    daysToGo: number;

    recurrence: number;
    importance: number;

    soonDaysPreference: number | null;
    imminentDaysPreference: number | null;
}