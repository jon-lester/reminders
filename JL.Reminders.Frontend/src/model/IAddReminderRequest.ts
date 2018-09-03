export default interface IAddReminderRequest {
    description: string;
    forDate: Date;
    importance: number;
    recurrence: number;
    title: string;
}