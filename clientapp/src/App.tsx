import React, { useState, useEffect } from 'react';
import './App.css';
import * as signalR from "@microsoft/signalr";

import 'bootstrap/dist/css/bootstrap.min.css';
import { Table, Alert } from 'react-bootstrap';
import { UserType } from './types';

const roles = [
  "1", "2", "3"
]
const App = () => {

  const hubConnection = new signalR.HubConnectionBuilder().
    withUrl("/roleChangeHub").build();
  try {
    hubConnection.start();
  } catch (err) {
    console.log(err);
  }
  interface SignalR {
    connect: signalR.HubConnection
  }
  

  const Messages = (messageProps: SignalR) => {
    const [list, setList] = useState<string[]>([]);
    useEffect(() => {
      messageProps.connect.on("sendToReact", message => {
        setList(oldArray => [...oldArray, message]);
      });
    }, [])
    return (
      <>
      
        {list.map((message, index) =>
          <Alert variant="success" key={`message${index}`}>{message}</Alert>
        )}
      </>
    )
  }

  const UserList = (userProps: SignalR) => {

    const [users, setUsers] = useState<UserType[]>([]);
  
    useEffect(() => {
      userProps.connect.on("UpdateUsers", users => {
        setUsers(users);
      });
    }, [])
  
    return (
      <>
        <Table striped bordered hover>
          <thead>
            <tr>
              <th>ID</th>
              <th>Name</th>
              <th>Role</th>
            </tr>
          </thead>
          <tbody>

            {users.map((user: UserType, idx) =>

              <tr key={user.id}>

                <td >{user.id}</td>
                <td>{user.name}</td>
                <td><select value={user.role} onChange={(e) => {
                  user.role = e.target.value;
                  userProps.connect.invoke("EditUser",user);
                }}>{
                    roles.map((role, index) =>
                      <option key={user.id + index} value={role} >{role}</option>

                    )
                  }
                </select></td>
              </tr>
            )}

          </tbody>
        </Table>
      </>)
  }


  return (
  <>
    <UserList connect={hubConnection} />

    <Messages connect={hubConnection}></Messages>
  </>
  )
}




export default App;
