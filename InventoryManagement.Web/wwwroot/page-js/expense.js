
//selectors
const tableBody = document.getElementById('table-body');
const btnCreate = document.getElementById('CreateClick');
const insertModal = $('#InsertModal');
const updateModal = $('#updateModal');


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
    const td4 = tr.insertCell(2);
    const textNode4 = document.createTextNode(moment(item.ExpenseDate).format('DD MMM YYYY'));
    td4.appendChild(textNode4);
    //column 4
    const td5 = tr.insertCell(3);
    td5.appendChild(createLink(item));

    return tr;
}

const displayExpense = function (data) {
    console.log(data)
    if (!data.length)
        tableBody.innerHTML = "<tr><td colspan='4'>No record found!</td></tr>";
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

    request.then(response => displayExpense(response.data));
}

//update show
tableBody.addEventListener("click", function (evt) {
    evt.preventDefault();

    const target = evt.target;
    const onClick = target.classList.contains('updateModal');

    if (!onClick) return;

    const url = target.getAttribute("href");

    axios.get(url).then(res => {
        updateModal.html(res.data).modal('show');
    });
});

function onUpdateSuccess(data) {
    if (data !== 'success') return;
    updateModal.modal('hide');
    getData();
}


//insert expense
btnCreate.addEventListener('click', function (evt) {
    const url = evt.target.getAttribute("data-url");
    axios.get(url).then(res => {
        insertModal.html(res.data).modal('show');
    });
});

function onCreateSuccess(data) {
    if (data !== 'success') return;
    insertModal.modal('hide');
    getData();
}


//call function
getData();