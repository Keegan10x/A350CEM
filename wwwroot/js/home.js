/* home.js */

import { customiseNavbar } from "../util.js";

export async function setup(node) {
  console.log("HOME: setup");
  try {
    
    const token = localStorage.getItem("authorization");      
    console.log('LOGGING', token);

    if (token){
      customiseNavbar(["home", "logout"]);
    }else{
      customiseNavbar(["home", "login"]);
    }  

  } catch (err) {
    console.error(err);
  }
}




