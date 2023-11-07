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
        try {
            dataJson = data.json;
            if (dataJson.success == true) {
                document.getElementById('successMessage').innerHTML = dataJson.message;
                $('#successModal').modal('show');
            } else throw new Exception(dataJson.error);
        } catch (err) {
            document.getElementById("body_content").innerHTML = data.text;
        }
        
    }).then(error => {
        document.getElementById('errorMessage').innerHTML = error;
        $('#errorModal').modal('show');
    })
    return false;
}