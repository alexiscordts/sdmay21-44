import React from "react";
import Nav from "./Nav";
import "./Settings.css";


const Settings = () => {

return ( 
    <div>
        <Nav/>
        <div class='pageContainer'>
            <button class = "manageLocationButton" onClick={() => {window.location.href = "/manage_locations"}}>
                Manage Locations
            </button>
        </div>
    </div>
    );
};

export default Settings;