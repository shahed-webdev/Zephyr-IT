
//selectors
const phoneContainer = document.getElementById("phone-wrapper");
const customerForm = document.getElementById("customer-form");

//functions
let elementIndex = 1;
const isError = [];

const btnEnabledDisable = function () {
    btnSubmit.disabled = isError.length ? true : false;
}

const checkPhoneIsExists = function (evt) {    
    const phoneInput = evt.target.classList.contains("valid-check");
    if (!phoneInput) return;

    const mobile = evt.target.value;
    const errorElement = evt.target.nextElementSibling;

    if (errorElement.nodeName === "SPAN")
        errorElement.remove();

    if (mobile.length > 10) {
        const url = '/Customer/CheckMobileIsAvailable';
        const parameter = { params: { mobile } };
        const request = axios.get(url, parameter);
        const element = `<span class="field-validation-error">This Mobile Number Already Exists!</span>`;
        const self = evt.target;
        const errorIndex = isError.indexOf(self.id);

        request.then(response => {
            if (response.data) {
                self.insertAdjacentHTML('afterend', element);
                if (errorIndex === -1) isError.push(self.id);
            }
            else {
                if (errorIndex !== -1) isError.splice(errorIndex, 1);
            }

            btnEnabledDisable();
        }).catch(err => console.log(err));
    }
}

const addInputelement = function () {
    const element = `<div class="phone-container">
                <div class="md-form m-0 flex-grow-1">
                    <input id="phone-${elementIndex}" name="PhoneNumbers[${elementIndex}].Phone" required type="number" class="form-control valid-check" />
                    <label for="phone-${elementIndex}">Phone Number</label>
                </div>
                <div class="add-element">
                    <a class="btn-floating btn-sm bg-danger remove m-0"><i class="fas fa-minus"></i></a>
                </div>
            </div>`;

    elementIndex++;
    phoneContainer.insertAdjacentHTML('beforeend', element);
}

const removeInputelement = function (evt) {
    evt.target.parentElement.parentElement.remove();
    const id = evt.target.parentElement.previousElementSibling.children[0].id;
    const errorIndex = isError.indexOf(id);

    if (errorIndex !== -1) isError.splice(errorIndex, 1);

    btnEnabledDisable();
}

const togglePhoneElement = function (evt) {
    const addClicked = evt.target.classList.contains("add");
    const removeClicked = evt.target.classList.contains("remove");

    if (addClicked)
        addInputelement(evt);

    if (removeClicked)
        removeInputelement(evt);
}

const onFormSubmit = function (evt) {
    evt.target.btnSubmit.disabled = true;
    evt.target.btnSubmit.innerText = "Please wait...";

    setTimeout(()=> {
        evt.target.btnSubmit.disabled = false;
        evt.target.btnSubmit.innerText = "Add Customer";
    }, 3000);
}

//events
phoneContainer.addEventListener("click", togglePhoneElement);
phoneContainer.addEventListener("input", checkPhoneIsExists);
customerForm.addEventListener('submit', onFormSubmit);
