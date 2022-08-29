import React, { PureComponent } from "react";

class BuildingType extends PureComponent {
    render() {
        const size = 100;

        return (
            <table>
                <tbody>
                    <tr>
                        <td rowSpan="2">
                            <img src={process.env.PUBLIC_URL + '/img/buildings/' + this.props.data.id + '.png'}
                                 style={{ width: size, height: size, background: "transparent" }}
                                 alt={this.props.desc} />
                        </td>
                        <td>{this.props.data.name}</td>
                    </tr>
                    <tr>
                        <td>{this.props.data.description}</td>
                    </tr>
                </tbody>
                
            </table>
        );
    }
}

export { BuildingType };