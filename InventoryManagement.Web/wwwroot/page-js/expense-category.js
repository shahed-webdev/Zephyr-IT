
//selectors
const tableBody = document.getElementById('table-body');
const btnCreate = document.getElementById('CreateClick');
const updateModal = $('#UpdateModal');
const insertModal = $('#InsertModal');


//functions
const isObject = function (value) {
    return value && typeof value === 'object' && value.constructor === Object;
}

const createLink = function (type, item) {
    const link = document.createElement("a");

    if (type === "edit") {
        link.href = `/ExpenseCategories/Edit/${item.ExpenseCategoryId}`;
        link.classList.add('edit', 'fas', 'fa-edit');
    }

    if (type === "delete") {
        link.href = `/ExpenseCategories/Delete/${item.ExpenseCategoryId}`;
        link.classList.add('delete', 'fas', 'fa-trash-alt');
    }

    return link;
}

const createTableRow = function (item) {
    const tr = document.createElement("tr");
    tr.setAttribute('data-id', item.ExpenseCategoryId);

    //column 1
    const td1 = tr.insertCell(0);
    const textNode = document.createTextNode(item.CategoryName);
    td1.appendChild(textNode);

    //column 2
    const td2 = tr.insertCell(1);
    td2.appendChild(createLink('edit', item));

    //column 3
    const td3 = tr.insertCell(2);
    td3.appendChild(createLink('delete', item));

    return tr;
}

const displayCategory = function (data) {
    if (!data.length) {
        tableBody.innerHTML = "<tr class='first-row'><td colspan='3'>No record found!</td></tr>";
        return
    }

    const fragment = document.createDocumentFragment();
    data.forEach(item => {
        fragment.appendChild(createTableRow(item));
    });

    tableBody.innerHTML = '';
    tableBody.appendChild(fragment);
}

const getData = function () {
    const url = '/ExpenseCategories/IndexData';
    const request = axios.get(url);

    request.then(response => displayCategory(response.data));
}

const onEdit = function (evt) {
    const url = evt.target.getAttribute("href");
    axios.get(url).then(res => {
        updateModal.html(res.data).modal('show');
    });
}

function onUpdateSuccess(data) {
    if (!isObject(data)) return;
    updateModal.modal('hide');

    const updatedId = data.ExpenseCategoryId;

    tableBody.querySelectorAll('tr').forEach(tr => {
        const id = +tr.getAttribute('data-id');

        if (updatedId === id) {
            tr.children[0].innerText = data.CategoryName;
            return;
        }
    });
}

const onDelete = function (evt) {
    const target = evt.target;
    const element = target.parentElement.parentElement;
    const url = target.getAttribute("href");
    if (!url) return;

    const isConfirm = confirm("Are you sure you want to delete?");
    if (!isConfirm) return;

    axios.get(url).then(res => {
        if (res.data === -1) {
            target.removeAttribute("href");
            element.insertAdjacentHTML('afterend', `<em class="used-error">"${element.innerText}" already used!</em>`);
            return;
        }
        element.remove();
    }).catch(err => console.log(err));
}

const onEditDeleteClicked = function (evt) {
    evt.preventDefault();

    const element = evt.target;
    const editClicked = element.classList.contains('edit');
    const deleteClicked = element.classList.contains('delete');

    if (editClicked)
        onEdit(evt);

    if (deleteClicked)
        onDelete(evt);
}

const onCreateClicked = function (evt) {
    const url = evt.target.getAttribute("data-url");
    axios.get(url).then(res => {
        insertModal.html(res.data).modal('show');
    });
}

function onCreateSuccess(data) {
    if (!isObject(data)) return;
    insertModal.modal('hide');

    //append new row
    const tr = createTableRow(data);
    const fRow = tableBody.querySelectorAll('tr')[0]
    tableBody.appendChild(tr);
}


//event listeners
tableBody.addEventListener("click", onEditDeleteClicked);
btnCreate.addEventListener('click', onCreateClicked);

//call function
getData();

const fRow = tableBody.querySelectorAll('tr')
console.log(fRow)