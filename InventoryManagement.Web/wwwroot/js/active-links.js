
let linkData = [{
    "LinkCategoryID": 1,
    "SN": 1,
    "Category": "Sub-Admin",
    "IconClass": "fas fa-user-tie",
    "links": [{
        "LinkID": 3,
        "SN": 0,
        "Controller": "Dashboard",
        "Action": "Index",
        "Title": "Dashboard",
        "IconClass": null
    },
    {
        "LinkID": 1,
        "SN": 1,
        "Controller": "SubAdmin",
        "Action": "Index",
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
    "SN": 2,
    "Category": "Product",
    "IconClass": "fas fa-shopping-cart",
    "links": [{
        "LinkID": 4,
        "SN": 1,
        "Controller": "ProductCategories",
        "Action": "IndexView",
        "Title": "Categories",
        "IconClass": null
    },
    {
        "LinkID": 6,
        "SN": 1,
        "Controller": "Products",
        "Action": "Index",
        "Title": "Products",
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
        "Controller": "ExpanseCategories",
        "Action": "Index",
        "Title": "Categories",
        "IconClass": null
    },
    {
        "LinkID": 7,
        "SN": 2,
        "Controller": "Expanses",
        "Action": "Index",
        "Title": "Expenses",
        "IconClass": null
    }]
},
{
    "LinkCategoryID": 6,
    "SN": 4,
    "Category": "Vendor/Party",
    "IconClass": "fas fa-user-tie",
    "links": [{
        "LinkID": 8,
        "SN": 1,
        "Controller": "Vendors",
        "Action": "Index",
        "Title": "Vendors",
        "IconClass": null
    }]
},
{
    "LinkCategoryID": 4,
    "SN": 5,
    "Category": "Sell",
    "IconClass": "fas fa-cart-plus",
    "links": [{
        "LinkID": 9,
        "SN": null,
        "Controller": "Selling",
        "Action": "Selling",
        "Title": "Selling",
        "IconClass": null
    },
    {
        "LinkID": 10,
        "SN": null,
        "Controller": "Selling",
        "Action": "Record",
        "Title": "Records",
        "IconClass": null
    }]
},
{
    "LinkCategoryID": 5,
    "SN": 6,
    "Category": "Reports",
    "IconClass": "fas fa-file-alt",
    "links": [{
        "LinkID": 11,
        "SN": 1,
        "Controller": "Report",
        "Action": "Income",
        "Title": "Income",
        "IconClass": null
    },
    {
        "LinkID": 12,
        "SN": 2,
        "Controller": "Report",
        "Action": "Expense",
        "Title": "Expense",
        "IconClass": null
    },
    {
        "LinkID": 13,
        "SN": 3,
        "Controller": "Report",
        "Action": "Selling",
        "Title": "Selling",
        "IconClass": null
    },
    {
        "LinkID": 14,
        "SN": 4,
        "Controller": "Report",
        "Action": "Vendor",
        "Title": "Due Summery",
        "IconClass": null
    },
    {
        "LinkID": 15,
        "SN": 5,
        "Controller": "Report",
        "Action": "PaymentSummery",
        "Title": "Payment Summery",
        "IconClass": null
    },
    {
        "LinkID": 16,
        "SN": 6,
        "Controller": "Report",
        "Action": "ProductSummery",
        "Title": "Product Summery",
        "IconClass": null
    },
    {
        "LinkID": 17,
        "SN": 8,
        "Controller": "Report",
        "Action": "NetSummery",
        "Title": "Net Summery",
        "IconClass": null
    }]
    }]

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
    let html = "";
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
    this.nextElementSibling.classList.toggle("active");
    this.classList.toggle("open");
}

function setNavigation() {
    let links = menuItem.querySelectorAll(".links");
    let path = window.location.pathname;

    path = path.replace(/\/$/, "");
    path = decodeURIComponent(path);

    links.forEach(link => {
        let href = link.getAttribute('href');

        if (path === href) {
            link.parentElement.parentElement.classList.add("active");
            link.classList.add('link-active');
        }
    });
}

//on Content Loaded
if (document.readyState === 'loading')
    document.addEventListener('DOMContentLoaded', setNavigation);
else
    setNavigation()