﻿document.addEventListener('DOMContentLoaded', () => {
    if (window.location.pathname === "/Admin" || window.location.pathname === "/Admin/Dashboard") {
        document.getElementById("navHome").classList.add("d-none");
    }
    if (window.location.href === "/Admin" && checkRole() && !roleCus()) {
        window.location.href = "/Admin/Dashboard";
    }
});