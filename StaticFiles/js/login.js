/* login.js */

import {
  createToken,
  customiseNavbar,
  loadPage,
  secureGet,
  showMessage,
} from "../util.js";

export async function setup(node) {
  try {
    console.log("LOGIN: setup");
    console.log(node);

    customiseNavbar(["home", "login"]);
    node.querySelector("form").addEventListener("submit", await login);
  } catch (err) {
    console.error(err);
  }
}

async function login() {
  event.preventDefault();
  console.log("form submitted");
  const formData = new FormData(event.target);
  const data = Object.fromEntries(formData.entries());
  const token = "Basic " + btoa(`${data.user}:${data.pass}`);
  console.log("making call to secureGet");
  const response = await secureGet("https://localhost:7098/api/v1/accounts", token);
  console.log(response);
  if (response.status === 200) {
    localStorage.setItem("authorization", token);
    showMessage(`you are logged in as admin`);
    await loadPage("home");
  } else {
    document.querySelector('input[name="pass"]').value = "";
    showMessage(response.json.errors[0].detail);
  }
}
