/* home.js */
window.myGlobalVariable = {}
import { customiseNavbar,loadPage } from "../util.js";

export async function setup(node) {
  console.log("HOME: setup");
  const token = localStorage.getItem("authorization");      
  console.log('LOGGING', token);

  if(token == null){
    customiseNavbar(["home", "login"])
  }else{
    customiseNavbar(["home", "logout"])


    const template = document.querySelector('template#home')
    const fragment = template.content.cloneNode(true)
    const servicesButton = document.createElement('a')
    const softwareButton = document.createElement('a')
    const repairButton = document.createElement('a')
    const inspectionButton = document.createElement('a')

    servicesButton.innerText = 'Services'
    softwareButton.innerText = 'Software'
    repairButton.innerText = 'Repairs'
    inspectionButton.innerText = 'Inspections'


    servicesButton.onclick=() => { loadPage("services") };
    softwareButton.onclick=() => { loadPage("software") };
    repairButton.onclick=() => { loadPage("repair") };
    inspectionButton.onclick=() => { loadPage("inspection") };

    const section = document.createElement('section')
    section.appendChild(servicesButton)
    section.appendChild(softwareButton)
    section.appendChild(repairButton)
    section.appendChild(inspectionButton)

    fragment.appendChild(section)
    node.appendChild(fragment) 
  }


  try {
    console.log('hello world')
  } catch (err) {
    console.error(err);
  }
}




