/**
 * Used by the ReminderAppMenu component, and defines a
 * single menu item, with an arbitrary id, the text for
 * display, and an action to carry out when clicked.
 */
export default interface IMenuItem {
    id: number;
    text: string;
    action: () => void;
}