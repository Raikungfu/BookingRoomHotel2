function loginAdmin() {
    var form = document.getElementById("loginAdminForm");
    form.addEventListener('submit', (e) => e.preventDefault());
    const dataToSend = new FormData(form);
    fetch('/Admin/Login', {
        method: "POST",
        body: dataToSend
    }).then(response => {
        if (response.ok) return response.json();
        else throw new Error('Network was not ok! Status: ' + response.status);
    }).then(data => {
        if (data.success === true) {
            if (data.accessToken !== null) localStorage.setItem("accessToken", data.accessToken);
            if (data.role !== null) localStorage.setItem("Role", data.role);
            if (data.name !== null) localStorage.setItem("Name", data.name);
            window.location.href = '/Admin/Dashboard';
        }
        else {
            throw new Error(data.error);
        }
    }).catch(error => {
        var errorStatus = document.createElement('div');
        errorStatus.className = 'text-danger';
        errorStatus.innerText = error;
        form.appendChild(errorStatus);
        window.setTimeout(function () {
            errorStatus.remove();
        }, 5000);
    });
    return false;
}
