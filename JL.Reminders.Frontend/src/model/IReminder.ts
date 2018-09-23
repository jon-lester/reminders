import Urgency from './Urgency';

/**
 * Represents a reminder item pulled from the API for
 * display as a reminder card.
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
    urgency: Urgency;

    soonDaysPreference: number | null;
    imminentDaysPreference: number | null;
}