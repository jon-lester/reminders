import * as React from 'react';

interface ICanvasComponentProps {
    width: number,
    height: number
}

class CanvasComponent extends React.Component<ICanvasComponentProps> {

    private canvas: React.RefObject<HTMLCanvasElement>;

    constructor(props: ICanvasComponentProps) {
        super(props);
        this.canvas = React.createRef();
    }

    public render() {
        return (
            <div>
                <canvas
                    ref={this.canvas}
                    width={this.props.width}
                    height={this.props.height} />
            </div>
        );
    }

    public componentDidMount() {
        const ctx = (this.canvas.current as HTMLCanvasElement).getContext('2d') as CanvasRenderingContext2D;
        const start: number = Date.now().valueOf();
        setInterval(() => {
            const elapsed: number = Date.now().valueOf() - start.valueOf();

            const r = Math.floor(Math.random() * 255);
            const g = Math.floor(Math.random() * 255);
            const b = Math.floor(Math.random() * 255);
            const x = Math.floor(Math.random() * this.props.width);
            const y = Math.floor(Math.random() * this.props.height);
            ctx.fillStyle = '#ffffff';
            ctx.fillRect(0, 0, this.props.width, this.props.height);
            ctx.fillStyle = `rgb(${r}, ${g}, ${b})`;
            // ctx.fillRect(x, y, 10, 10);
            ctx.beginPath();
            ctx.moveTo(x, y);
            ctx.lineTo(x + 10, y + 10);
            // ctx.stroke();
            ctx.font = "50px Roboto";
            ctx.fillStyle = "#777";
            ctx.fillText(`${(Math.floor(elapsed / 1000)).toString()}s`, 100, 100);
        }, 500);
    }
}

export default CanvasComponent;