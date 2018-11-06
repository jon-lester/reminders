/**
 * Represents the user's preferences for calculating
 * the urgency of reminders, ie. whether they should
 * be displayed as 'imminent' or 'soon' instead of
 * being left with no special formatting.
 */
export default interface IUrgencyConfiguration {
    soonDays: number;
    imminentDays: number;
}
