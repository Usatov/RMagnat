import React, { Component } from 'react';
import { v4 as uuid } from 'uuid';

export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);
        this.state = { user: {}, loading: true };
    }

    componentDidMount() {
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

    render() {
        let userName = this.state.loading
            ? "stranger"
            : this.state.user.name;

        return (
            <div>
                <h1>Hello, {userName}!</h1>
                <p>Welcome to your new single-page application, built with:</p>
                <ul>
                  <li><a href='https://get.asp.net/'>ASP.NET Core</a> and <a href='https://msdn.microsoft.com/en-us/library/67ef8sbd.aspx'>C#</a> for cross-platform server-side code</li>
                  <li><a href='https://facebook.github.io/react/'>React</a> for client-side code</li>
                  <li><a href='http://getbootstrap.com/'>Bootstrap</a> for layout and styling</li>
                </ul>
                <p>To help you get started, we have also set up:</p>
                <ul>
                  <li><strong>Client-side navigation</strong>. For example, click <em>Counter</em> then <em>Back</em> to return here.</li>
                  <li><strong>Development server integration</strong>. In development mode, the development server from <code>create-react-app</code> runs in the background automatically, so your client-side resources are dynamically built on demand and the page refreshes when you modify any file.</li>
                  <li><strong>Efficient production builds</strong>. In production mode, development-time features are disabled, and your <code>dotnet publish</code> configuration produces minified, efficiently bundled JavaScript files.</li>
                </ul>
                <p>The <code>ClientApp</code> subdirectory is a standard React application based on the <code>create-react-app</code> template. If you open a command prompt in that directory, you can run <code>npm</code> commands such as <code>npm test</code> or <code>npm install</code>.</p>
            </div>
        );
    }

    async getUserData() {
        // eslint-disable-next-line
        const webRoot = process.env.NODE_ENV == "production" ? "" : "/";
        const uid = this.getUserId();

        fetch(webRoot + "api/user/get/" + uid)
            .then(response => response.json())
            .then(data => {
                sessionStorage.setItem("session", data.sessionId);
                this.setState({ user: data, loading: false });
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
}
