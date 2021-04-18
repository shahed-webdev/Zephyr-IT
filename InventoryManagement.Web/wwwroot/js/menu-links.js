
//let linkData = [{
//    "LinkCategoryID": 1,
//    "SN": 1,
//    "Category": "Sub-Admin",
//    "IconClass": "fas fa-user-tie",
//    "links": [{
//        "LinkID": 3,
//        "SN": 0,
//        "Controller": "SubAdmin",
//        "Action": "SignUp",
//        "Title": "Sign Up",
//        "IconClass": null
//    },
//    {
//        "LinkID": 1,
//        "SN": 7,
//        "Controller": "SubAdmin",
//        "Action": "List",
//        "Title": "Sub-admins",
//        "IconClass": null
//    },
//    {
//        "LinkID": 2,
//        "SN": 2,
//        "Controller": "SubAdmin",
//        "Action": "PageAccess",
//        "Title": "Page Access",
//        "IconClass": null
//    }]
//},
//{
//    "LinkCategoryID": 2,
//    "SN": 7,
//    "Category": "Customer",
//    "IconClass": "fas fa-user",
//    "links": [
//    {
//        "LinkID": 6,
//        "SN": 2,
//        "Controller": "Customer",
//        "Action": "List",
//        "Title": "Customers",
//        "IconClass": null
//    }]
//},
//{
//    "LinkCategoryID": 3,
//    "SN": 3,
//    "Category": "Expense",
//    "IconClass": "fas fa-chart-pie",
//    "links": [{
//        "LinkID": 5,
//        "SN": 1,
//        "Controller": "ExpenseCategories",
//        "Action": "Index",
//        "Title": "Categories",
//        "IconClass": null
//    },
//    {
//        "LinkID": 7,
//        "SN": 2,
//        "Controller": "Expenses",
//        "Action": "Index",
//        "Title": "Expenses",
//        "IconClass": null
//    }]
//},
//{
//    "LinkCategoryID": 6,
//    "SN": 4,
//    "Category": "Vendor",
//    "IconClass": "fas fa-user-tie",
//    "links": [{
//        "LinkID": 8,
//        "SN": 1,
//        "Controller": "Vendor",
//        "Action": "List",
//        "Title": "Vendors",
//        "IconClass": null
//    }]
//},
//    {
//        "LinkCategoryID": 2,
//        "SN": 7,
//        "Category": "Product",
//        "IconClass": "fas fa-shopping-cart",
//        "links": [{
//            "LinkID": 11,
//            "SN": 3,
//            "Controller": "Product",
//            "Action": "Cataloglist",
//            "Title": "Category",
//            "IconClass": null
//        },
//        {
//            "LinkID": 9,
//            "SN": 1,
//            "Controller": "Product",
//            "Action": "Barcode",
//            "Title": "Barcode",
//            "IconClass": null
//        },
//        {
//            "LinkID": 9,
//            "SN": 1,
//            "Controller": "Product",
//            "Action": "Selling",
//            "Title": "Selling",
//            "IconClass": null
//        },
//            ,
//        {
//            "LinkID": 6,
//            "SN": 1,
//            "Controller": "Product",
//            "Action": "SellingRecords",
//            "Title": "Selling Invoice",
//            "IconClass": null
//        },
//        {
//            "LinkID": 6,
//            "SN": 1,
//            "Controller": "Product",
//            "Action": "Purchase",
//            "Title": "Purchase",
//            "IconClass": null
//        },
//        {
//            "LinkID": 6,
//            "SN": 1,
//            "Controller": "Product",
//            "Action": "PurchaseRecords",
//            "Title": "Purchase Invoice",
//            "IconClass": null
//        }]
//    }];

//selectors
const menuItem = document.getElementById("menuItem");

//functions

//get data from server
const getMenuData = function () {
    const url = '/Basic/GetSideMenu'
    axios.get(url)
        .then(response => {
            appendMenuDOM(response.data)
            setNavigation()
        })
        .catch(err => console.log(err))
}

//create links
const createLinks = function (links) {
    let fragment = document.createDocumentFragment()
    links.forEach(link => {
        let anchor = document.createElement('a')
        anchor.classList.add('links')
        anchor.href = `/${link.Controller}/${link.Action}`
        anchor.textContent = link.Title

        let li = document.createElement('li')
        li.appendChild(anchor)

        fragment.appendChild(li)
    });

    return fragment
}

//create link li
const linkCategory = function (category, iconCss, links) {
    let span = document.createElement('span')
    span.className = iconCss

    let ico = document.createElement('i')
    ico.classList.add('fas', 'fa-caret-right')

    let strong = document.createElement('strong')
    strong.appendChild(span)
    strong.appendChild(ico)
    strong.appendChild(document.createTextNode(category))  

    let ul = document.createElement('ul')
    ul.classList.add('sub-menu')
    ul.appendChild(links)

    let li = document.createElement('li')
    li.appendChild(strong)
    li.appendChild(ul)

    return li
}

//append link to DOM
const appendMenuDOM = function (linkData) { 
    let fragment = document.createDocumentFragment()

    let span = document.createElement('span')
    span.className = 'fas fa-tachometer-alt'

    let anchor = document.createElement('a')
    anchor.classList.add('links')
    anchor.href = '/Dashboard/Index'
    anchor.textContent = 'Dashboard'

    let strong = document.createElement('strong')
    strong.appendChild(span)
    strong.appendChild(anchor)

    let li = document.createElement('li')
    li.appendChild(strong)

    fragment.appendChild(li)
    
    linkData.forEach(item => {
        const li = linkCategory(item.Category, item.IconClass, createLinks(item.links))
        fragment.appendChild(li)
    });

    menuItem.appendChild(fragment)
}

//active current url
function setNavigation() {
    const links = menuItem.querySelectorAll(".links")
    let path = window.location.pathname

    path = path.replace(/\/$/, "")
    path = decodeURIComponent(path)

    links.forEach(link => {
        const href = link.getAttribute('href')

        if (path === href) {
            if (link.parentElement.nodeName !== "STRONG") {
                const prentElement = link.parentElement.parentElement
                prentElement.previousElementSibling.classList.add("open")
                prentElement.classList.add("active")
            }

            link.classList.add('link-active')
        }
    })
}

//click on link
const linkCategoryClicked = function (evt) {
    const element = evt.target;
    if (element.nodeName === "STRONG") {
        if (element.nextElementSibling) {
            element.nextElementSibling.classList.toggle("active")
            element.classList.toggle("open")
        }
    }
}

//event listener
menuItem.addEventListener("click", linkCategoryClicked);

//on load
getMenuData();