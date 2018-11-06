/**
 * Used by the FloatingMenu component. Defines a
 * single menu item, with some descriptive text for
 * display, and an action to carry out when clicked.
 */
export default interface IMenuItem {
    text: string;
    action: () => void;
}