import React, { PureComponent } from "react";
import Dropdown from 'react-bootstrap/Dropdown';
import './Styles.css';

class Building extends PureComponent {

    constructor(props) {
        super(props);
        this.state = { coins: 0 };
    }

    componentDidMount() {
        setInterval(() => {
            this.setState({ coins: this.props.owner.props.owner.state.coins });
        }, 1000);
    }

    handleClick = event => {
        //if (this.props.building == 0) {
        //    this.props.owner.addBuilding(this.props.building.x, this.props.building.y);
        //}
    };

    clickPlus = () => {
        this.props.owner.upBuilding(this.props.id);
    }

    clickMinus = () => {
        this.props.owner.downBuilding(this.props.id);
    }

    clickAdd = (buildingId) => {
        this.props.owner.addBuilding(this.props.x, this.props.y, buildingId + 1);
    }

    clickRemove = () => {
        this.props.owner.removeBuilding(this.props.id);
    }

    buildCost = (initialCost, level) => {
        return Math.trunc(initialCost * (1 + 0.1 * (level - 1)));
    }

    buildAllCost = (initialCost, level) => {
        let cost = 0;
        while (level >= 1) {
            cost += this.buildCost(initialCost, level);
            level--;
        }

        return cost;
    }

    isExistNeighbor = (x, y) => {
        const buildings = this.props.owner.props.owner.state.buildings;
        return (buildings.find(i => i.x === x - 1 && i.y === y) != null) ||
            (buildings.find(i => i.x === x + 1 && i.y === y) != null) ||
            (buildings.find(i => i.x === x && i.y === y - 1) != null) ||
            (buildings.find(i => i.x === x && i.y === y + 1) != null);
    }

