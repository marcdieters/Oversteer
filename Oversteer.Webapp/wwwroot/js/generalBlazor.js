// Collapse sidemenu
document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll('.sidebar .nav-link').forEach(function (element) {

        element.addEventListener('click', function (e) {

            let nextEl = element.nextElementSibling;
            let parentEl = element.parentElement;

            if (nextEl) {
                e.preventDefault();
                let mycollapse = new bootstrap.Collapse(nextEl);

                if (nextEl.classList.contains('show')) {
                    mycollapse.hide();
                } else {
                    mycollapse.show();
                    var opened_submenu = parentEl.parentElement.querySelector('.submenu.show');
                    if (opened_submenu) {
                        new bootstrap.Collapse(opened_submenu);
                    }
                }
            }
        });
    })
}); 

// Get Cookie by name
window.getcookie2 = (name) => {
    console.log(name);
    var pattern = RegExp(name + "=.[^;]*")
    var matched = document.cookie.match(pattern)
    if (matched) {
        var cookie = matched[0].split('=')
        return cookie[1]
    }
    return "none"
};

// Set Cookie by name and value
window.setcookie2 = (name, value) => {
    var expires;
    var date = new Date();
    date.setTime(date.getTime() + (30 * 24 * 60 * 60 * 1000));
    expires = "; expires=" + date.toGMTString();
    document.cookie = name + "=" + value + expires + "; path=/";
};

// Add datatable
function DataTablesAdd(table) {
    $(document).ready(function () {
        $(table).DataTable();
    });
}

// Remove datatable
function DataTablesRemove(table) {
    $(document).ready(function () {
        $(table).DataTable().destroy();
    });
}

window.localDate = () => {
    var ldCurrentDate = new Date();
    return ldCurrentDate.getFullYear() +
        "-" + String(ldCurrentDate.getMonth() + 1).padStart(2, '0') +
        "-" + String(ldCurrentDate.getDate()).padStart(2, '0') +
        "T" +
        String(ldCurrentDate.getHours()).padStart(2, '0') +
        ":" + String(ldCurrentDate.getMinutes()).padStart(2, '0') +
        ":" + String(ldCurrentDate.getSeconds()).padStart(2, '0');
};
window.utcDate = () => {
    var ldCurrentDate = new Date();
    return ldCurrentDate.getUTCFullYear() +
        "-" + String(ldCurrentDate.getUTCMonth() + 1).padStart(2, '0') +
        "-" + String(ldCurrentDate.getUTCDate()).padStart(2, '0') +
        "T" +
        String(ldCurrentDate.getUTCHours()).padStart(2, '0') +
        ":" + String(ldCurrentDate.getUTCMinutes()).padStart(2, '0') +
        ":" + String(ldCurrentDate.getUTCSeconds()).padStart(2, '0');
};
window.timeZoneOffset = () => {
    return new Date().getTimezoneOffset() / 60;
};