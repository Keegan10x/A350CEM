/* util.js */

export async function loadData(uri, token){
	const options = {
    method: "GET",
    headers: {
      "Content-Type": "application/vnd.api+json",
      "Authorization": token,
    }
  }
	const rsp = await fetch(uri, options)
	const result = await rsp.json()
  return result
}

export function displayData(array, node, templateName, param){
	const template = document.querySelector(templateName)
	const fragment = template.content.cloneNode(true)
	const table = document.createElement('table')
	const thead = document.createElement('thead')

  const idHeader = document.createElement('th')
  idHeader.innerText = 'ID'
  const completedHeader = document.createElement('th')
  completedHeader.innerText = 'Completed'
  const dateHeader = document.createElement('th')
  dateHeader.innerText = 'Date'
  const feeHeader = document.createElement('th')
  feeHeader.innerText = 'Fee'
  const telHeader = document.createElement('th')
  telHeader.innerText = 'Tel'
  
	const tr1 = document.createElement('tr')
  tr1.appendChild(idHeader)
  tr1.appendChild(completedHeader) 
  tr1.appendChild(dateHeader)
  tr1.appendChild(feeHeader)
  tr1.appendChild(telHeader)

	//const th = document.createElement('th')
  //th.innerText = paramName
	const tbody = document.createElement('tbody')
	for(const val of array){
		const tr = document.createElement('tr')
		const idCell = document.createElement('td')
    const completedCell = document.createElement('td')
    const dateCell = document.createElement('td')
    const feeCell = document.createElement('td')
    const telCell = document.createElement('td')
    idCell.innerText = val.id
    completedCell.innerText = val.completed
    dateCell.innerText = val.date
    feeCell.innerText = val.fee
    telCell.innerText = val.tel
		
		tr.appendChild(idCell)
    tr.appendChild(completedCell)
    tr.appendChild(dateCell)
    tr.appendChild(feeCell)
    tr.appendChild(telCell)

		tbody.appendChild(tr)
	}
	
	thead.appendChild(tr1)
	table.appendChild(thead)
	table.appendChild(tbody)

  const h1 = document.createElement('h1')
  h1.innerText = param
  fragment.appendChild(h1)

	fragment.appendChild(table)

  
	node.appendChild(fragment)
}


export function showMessage(message, delay = 3000) {
  console.log(message);
  document.querySelector("aside p").innerText = message;
  document.querySelector("aside").classList.remove("hidden");
  setTimeout(
    () => document.querySelector("aside").classList.add("hidden"),
    delay,
  );
}

/* NAV FUNCTIONS */

export function loadPage(page) {
  history.pushState(null, null, `/${page}`);
  triggerPageChange();
}

export async function triggerPageChange() {
  console.log("pageChange");
  const page = getPageName();
  console.log(`trying to load page: ${page}`);
  // get a reference to the correct template element
  const template = document.querySelector(`template#${page}`) ??
    document.querySelector("template#home");
  const node = template.content.cloneNode(true); // get a copy of the template node
  try {
    const module = await import(`./js/${page}.js`);
    await module.setup(node); // the setup script may need to modify the template fragment before it is displayed
  } catch (err) {
    console.warn(`no script for "${page}" page or error in script`);
    console.log(err);
  }
  // replace contents of the page with the correct template
  const article = document.querySelector("article");
  while (article.lastChild) article.removeChild(article.lastChild); // remove any content from the article element
  article.appendChild(node); // insert the DOM fragment into the page
  highlightNav(page);
  article.id = page;
}

function getPageName() {
  console.log(window.location.pathname);
  const path = window.location.pathname.replace("/", "");
  let page = path ? path : "home";
  console.log(`page: ${page}`);
  return page;
}

export function highlightNav(page) {
  document.querySelectorAll("nav li").forEach((element) => {
    const link = element.querySelector("a").href.replace(
      `${window.location.origin}/`,
      "",
    ) || "home";
    if (link === page) {
      element.classList.add("currentpage");
    } else {
      element.classList.remove("currentpage");
    }
  });
  document.querySelector("nav").style.visibility = "visible";
}

export function customiseNavbar(items) {
  document.querySelectorAll("nav li").forEach((element) => {
    const link = element.querySelector("a").href.replace(
      `${window.location.origin}/`,
      "",
    ) || "home";
    if (items.includes(link)) {
      element.style.display = "block";
    } else {
      element.style.display = "none";
    }
  });
}

/* FUNCTIONS USED IN FORMS */

export function createToken(username, password) {
  const token = btoa(`${username}:${password}`);
  return `Basic ${token}`;
}

export function file2DataURI(file) {
  return new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.onload = () => {
      resolve(reader.result);
    };
    reader.readAsDataURL(file);
  });
}

/* FUNCTIONS TO MAKE API CALLS
 * all API calls support the JSON:API specification */

export async function secureGet(url, token) {
  console.log("secure get");
  const options = {
    method: "GET",
    headers: {
      "Authorization": token,
      "Content-Type": "application/vnd.api+json",
      "Accept": "application/vnd.api+json",
    },
  };
  console.log(options);
  const response = await fetch(url, options);
  const json = await response.json();
  return { status: response.status, json: json };
}