    render() {
        const size = 100;

        let table = [];
        for (let i = 0; i < this.props.owner.props.owner.state.buildingTypes.length; i++) {
            let menuItem = <table className="dropdowntable">
                               <tbody>
                               <tr>
                                   <td style={{ width: "80px" }}>
                                       <img src={process.env.PUBLIC_URL + '/img/buildings/' + (i + 1) + '.png'}
                                            style={{
                                                width: 32,
                                                height: 32,
                                                background: "transparent",
                                                marginLeft: "10px"
                                            }}
                                            alt={this.props.desc} />
                                   </td>
                                   <td style={{ width: "300px" }}>
                                        <h4 style={{ display: "inline-block" }}>{this.props.owner.props.owner.state.buildingTypes[i].name}</h4>
                                        <span style={{ fontSize: 14, marginLeft: "10px" }}>
                                            {this.props.owner.props.owner.separateComma(this.props.owner.props.owner.state.buildingTypes[i].initionalCoins)} / сек
                                        </span>
                                   </td>
                               </tr>
                               <tr>
                                   <td style={{ width: "80px" }}>
                                       <img src={process.env.PUBLIC_URL + '/img/Coin.png'}
                                            style={{
                                                width: 16,
                                                height: 16,
                                                background: "transparent",
                                                display: "inline-block",
                                                marginRight: "5px"
                                            }}
                                            alt="Монеты" />
                                       {this.props.owner.props.owner.separateComma(this.buildCost(this.props.owner.props.owner.state.buildingTypes[i].initionalCost, 1))}
                                   </td>
                                   <td style={{ width: "300px" }}>
                                       <span style={{ fontSize: 14 }}>{this.props.owner.props.owner.state.buildingTypes[i].description}</span>
                                   </td>
                               </tr>
                               </tbody>
                           </table>;

            if (this.state.coins >= this.buildCost(this.props.owner.props.owner.state.buildingTypes[i].initionalCost, 1)) {
                table.push(<Dropdown.Item onClick={() => this.clickAdd(i)} key={this.props.building * 1000 + i}>
                    {menuItem}
                </Dropdown.Item>);
            } else {
                table.push(<Dropdown.ItemText key={this.props.building * 1000 + i} className="graytext">
                    {menuItem}
                </Dropdown.ItemText>);
            }
        }

        const plusItem =
            <span style={{ display: "inline-block", whiteSpace: "nowrap" }}>
                <img src={process.env.PUBLIC_URL + '/img/Coin.png'}
                    style={{ width: 24, height: 24, background: "transparent", marginRight: "5px", verticalAlign: "middle", display: "inline-block" }}
                     alt={this.props.desc} />
                <span style={{ display: "inline-block", fontSize: 18, color: "forestgreen" }}>-{this.buildCost(this.props.initionalCost, this.props.level + 1)}</span>
                <span style={{ display: "inline-block", fontSize: 18, marginLeft: "10px" }}>Добавить</span>
                <span style={{ display: "inline-block", fontSize: 14, marginLeft: "10px" }}>+{this.props.initionalCoins} / сек</span>
            </span>;

        const plusMenu = this.state.coins >= this.buildCost(this.props.initionalCost, this.props.level + 1)
            ? <Dropdown.Item onClick={() => this.clickPlus()}>{plusItem}</Dropdown.Item>
            : <Dropdown.ItemText className="graytext">{plusItem}</Dropdown.ItemText>;

        let minusItem = this.props.level > 1
            ? <Dropdown.Item onClick={() => this.clickMinus()}>
                <img src={process.env.PUBLIC_URL + '/img/Coin.png'}
                    style={{
                        width: 24,
                        height: 24,
                        background: "transparent",
                        marginRight: "5px",
                        verticalAlign: "middle"
                    }}
                    alt={this.props.desc} />
                <span style={{ fontSize: 18, color: "forestgreen" }}>+{this.buildCost(this.props.initionalCost, this.props.level)}</span>
                <span style={{ fontSize: 18, marginLeft: "10px" }}>Убрать</span>
                <span style={{ fontSize: 14, marginLeft: "10px" }}>-{this.props.initionalCoins} / сек</span>
            </Dropdown.Item>
            : "";

        const deleteItem = this.props.owner.props.owner.state.buildings.length > 1 
            ? <Dropdown.Item onClick={() => this.clickRemove()}>
                  <img src={process.env.PUBLIC_URL + '/img/Remove.png'}
                       style={{
                        width: 24,
                        height: 24,
                        background: "transparent",
                        marginRight: "5px",
                        verticalAlign: "middle"
                    }}
                       alt="Удалить здание" />
                  <span style={{ fontSize: 18, color: "forestgreen" }}>+{this.buildAllCost(this.props.initionalCost, this.props.level)}</span>
                  <span style={{ fontSize: 18, marginLeft: "10px" }}>Снести</span>
                  <span style={{ fontSize: 14, marginLeft: "10px" }}>-{this.props.initionalCoins * this.props.level} / сек</span>
            </Dropdown.Item>
            : ""


        let dropMenu = this.props.building === 0
            ? (this.isExistNeighbor(this.props.x, this.props.y)
                ? <Dropdown>
                    <Dropdown.Toggle id="dropdown-basic">
                        +
                    </Dropdown.Toggle>
                    <Dropdown.Menu>
                        {table}
                    </Dropdown.Menu>
                    </Dropdown>
                : "")
            : <Dropdown>
                  <Dropdown.Toggle id="dropdown-basic">
                      √
                  </Dropdown.Toggle>

                <Dropdown.Menu>
                    {plusMenu}
                    {minusItem}
                    {deleteItem}
                </Dropdown.Menu>
            </Dropdown>;

        let levelSpan = this.props.building === 0
            ? ""
            : <span style={{
                position: "absolute", marginLeft: "3px", marginTop: "3px", border: "1px solid black", borderRadius: "50%",
                width: "30px", height: "30px", fontSize: "18px", background: "lightgray"
            }}>{this.props.level}</span>;

        return (
            <div>
                {levelSpan}
                <img src={process.env.PUBLIC_URL + '/img/buildings/' + this.props.building + '.png'}
                    style={{ width: size, height: size, background: "transparent" }}
                    alt={this.props.desc} onClick={this.handleClick} />
                {dropMenu}
            </div>
        );
    }
}

export { Building };