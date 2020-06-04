
//selectors
const tableBody = document.getElementById('table-body');
const btnCreate = document.getElementById('CreateClick');
const insertModal = $('#InsertModal');


//functions
const createLink = function (item) {
    const link = document.createElement("a");

    link.href = `/Expenses/Delete/${item.ExpenseId}`;
    link.classList.add('delete', 'fas', 'fa-trash-alt');

    return link;
}

const createTableRow = function (item) {
    const tr = document.createElement("tr");

    //column 1
    const td1 = tr.insertCell(0);
    const textNode1 = document.createTextNode(item.ExpenseAmount);
    td1.appendChild(textNode1);
    //column 2
    const td2 = tr.insertCell(1);
    const textNode2 = document.createTextNode(item.ExpenseFor? item.ExpenseFor: '');
    td2.appendChild(textNode2);
    //column 3
    const td3 = tr.insertCell(2);
    const textNode3 = document.createTextNode(item.ExpensePaymentMethod);
    td3.appendChild(textNode3);
    //column 4
    const td4 = tr.insertCell(3);
    const textNode4 = document.createTextNode(moment(item.ExpanseDate).format('DD MMM YYYY'));
    td4.appendChild(textNode4);
    //column 5
    const td5 = tr.insertCell(4);
    td5.appendChild(createLink(item));

    return tr;
}

const displayCategory = function (data) {
    if (!data.length)
        tableBody.innerHTML = "<tr><td colspan='5'>No record found!</td></tr>";
    else
        tableBody.innerHTML = '';

    const fragment = document.createDocumentFragment();

    data.forEach(item => {
        const tr = createTableRow(item);
        fragment.appendChild(tr);
    });

    tableBody.appendChild(fragment);
}

const getData = function () {
    const url = '/Expenses/IndexData';
    const request = axios.get(url);

    request.then(response => displayCategory(response.data));
}

const onDeleteClicked = function (evt) {
    evt.preventDefault();

    const target = evt.target;
    const deleteClicked = target.classList.contains('delete');

    if (!deleteClicked) return;
 
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

const onCreateClicked = function (evt) {
    const url = evt.target.getAttribute("data-url");
    axios.get(url).then(res => {
        insertModal.html(res.data).modal('show');
    });
}

function onCreateSuccess(data) {
    if (data !== 'success') return;
    insertModal.modal('hide');
    getData();
}



//event listners
tableBody.addEventListener("click", onDeleteClicked);
btnCreate.addEventListener('click', onCreateClicked);

//call function
getData();