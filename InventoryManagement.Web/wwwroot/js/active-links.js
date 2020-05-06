
let linkData = [{
    "LinkCategoryID": 1,
    "SN": 1,
    "Category": "Sub-Admin",
    "IconClass": "fas fa-user-tie",
    "links": [{
        "LinkID": 3,
        "SN": 0,
        "Controller": "SubAdmin",
        "Action": "SignUp",
        "Title": "Sign Up",
        "IconClass": null
    },
    {
        "LinkID": 1,
        "SN": 7,
        "Controller": "SubAdmin",
        "Action": "List",
        "Title": "Sub-admins",
        "IconClass": null
    },
    {
        "LinkID": 2,
        "SN": 2,
        "Controller": "SubAdmin",
        "Action": "PageAccess",
        "Title": "Page Access",
        "IconClass": null
    }]
},
{
    "LinkCategoryID": 2,
    "SN": 7,
    "Category": "Customer",
    "IconClass": "fas fa-user",
    "links": [{
        "LinkID": 4,
        "SN": 1,
        "Controller": "Customer",
        "Action": "Add",
        "Title": "Add Customer",
        "IconClass": null
    },
    {
        "LinkID": 6,
        "SN": 2,
        "Controller": "Customer",
        "Action": "List",
        "Title": "Customers",
        "IconClass": null
    }]
},
{
    "LinkCategoryID": 3,
    "SN": 3,
    "Category": "Expense",
    "IconClass": "fas fa-chart-pie",
    "links": [{
        "LinkID": 5,
        "SN": 1,
        "Controller": "ExpenseCategories",
        "Action": "Index",
        "Title": "Categories",
        "IconClass": null
    },
    {
        "LinkID": 7,
        "SN": 2,
        "Controller": "Expenses",
        "Action": "Index",
        "Title": "Expenses",
        "IconClass": null
    }]
},
{
    "LinkCategoryID": 6,
    "SN": 4,
    "Category": "Vendor",
    "IconClass": "fas fa-user-tie",
    "links": [{
        "LinkID": 8,
        "SN": 1,
        "Controller": "Vendor",
        "Action": "List",
        "Title": "Vendors",
        "IconClass": null
    }]
},
    {
        "LinkCategoryID": 2,
        "SN": 7,
        "Category": "Product",
        "IconClass": "fas fa-shopping-cart",
        "links": [{
            "LinkID": 9,
            "SN": 1,
            "Controller": "Product",
            "Action": "Barcode",
            "Title": "Barcode",
            "IconClass": null
        },
        {
            "LinkID": 10,
            "SN": 2,
            "Controller": "Product",
            "Action": "Catalog",
            "Title": "Category",
            "IconClass": null
        },
        {
            "LinkID": 6,
            "SN": 1,
            "Controller": "Product",
            "Action": "List",
            "Title": "Products",
            "IconClass": null
        }]
    }
]

//selectors
const menuItem = document.getElementById("menuItem");

//on load
getMenus();

//add event listener
menuItem.querySelectorAll("strong").forEach(category => {
    category.addEventListener("click", linkCategoryClicked);
});

//functions
function getMenus() {
    let html = `<li><strong><span class="fas fa-tachometer-alt"></span><a class="links" href="/Dashboard/Index">Dashboard</a></strong></li>`;
    linkData.forEach(item => {    
        html += `<li><strong><span class="${item.IconClass}"></span>${item.Category} <i class="fas fa-caret-right"></i></strong><ul class="sub-menu">${appendLinks(item.links)}</ul></li>`;
    });
    menuItem.innerHTML = html;
}

function appendLinks(links) {
    let html = "";
    links.forEach(link => {
        html += `<li><a class="links" href="/${link.Controller}/${link.Action}">${link.Title}</a></li>`;
    });
    return html;
}

function linkCategoryClicked() {
    if (this.nextElementSibling) {
        this.nextElementSibling.classList.toggle("active");
        this.classList.toggle("open");
    }
}

function setNavigation() {
    const links = menuItem.querySelectorAll(".links");
    let path = window.location.pathname;

    path = path.replace(/\/$/, "");
    path = decodeURIComponent(path);

    links.forEach(link => {
        const href = link.getAttribute('href');

        if (path === href) {
            if (link.parentElement.nodeName !== "STRONG") {
                const prentElement = link.parentElement.parentElement;
                prentElement.previousElementSibling.classList.add("open");
                prentElement.classList.add("active");
            }

            link.classList.add('link-active');
        }
    });
}

//on Content Loaded
if (document.readyState === 'loading')
    document.addEventListener('DOMContentLoaded', setNavigation);
else
    setNavigation()