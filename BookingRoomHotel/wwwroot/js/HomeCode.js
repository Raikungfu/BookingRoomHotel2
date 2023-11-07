
function saveSearchInfo() {
    var form = document.getElementById('formSearch');
    const dataToSend = new FormData(form);
    var object = {};
    var json = formDataToJson(dataToSend);
    localStorage.setItem('searchForm', json);
}


function BookingRoom() {
    var form = document.getElementById('formBooking');
    const dataToSend = new FormData(form);
    localStorage.setItem('formSearch', formDataToJson(dataToSend));
    var endPoint = '/Home/Booking';
    sendToBookingAction(endPoint, dataToSend);
    return false;
}

function sendToBookingAction(endPoint, dataToSend) {
    fetch(endPoint, {
        method: "POST",
        body: dataToSend
    }).then(response => {
        return response.json();
    }).then(data => {
        if (data.success == true) {
            document.getElementById('successMessage').innerHTML = data.message;
            $('.modal').modal().hide();
            $('.modal').on('hidden.bs.modal', function () {
                $('.modal-backdrop').remove();
            });
            $('#successModal').modal('show');
            if (data.noti !== null) appendNoti(data.noti);
        } else {
            for (var err in data) {
                document.getElementById(err).innerHTML = data[err];
            }
        }
    }).catch(error => {
        document.getElementById('BookingError').innerHTML = error;
    });
}

function formDataToJson(formData) {
    const json = {};
    formData.forEach((value, key) => {
        json[key] = value;
    });
    return JSON.stringify(json);
}

function jsonToFormData(json) {
    const formData = new FormData();
    for (const key in json) {
        if (json.hasOwnProperty(key)) {
            formData.append(key, json[key]);
        }
    }
    return formData;
}

function setFormBooking(roomID, roomP) {
    document.getElementById('BookingRoomID').value = roomID;
    document.getElementById('BookingPrice').value = roomP;
    document.getElementById('IdCus').value = localStorage.getItem('IdCus');
}

function appendNoti(noti) {
    var notification = JSON.parse(noti);
    var notiList = document.getElementById('notiList');
    var noti = document.createElement('li');
    noti.className = "notification-message";
    noti.innerHTML = `
        <a href="#">
            <div class="media">
                <span class="avatar avatar-sm">
                    <img class="avatar-img rounded-circle" alt="User Image" src="/images/Admin/k-logo-design.jpg">
                </span>
                <div class="media-body">
                    <p class="noti-details">
                        <span class="noti-title"> ${notification.Title}</span><br>
                        <span class="noti-content"> ${notification.Content}</span><br>
                        <p class="noti-time"><span class="notification-time">${notification.CreatedAt}</span> </p>
                    </p>
                </div>
            </div>
        </a>
    `;
    notiList.prepend(noti);
}

function searchRoom(endPoint) {
    var form = document.getElementById("formData");
    const dataToSend = new FormData(form);
    fetch(endPoint, {
        method: "POST",
        body: dataToSend
    }).then(response => {
        if (response.ok) {
            return response.text();
        } else {
            throw new Error('Network was not ok! Status: ' + response.status);
        }
    }).then(data => {
        document.getElementById("listData").innerHTML = data;
    }).catch(error => {

    });
    return false;
}