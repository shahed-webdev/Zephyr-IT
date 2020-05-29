
//date picker
$('.datepicker').pickadate().val(moment(new Date()).format('DD MMMM, YYYY'));
document.getElementById('label-date').classList.add('active');

 // Material Select Initialization
$('.mdb-select').materialSelect();


//*****SELECTORS***//

//product code form
const formCode = document.getElementById('formCode')
const inputBarCode = formCode.inputBarCode

//****FUNCTIONS****//

//scan barcode
onScan.attachTo(document, {
    suffixKeyCodes: [13],
    reactToPaste: true,
    onScan: (sCode, iQty) => {
        inputBarCode.value = sCode
        inputBarCode.nextElementSibling.classList.add('active')
    }
})

//event listners
formCode.addEventListener('submit', evt => {
    evt.preventDefault()
    console.log('form submitted')
})




//****CUSTOMER****//