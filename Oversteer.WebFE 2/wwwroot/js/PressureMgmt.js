$(document).ready(function () {


    $('#load_setups').click(function () {
        LoadSetups()
    });

    $('body').on('click', '.deleteSetup', function () {
        var clickedBtnID = $(this).attr('setupId'); // or var clickedBtnID = this.id
        alert('you clicked on button #' + clickedBtnID);
    });

    $('#new_setup').click(function () {
        $.ajax({
            url: 'PressureManagement?handler=CreateNewSetup',
            success: function (response) {
                $('#modalcontent').html(response);
                $('#driverdetails').modal('show').css('overflow-y', 'auto');
            },
            error: function (error) {
                // handle error code
            }

        });
    });


    function LoadSetups() {
        $.ajax({
            type: "GET",
            url: 'PressureManagement?handler=LoadSetup',
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
                $('.setupsTable').DataTable().clear().draw();
                $('.setupsTable').DataTable().destroy();
            },
            data: { "trackId": $('#SelectedTrack').val(), "carId": $('#carModel').val() },
            success: function (setups) {
                var html = "";

                if (setups.length > 0) {
                    for (i = 0; i < setups.length; i++) {
                        html += "<tr>";
                        html += "<td>" + setups[i].setupName + "</td><td>" + setups[i].tp_FL + "</td><td>" + setups[i].tp_FR + "</td><td>" + setups[i].tp_RL + "</td><td>" + setups[i].tp_RR + "</td><td>" + setups[i].envTemp + "</td><td>" + setups[i].trackTemp + "</td><td>" + setups[i].fuelLoad + "</td><th>" + setups[i].isWet + "</td><td><img src=\"../images/editing.png\" width=\"16\" height=\"16\" style=\"margin-right:10px\"/><img src=\"../images/bin.png\" width=\"16\" height=\"16\" class=\"deleteSetup\" setupId=\"" + setups[i].setupId + "\"/></td></tr>";
                    }
                    console.log(html);

                    $(html).appendTo('.setupsTable tbody');

                    $("#setupTableContainer").addClass('tableVisible');
                    $("#setupTableContainer").removeClass('tableHidden');

                    $("#emptyTablePlaceholder").removeClass('tableVisible');
                    $("#emptyTablePlaceholder").addClass('tableHidden');
                    $('.setupsTable').DataTable();
                }
                else {
                    $("#setupTableContainer").addClass('tableHidden');
                    $("#setupTableContainer").removeClass('tableVisible');

                    $("#emptyTablePlaceholder").removeClass('tableHidden');
                    $("#emptyTablePlaceholder").addClass('tableVisible');
                }
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    }


});