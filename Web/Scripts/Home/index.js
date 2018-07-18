$(document).ready(function () {
    $(".datepicker").datepicker({ dateFormat: 'dd.mm.yy' });
    $(".addPerson").hide();
    $(".edit").hide();
    $(".updateButton").hide();
    $(".cancelButton").hide();
    pageableTable(10);
});

function uploadPersons(input) {
    var formData = new FormData();
    for (var i = 0; i < input.files.length; i++) {
        formData.append('files', input.files[i]);
    }

    $.ajax({
        url: "/PersonsApi/UploadPersons",
        data: formData,
        method: "POST",
        processData:  false,
        contentType: false,
        success: function (response) {
            if (response.message)
                alert(response.message);
            if (response.success)
                location.reload();
        },
        error: function (response) {
            alert("Ошибка при загрузке!");
        }
    })
}

function editPerson(id) {
    $("#tr" + id).children().find(".read").hide();
    $("#tr" + id).children().find(".edit").show();
    $("#tr" + id).children().find("button").hide();
    $("#tr" + id).children().find(".updateButton").show();
    $("#tr" + id).children().find(".cancelButton").show();
}

function updatePerson(id) {
    var personViewModel = {
        Id: id,
        FullName: $("#tr" + id).children().find(".fullName.edit").val(),
        Birthday: $("#tr" + id).children().find(".birthday.edit").val(),
        Email: $("#tr" + id).children().find(".email.edit").val(),
        PhoneMobile: $("#tr" + id).children().find(".phoneMobile.edit").val()
    };
    $.ajax({
        url: "/PersonsApi/UpdatePerson",
        data: { personViewModel: personViewModel },
        method: "POST",
        success: function (response) {
            if (response.success) {
                $("#tr" + id).children().find(".fullName.read").text(response.data.FullName);
                $("#tr" + id).children().find(".fullName.edit").val(response.data.FullName);
                $("#tr" + id).children().find(".birthday.read").text(ToJavaScriptDate(response.data.Birthday));
                $("#tr" + id).children().find(".birthday.edit").val(ToJavaScriptDate(response.data.Birthday));
                $("#tr" + id).children().find(".email.read").text(response.data.Email);
                $("#tr" + id).children().find(".email.edit").val(response.data.Email);
                $("#tr" + id).children().find(".phoneMobile.read").text(response.data.PhoneMobile);
                $("#tr" + id).children().find(".phoneMobile.edit").val(response.data.PhoneMobile);
                cancelUpdatePerson(id);
            }
            if (response.message)
                alert(response.message);
        },
        error: function (response) {
            alert("Ошибка при изменении!");
        }
    })
}

function cancelUpdatePerson(id) {
    $("#tr" + id).children().find(".edit").hide();
    $("#tr" + id).children().find(".read").show();
    $("#tr" + id).children().find("button").hide();
    $("#tr" + id).children().find(".editButton").show();
    $("#tr" + id).children().find(".deleteButton").show();
}

function deletePerson(id) {
    if (!confirm("Вы уверены, что хотите удалить запись?"))
        return;

    $.ajax({
        url: "/PersonsApi/DeletePerson",
        data: { personId: id },
        method: "POST",
        success: function (response) {
            if (response.success)
            {
                $("#tr"+id).remove();
            }
            if (response.message)
                alert(response.message);
        },
        error: function (response) {
            alert("Ошибка при удалении!");
        }
    })
}

function ToJavaScriptDate(value) {
    if (!value)
        return "";
    var pattern = /Date\(([^)]+)\)/;
    var results = pattern.exec(value);
    var dt = new Date(parseFloat(results[1]));
    var day = ((dt.getDate() < 10) ? "0" : "") + dt.getDate();
    var month = ((dt.getMonth() < 9) ? "0" : "") + (dt.getMonth() + 1);
    return day + "." + month + "." + dt.getFullYear();
}

function pageableTable(rowsShown)
{
    $('table').after('<div id="nav"></div>');
    var rowsTotal = $('table tbody tr').length;
    var numPages = rowsTotal / rowsShown;
    for (i = 0; i < numPages; i++) {
        var pageNum = i + 1;
        $('#nav').append('<a href="#" rel="' + i + '">' + pageNum + '</a> ');
    }
    $('table tbody tr').hide();
    $('table tbody tr').slice(0, rowsShown).show();
    $('#nav a:first').addClass('active');
    $('#nav a').bind('click', function () {
        $('#nav a').removeClass('active');
        $(this).addClass('active');
        var currPage = $(this).attr('rel');
        var startItem = currPage * rowsShown;
        var endItem = startItem + rowsShown;
        $('table tbody tr').css('opacity', '0.0').hide().slice(startItem, endItem).
            css('display', 'table-row').animate({ opacity: 1 }, 300);
    });
}

function showAddForm() {
    $("#addButton").hide();
    $(".addPerson").show();
}

function hideAddForm() {
    $(".addPerson").hide();
    $("#addButton").show();
}

function addPerson() {
    var personViewModel = {
        FullName: $(".fullName.add").val(),
        Birthday: $(".birthday.add").val(),
        Email: $(".email.add").val(),
        PhoneMobile: $(".phoneMobile.add").val()
    };

    $.ajax({
        url: "/PersonsApi/AddPerson",
        data: { personViewModel: personViewModel },
        method: "POST",
        success: function (response) {
            if (response.message)
                alert(response.message);
            if (response.success)
                location.reload();
        },
        error: function (response) {
            alert("Ошибка при добавлении!");
        }
    })
}