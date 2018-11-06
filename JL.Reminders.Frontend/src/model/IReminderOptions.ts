/**
 * Represents static data provided by the API
 * for populating drop-down option menus.
 */
export default interface IReminderOptions {
    recurrences: { [key: number]: string };
    importances: { [key: number]: string };
}