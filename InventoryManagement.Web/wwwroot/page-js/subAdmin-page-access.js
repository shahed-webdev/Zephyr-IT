
//selectors
const formAssign = document.getElementById('formAssign');
const selectSubAdmin = formAssign.selectSubAdmin;
const tableContainer = formAssign.querySelector('#table-container');
const SuccessMessage = formAssign.querySelector('#SuccessMessage');

//functions
//const onChangeSubAdmin = function () {
//    const regId = +this.value;
//    const url = "/SubAdmin/GetLinks";
//    tableContainer.innerHTML = '';

//    if (!regId) return;

//    const request = axios.get(url, { params: { regId } });
//    request.then(response => {
//        console.log(response.data)
//    })
//        .catch(err => console.log(err));
//}

//submit form
const onFormSubmit = function (evt) {
    evt.preventDefault();
}

//event listner
formAssign.addEventListener('submit',onFormSubmit);
//selectSubAdmin.addEventListener('change',onChangeSubAdmin);