export default interface IMenuItem {
    id: number;
    text: string;
    action: () => void;
}