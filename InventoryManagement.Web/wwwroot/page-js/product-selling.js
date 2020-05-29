
//date picker
$('.datepicker').pickadate().val(moment(new Date()).format('DD MMMM, YYYY'));
document.getElementById('label-date').classList.add('active');

 // Material Select Initialization
$('.mdb-select').materialSelect();


//*****SELECTORS***//

//product code form
const formCode = document.getElementById('formCode')
const inputBarCode = formCode.inputBarCode
const productCount = formCode.querySelector('#productCount')
const btnFind = formCode.btnFind

//product table
const tbody = document.getElementById('t-body')


//****FUNCTIONS****//

//scan barcode
onScan.attachTo(document, {
    suffixKeyCodes: [13],
    reactToPaste: true,
    onScan: (sCode, iQty) => {
        inputBarCode.value = sCode
        inputBarCode.nextElementSibling.classList.add('active')
        btnFind.click()
    }
})

//create table rows
const createTableRow = function (item) {
    let tr = document.createElement("tr");
    tr.setAttribute('data-id', item.ProductId);

    //column 1
    let span = document.createElement('span');
    span.classList.add('text-dark');
    span.appendChild(document.createTextNode(item.ProductCatalogName));

    let p = document.createElement('p');
    p.classList.add('text-muted', 'small', 'mb-0');
    p.appendChild(document.createTextNode(item.ProductName));

    let td1 = tr.insertCell(0);
    td1.appendChild(span);
    td1.appendChild(p);

    //column 2
    let td2 = tr.insertCell(1);
    td2.appendChild(document.createTextNode(1));

    //column 3
    let td3 = tr.insertCell(2);
    td3.appendChild(document.createTextNode(item.SellingPrice));

    //column 4
    let i = document.createElement('i');
    i.classList.add('fal', 'fa-times', 'remove');
    let td4 = tr.insertCell(3);
    td4.classList.add('text-center');
    td4.appendChild(i);

    return tr;
}


//event listners
formCode.addEventListener('submit', evt => {
    evt.preventDefault()
    const url = '/Product/FindProductByCode'
    const param = { params: { code: inputBarCode.value } };

        axios.get(url, param)
            .then(res => {
                console.log(res.data)
                if (!res.data) return
                tbody.appendChild(createTableRow(res.data))
            })
        .catch(err => console.log(err))
})




//****CUSTOMER****//