/* UploadCsv.js */

import { customiseNavbar, file2DataURI, loadPage, secureGet, showMessage, loadData, displayData } from '../util.js'

export async function setup(node) {
	console.log('services: setup')



	try {
		const token = localStorage.getItem('authorization')
		if(!token)loadPage("home"); //redirect to home if not authorized

		//console.log(node)
		customiseNavbar(['home', 'logout',])

		const data = await loadData(`https://localhost:7098/api/v1/services`, token)
  		console.log(data)
		
		displayData(data, node, 'template#services', 'Services')
		//displayData(array, node, 'template#hmdt', 'Humidity')


		node.querySelector("form").addEventListener("submit", await addRecord)
	} catch(err) {
		console.error(err)
	}
}


async function addRecord() {
	event.preventDefault()
	console.log("form submitted");
	const formData = new FormData(event.target);
	const data = Object.fromEntries(formData.entries());

	console.log("LOGGING FORM OBJECT", data)
	
	const formattedDate = data.date.split("-").join("/");

	let postObject = {}
	postObject.Date = formattedDate

	if(data.completed === 'true'){
		postObject.Completed = true
	}else{
		postObject.Completed = false
	}

	postObject.Fee = parseInt(data.fee)
	postObject.Tel = parseInt(data.tel)

	console.log("LOGGING POSTABLE OBJECT", postObject)

	const uri = "https://localhost:7098/api/v1/services"
	const options = {
		method: "POST",
		headers: {
			"Content-Type": "application/vnd.api+json",
			"Authorization": localStorage.getItem("authorization"),
		},
		body: JSON.stringify(postObject)
	}
	try{
		const rsp = await fetch(uri, options)
		const json = await rsp.json();
		console.log(json)
		console.log("SUCCESS")
	}catch(err){
		console.error(err)
		console.log("FAILED")
	}finally{
		loadPage("services")
	}

  }
