import React, {useState} from "react";
import "../FormStyles.css";
import Nav from "../Nav";

const AddAdmin = () => {
  const [fName, setFName] = useState('');
  const [lName, setLName] = useState('');
  const [email, setEmail] = useState('');
    
  //Form for adding an admin user
  return (
    <div >
      <Nav/>
      <div class="formScreen">
        <div class="form-style">
          <div class="form-style-heading"> Add an Admin </div>
            <form action="" method="post">
              <label for="fname">
                    <span>
                      First Name
                      <span class="required">*</span>
                    </span>
                    <input 
                      type="text" 
                      class="input-field" 
                      onChange={(e)=> setFName(e.target.value)} 
                      name="fname" 
                      value={fName}
                    />
                  </label>
                  <label for="lname">
                    <span>
                      Last Name
                      <span class="required">*</span>
                    </span>
                    <input 
                      type="text" 
                      class="input-field" 
                      onChange={(e)=> setLName(e.target.value)} 
                      name="lname" 
                      value={lName}
                    />
                  </label>
                  <label for="email">
                    <span>
                      E-mail
                      <span class="required">*</span>
                    </span>
                    <input 
                      type="text" 
                      class="input-field" 
                      onChange={(e)=> setEmail(e.target.value)} 
                      name="email" 
                      value={email}
                    />
                  </label>
                  <p class="submitLabel">By clicking the button, an email will be sent to the Admin with their login information.</p>
                <div class="submitLabel"><input type="submit" value="Create" /></div>
              </form>
            </div>
        </div>
      </div>
    );
  };
export default AddAdmin;