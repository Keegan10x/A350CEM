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
		

		//displayData(array, node, 'template#hmdt', 'Humidity')

		
	} catch(err) {
		console.error(err)
	}
}

