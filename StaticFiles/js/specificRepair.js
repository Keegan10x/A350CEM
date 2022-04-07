import { customiseNavbar,loadPage } from "../util.js";

export async function setup(node) {
  console.log("Showing Specific Repair Item");
  const token = localStorage.getItem("authorization");      
  console.log('LOGGING', token);

  if(token == null){
    loadPage("home")
  }else{
    customiseNavbar(["home", "logout"])

	console.log(window.myGlobalVariable.taskId)			      
	const uri = `https://localhost:7098/api/v1/Repair/${window.myGlobalVariable.taskId}`
    const options = {
		method: "GET",
		headers: {
			"Content-Type": "application/vnd.api+json",
			"Authorization": localStorage.getItem("authorization"),
		}
	}
    try{
		const rsp = await fetch(uri, options)
		const data = await rsp.json();
		console.log(data)
		console.log("SUCCESS")
	
		
    const template = document.querySelector('template#home')
    //const fragment = template.content.cloneNode(true)
    const form = document.createElement('form')
    form.name = 'updateRecord'

    
    //Completed
    //true
    const p1 = document.createElement('p')
    p1.innerHTML = `<h1>Update Repair Record ${window.myGlobalVariable.taskId}</h1>`

    const lb1 = document.createElement('label')
    lb1.innerHTML = '<h3>True</h3>'
    const radioTrue = document.createElement('input')
    radioTrue.setAttribute("type", "radio");
    radioTrue.setAttribute('name', 'completed');
    radioTrue.value = 'true'
    radioTrue.id = 'true'


    //false
    const lb2 = document.createElement('label')
    lb2.innerHTML = '<h3>False</h3>'
    const radioFalse = document.createElement('input')
    radioFalse.setAttribute("type", "radio");
    radioFalse.setAttribute('name', 'completed');
    radioFalse.value = 'false'
    radioFalse.id = 'false'

    //Date
    const p2 = document.createElement('p')
    p2.innerHTML = '<h3>Date</h3>'
    const inputDate = document.createElement('input')
    inputDate.setAttribute("type", "date");
    inputDate.id = 'date'
    inputDate.value = ''
    
    //Fee
    const p3 = document.createElement('p')
    p3.innerHTML = "<h3>Fee</h3>"
    const inputFee = document.createElement('input')
    inputFee.setAttribute("type", "number");
    inputFee.id = 'fee'
    inputFee.value = ''

    //Tel
    const p4 = document.createElement('p')
    p4.innerHTML = '<h3>Tel</h3>'
    const inputTel = document.createElement('input')
    inputTel.setAttribute("type", "phone");
    inputTel.id = 'tel'
    inputTel.value = ''

    p1.appendChild(lb1)
    p1.appendChild(radioTrue)
    p1.appendChild(lb2)
    p1.appendChild(radioFalse)

    p2.appendChild(inputDate)
    p3.appendChild(inputFee)
    p4.appendChild(inputTel)
    form.appendChild(p1)
    form.appendChild(p2)
    form.appendChild(p3)
    form.appendChild(p4)
    

    //populate data from fetch request.
    //populate completed status
    if(data.completed == true){
        radioTrue.checked=true
    }else{
        radioFalse.checked=true
    }    

    //populate date
    const populateDate = data.date.split("/").join("-");
    inputDate.value = populateDate;

    //populate fee
    inputFee.value = data.fee

    //populate fee
    inputTel.value = data.tel
    
   

    //Submit
    const submit = document.createElement('input')
    submit.innerText = 'Update'
    submit.setAttribute("type", "submit");
    submit.id = 'submit' 
    form.appendChild(submit)
    


    

    node.appendChild(form)


   


    }catch(err){
    console.error(err)
    console.log("FAILED")
}finally{
    node.querySelector("form").addEventListener("submit", async () => {
        event.preventDefault()
        const updateForm = document.querySelector("form")
        
        let completeVal = updateForm.querySelector('input[name="completed"]:checked').value;
        if(completeVal == 'true'){
            completeVal = true
        }else{
            completeVal = false
        }

        //console.log(completeVal)

        const dateVal = updateForm.querySelector('input[type="date"]').value;
        const feeVal = updateForm.querySelector('input[type="number"]').value;
        const telVal = updateForm.querySelector('input[type="phone"]').value;
        

        const putObj = {
            Date: dateVal.split("-").join("/"),
            Completed: completeVal,
            Fee: parseInt(feeVal),
            Tel: parseInt(telVal)
        }
        //console.log("LOGGING PUT OBJ", putObj)

        const putUri = `https://localhost:7098/api/v1/repair/${window.myGlobalVariable.taskId}`
	    const putOptions = {
		method: "PUT",
		headers: {
			"Content-Type": "application/vnd.api+json",
			"Authorization": localStorage.getItem("authorization"),
		},
		body: JSON.stringify(putObj) 
	    }
        try{
            await put(putUri, putOptions)
		    console.log("SUCCESS")
	    }catch(err){
		    console.error(err)
		    console.log("FAILED")
	    }finally{
		    loadPage("repair")
	}
    })
}
  
}
}


async function put(link, data){
    const response = await fetch(link, data)
	const result = await response.json();
	console.log(result)
    return result
}

