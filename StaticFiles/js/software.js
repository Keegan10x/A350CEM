/* UploadCsv.js */

import { customiseNavbar, file2DataURI, loadPage, secureGet, showMessage, loadData, displayData } from '../util.js'

export async function setup(node) {
	console.log('uploadCsv: setup')
	try {
		console.log(node)
		customiseNavbar(['home', 'login'])
		if(localStorage.getItem('authorization'))customiseNavbar(['home', 'logout',])
		// there is a token in localstorage
		const array = await loadData('pressure', `/api/v1/data`)
		console.log(array)
		displayData(array, node, 'template#prss', 'Pressure')
	} catch(err) {
		console.error(err)
	}

}


