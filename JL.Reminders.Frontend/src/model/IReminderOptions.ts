export default interface IReminderOptions {
    recurrences: { [key: number]: string };
    importances: { [key: number]: string };
}