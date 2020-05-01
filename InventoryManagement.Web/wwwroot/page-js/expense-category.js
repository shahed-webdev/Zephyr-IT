
//selectors
const tableBody = document.getElementById('table-body');
const btnCreate = document.getElementById('CreateClick');
const updateModal = $('#UpdateModal');
const insertModal = $('#InsertModal');


//functions
const createLink = function (type, item) {
    let link = document.createElement("a");

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
    let tr = document.createElement("tr");

    //column 1
    let td1 = tr.insertCell(0);
    let textNode = document.createTextNode(item.CategoryName);
    td1.appendChild(textNode);

    //column 2
    let td2 = tr.insertCell(1);
    td2.appendChild(createLink('edit', item));

    //column 3
    let td3 = tr.insertCell(2);
    td3.appendChild(createLink('delete', item));

    return tr;
}

const displayCategory = function (data) {
    if (!data.length)
        tableBody.innerHTML = "<tr><td colspan='3'>No record found!</td></tr>";
    else
        tableBody.innerHTML = '';

    let fragment = document.createDocumentFragment();

    data.forEach(item => {
        const tr = createTableRow(item);
        fragment.appendChild(tr);
    });

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

const onDelete = function (evt) {
    if (confirm("Are you sure you want to delete?")) {
        const element = evt.target;
        const url = element.getAttribute("href");

        axios.get(url).then(res => {
            element.parentElement.parentElement.remove();
        }).catch(err => console.log(err));
    }
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

function onUpdateSuccess(data) {
    if (data !== "success") return;
    updateModal.modal('hide');
}

const onCreateClicked = function (evt) {
    const url = evt.target.getAttribute("data-url");
    axios.get(url).then(res => {
        insertModal.html(res.data).modal('show');
    });
}

function onCreateSuccess(data) {
    if (data !== "success") return;
    insertModal.modal('hide');
}


//event listners
tableBody.addEventListener("click", onEditDeleteClicked);
btnCreate.addEventListener('click', onCreateClicked);

//call function
getData();