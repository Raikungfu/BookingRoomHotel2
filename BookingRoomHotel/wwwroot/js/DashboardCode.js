﻿function pagination(endPoint) {
    fetch(endPoint, {
        method: "POST",
        headers: {
            "Authorization": `Bearer ${jwtToken}`
        }
    }).then(response => {
        if (response.ok) {
            return response.text();
        } else {
        }
    }).then(data => {
        document.getElementById("listData").innerHTML = data;
    }).catch(error => {
        alert(error);
    });
    return false;
}
function sendJwt(endPoint, id) {
    if (id !== '') {
        endPoint += id;
    }
    fetch(endPoint, {
        method: "GET",
        headers: {
            "Authorization": `Bearer ${jwtToken}`
        }
    }).then(response => {
        if (response.ok) {
            return response.text();
        } else {
        }
    }).then(data => {
        document.getElementById("listData").innerHTML = data;
    }).catch(error => {
        alert(error);
    });
    return false;
}
var idDel;
function delete(endPoint) {
    sendJwt(endPoint, idDel);
}

function setIdToDelete(id) {
    idDel = id;
}
