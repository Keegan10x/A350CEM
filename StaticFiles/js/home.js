/* home.js */

import { customiseNavbar } from "../util.js";

export async function setup(node) {
  console.log("HOME: setup");
  const token = localStorage.getItem("authorization");      
  console.log('LOGGING', token);

  if(token == null){
    customiseNavbar(["home", "login"])
  }else{
    customiseNavbar(["home", "logout"])
  }


  try {
    console.log('hello world')
  } catch (err) {
    console.error(err);
  }
}




