import React, { Component } from 'react';
import { v4 as uuid } from 'uuid';
import { Game } from './Game'
import './Styles.css';

export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);
        this.state = { user: {}, buildingTypes: [], buildings: [], coins: 0, loading: true };
    }

    componentDidMount() {
        setInterval(() => {
            this.setState({ coins: this.state.coins + this.state.user.coinsPerSecond });
        }, 1000);
        this.getUserData();
    }

    getUserId() {
        let user_uid = localStorage.getItem('user_uid');
        if (user_uid == null) {
            user_uid = uuid();
            localStorage.setItem('user_uid', user_uid);
        }
        return user_uid;
    }

    separateComma(val) {
        let sign = 1;
        if (val < 0) {
            sign = -1;
            val = -val;
        }
        
        let num = val.toString().includes('.') ? val.toString().split('.')[0] : val.toString();
        let len = num.toString().length;
        let result = '';
        let count = 1;

        for (let i = len - 1; i >= 0; i--) {
            result = num.toString()[i] + result;
            if (count % 3 === 0 && count !== 0 && i !== 0) {
                result = ',' + result;
            }
            count++;
        }

        
        if (val.toString().includes('.')) {
            result = result + '.' + val.toString().split('.')[1];
        }
        
        return sign < 0 ? '-' + result : result;
    }


    render() {
        if (this.state.loading) {
            return (
                <div className="center">
                    Загрузка...
                </div>
            );
        } else {
            return (
                <div className="center">
                    <h1>Добытчик ресурсов</h1>
                    <span style={{ ver: "inline-block" }}>
                        <img src={process.env.PUBLIC_URL + '/img/Coin.png'}
                            style={{ width: 32, height: 32, background: "transparent", display: "inline-block" }}
                            alt="Монеты" />
                        <h2 style={{ display: "inline-block", verticalAlign: "middle", marginLeft: 10 }}>{this.separateComma(this.state.coins)}</h2>
                        <h4>{this.state.user.coinsPerSecond} / сек.</h4>
                    </span>
                    <Game buildings={this.state.buildings} owner={this} />
                </div>
            );
        }
    }

    // async
    getUserData() {
        // eslint-disable-next-line
        const webRoot = process.env.NODE_ENV == "production" ? "" : "/";

        // Получаем информацию о пользователе
        fetch(webRoot + "api/user/get/" + this.getUserId())
            .then(response => response.json())
            .then(data => {
                sessionStorage.setItem("session", data.sessionId);
                this.setState({ user: data, coins: data.coins });
                this.getBuildings();

            });

        //let response = await fetch(webRoot + 'api/user/uid?uid=' + uid);
        //if (response.status === 404) {
        //    const requestOptions = {
        //        method: 'POST',
        //        headers: { 'Content-Type': 'application/json' },
        //        body: JSON.stringify({
        //            name: "User",
        //            uid: uid
        //        })
        //    };

        //    await fetch(webRoot + 'api/user/', requestOptions);
        //    response = await fetch(webRoot + 'api/user/uid?uid=' + uid);
        //    let data = await response.json();
        //    this.setState({ user: data, loading: false });
        //} else {
        //    let data = await response.json();
        //    this.setState({ user: data, loading: false });
        //}
    }

    getBuildings() {
        // eslint-disable-next-line
        const webRoot = process.env.NODE_ENV == "production" ? "" : "/";
        const sessionId = sessionStorage.getItem("session");

        // Получаем информацию о всех зданиях
        fetch(webRoot + "api/building/")
            .then(response => response.json())
            .then(data => {
                this.setState({ buildingTypes: data });

                // Получаем информацию о зданиях пользователя
                fetch(webRoot + "api/building/own/" + sessionId)
                    .then(response => response.json())
                    .then(data => {
                        data.forEach(element => {
                            const buildingType = this.state.buildingTypes.find(b => b.id == element.buildingTypeId);
                            if (buildingType != null) {
                                element.name = buildingType.name;
                                element.desc = buildingType.description;
                            }
                        });

                        this.setState({ buildings: data, loading: false });
                    });
            });
    }

    addBuilding(x, y, id) {
        // eslint-disable-next-line
        const webRoot = process.env.NODE_ENV == "production" ? "" : "/";
        const sessionId = sessionStorage.getItem("session");

        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                buildingTypeId: id,
                x: x,
                y: y
            })
        };

        fetch(webRoot + "api/building/add/" + sessionId, requestOptions)
            .then(response => {
                this.getUserData();
            });

    }

    upBuilding(id) {
        // eslint-disable-next-line
        const webRoot = process.env.NODE_ENV == "production" ? "" : "/";
        const sessionId = sessionStorage.getItem("session");

        fetch(webRoot + "api/building/up/" + sessionId + "/" + id)
            .then(response => {
                this.getUserData();
            });

    }

    downBuilding(id) {
        // eslint-disable-next-line
        const webRoot = process.env.NODE_ENV == "production" ? "" : "/";
        const sessionId = sessionStorage.getItem("session");

        fetch(webRoot + "api/building/down/" + sessionId + "/" + id)
            .then(response => {
                this.getUserData();
            });

    }

    removeBuilding(id) {
        // eslint-disable-next-line
        const webRoot = process.env.NODE_ENV == "production" ? "" : "/";
        const sessionId = sessionStorage.getItem("session");

        fetch(webRoot + "api/building/remove/" + sessionId + "/" + id)
            .then(response => {
                this.getUserData();
            });

    }
}
