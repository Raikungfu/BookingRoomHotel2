function sendQuestion() {
    var form = document.getElementById('formQuestion');
    const dataToSend = new FormData(form);
    fetch('/Questions/Create', {
        method: 'POST',
        body: dataToSend
    }).then(response => {
        if (response.ok) return response.json;
        else throw new Exception(response.status);
    }).then(data => {
        if (data.success == true) {
            document.getElementById('successMessage').innerHTML = data.message;
            $('#successModal').modal('show');
        } else throw new Exception(data.error);
    }).then(error => {
        document.getElementById('errorMessage').innerHTML = error;
        $('#errorModal').modal('show');
    })
    return false;
}