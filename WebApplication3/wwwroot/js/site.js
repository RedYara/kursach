$(document).ready(function () {

    logoInput.onchange = evt => {
        const [file] = logoInput.files
        if (file) {
            logo.src = URL.createObjectURL(file);
            logo.hidden = false;
            logo.style.setProperty('max-height', '200px');
            logo.style.setProperty('max-width', '250px');

        }
    }
    function readURL(input) {
        if (input.files && input.files.length > 0) {
            for (var i = 0; i < input.files.length; i++) {
                var reader = new FileReader();
                $('#imgViewer').empty();
                reader.onload = function (e) {
                    $('#imgViewer').attr('src', e.target.result);
                    $('#imgViewer').append($('<img>', { src: e.target.result, width: '300px', height: '200px', class: "img-fluid col-auto" }));
                }
                reader.readAsDataURL(input.files[i]);
            }
        }
    }

    $("#scrinsUploader").change(function () {
        readURL(this);
    });

});

function createGroup() {
    var value = document.getElementById("groupName").value;
    $.ajax({
        url: '/groups/CreateGroupAjax?name=' + value,
        type: "Get",
        success: function (data) {
        },
        error: function (xhr, status, error) {
            console.log(error)
        },
        complete: function () {
            appendGroupList();
        }
    });
};
function createGroupType() {
    var value = document.getElementById("groupTypeName").value;
    var value2 = $("#groupTypeId").val();
    $.ajax({
        url: '/grouptypes/CreateGroupTypeAjax?name=' + value + '&groupId=' + value2,
        type: "Get",
        success: function (data) {
        },
        error: function (xhr, status, error) {
            console.log(error)
        },
        complete: function () {
            appendGroupList();
        }
    });
};
function appendGroupList() {
    $.ajax({
        url: '/apps/GetGroups',
        type: "Get",
        success: function (data) {
            $("#Groups").empty();
            $("#groupTypeId").empty();
            $.each(data, function (i, item) {
                if (groupId == item.id) {
                    $('#Groups').append($('<option>', {
                        value: item.id,
                        text: item.name,
                        selected: ""
                    }));
                }
                else {
                    $('#Groups').append($('<option>', {
                        value: item.id,
                        text: item.name
                    }));
                };
            })
        },
        error: function (xhr, status, error) {
            console.log(error)
        },
        complete: function () {
            appendGroupTypeList();
        }
    },
    )
};
function appendGroupTypeList() {
    var value = $("#Groups").val();
    $.ajax({
        url: '/apps/getgrouptypes?id=' + value,
        type: "Get",
        success: function (data) {
            $("#GroupTypes").empty();
            $.each(data, function (i, item) {
                if (groupTypeId == item.id) {
                    $('#GroupTypes').append($('<option>', {
                        value: item.id,
                        text: item.name,
                        selected: ""
                    }));
                }
                else {
                    $('#GroupTypes').append($('<option>', {
                        value: item.id,
                        text: item.name
                    }));
                };
            })
        },
        error: function (xhr, status, error) {
            console.log(error)
        },
        complete: function () {
        }
    });
};