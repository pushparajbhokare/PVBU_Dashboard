//************************ Documnet Ready Function  *********************************//

$(document).ready(function () {
    getList()
    GetUserDetails();
    var getCall = new ApiGet(`/userDetails`);
    getCall.onSuccess = function onSuccess(result) {
        addHead("Role List", result);
    };
})


//************************ add/Cancle/Save Role Click  *********************************//

$("#addRole").click(function () {
    $("#addrolemodal").modal("show")
})
$("#cancelRole").click(function () {
    $("#addrolemodal").modal("hide")
})
$("#saveRole").click(function () {
    $("#addrolemodal").modal("hide")
    role_name = $("#role_name").val()
    role_code = $("#role_code").val()
    landing_page = $("#pagesDropDown").val()
    data = {
        role_name,
        role_code,
        landing_page,
        type: 1
    }
    AddUpdateRoleInfo(data)

})

//************************ getList Function  *********************************//

function getList() {
    var getCall = new ApiGet(`/GetRoleList`);
    getCall.onSuccess = (function onSuccess(data) {
        genrateRoleTable(data)
    });
    getCall.call();
}
//************************ genrateRoleTable Function  *********************************//

function genrateRoleTable(data) {
    $("#manageroleTable").html('')
    data.forEach(function (ele) {
        $("#manageroleTable").append(`
            <tr style="text-align:center;">
                <td>
                    ${ele.role_name}
                </td>
                <td>
                    ${ele.role_code}
                </td>
                <td class="d-grid justify-center">
                    <div class="w-100 h-100" >

             <a href="./managePermission.html?role_id=${ele.role_id}"><i style="font-size:18px; cursor:pointer;" class=" "><img src="./eye-regular.svg" style="width:25px"/></i><a/>

                    </div>
                </td>
            </tr>
         `)
    })
}

//************************ DeleteRoleByInfo Function  *********************************//

function DeleteRoleByInfo(role_id) {
    if (confirm('Are you sure???')) {
        var getCall = new ApiPost(`/DeleteRoleInfo?role_id=${role_id}`, { role_id });
        getCall.onSuccess = (function onSuccess(response) {
            if (response == -1) {
                alert("Role is in use. Cannot delete!!!")
            }
            getList()
        });
        getCall.call();
    }
}

//************************ AddUpdateRoleInfo Function  *********************************//


function AddUpdateRoleInfo(data) {
    if (data.role_name == "" || data.role_code == "") {
        alert("Please fill up all the fields before submitting...")
        return
    }
    var getCall = new ApiPost(`/AddUpdateRoleInfo`, data)
    getCall.onSuccess = (function onSuccess(response) {
        getList()
    });
    getCall.call();
}
