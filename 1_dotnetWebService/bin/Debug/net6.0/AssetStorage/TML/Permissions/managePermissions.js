

const urlParams = new URLSearchParams(window.location.search);

var roleId = urlParams.get('role_id') || 0;

currentTarget = {}
//************************ Document Ready Function  *********************************//

$(document).ready(function () {

    getList(roleId)



})

//************************ getList Function  *********************************//


async function getList(roleId) {
    var getCall = new ApiGet(`/GetRolePermissionList?role_id=${roleId}`);
    getCall.onSuccess = (function onSuccess(data) {
        genratePermissionsTable(data)
        $("input.permission-access").change(function (event) {
            target = event.target
            data = {
                "role_id": roleId,
                "pr_id": Number(target.name),
                "pr_granted": target.checked,
                "type": 2 //1 add // 2 update
            }
            SaveRolePermissionInfo(data)
        })
    });
    await getCall.call();
}

//************************ onMarkChange Function  *********************************//


function onMarkChange(event) {
    target = event.target
    data = {
        "role_id": roleId,
        "pr_id": Number(target.name),
        "pr_granted": target.checked,
        "type": 2 //1 add // 2 update
    }
    SaveRolePermissionInfo(data)
}

//************************ genratePermissionsTable Function  *********************************//


async function genratePermissionsTable(data) {
    if (data.length) {
        $("#role_name").val(data[0].role_name).attr('readonly', (data[0].role_code == 'ADMIN' && 'readonly'))
        $("#role_code").val(data[0].role_code).attr('readonly', (data[0].role_code == 'ADMIN' && 'readonly'))
    }
    $("#managePermissionsTable").html('')

    await data.forEach(function (ele) {
        $("#managePermissionsTable").append(`
            <tr style="text-align:center;">
                <td>
                    ${ele.page_name}
                </td>
                <td>
                    ${ele.permission_name}
                </td>
                <td class="d-grid justify-center">
                    <div class="w-100 h-100" >
                <i style="font-size:18px; cursor:pointer;" class="fa fa-solid fa-${ele.pr_granted ? 'check' : 'xmark'} mx-4"></i>
                    </div>
                </td>
            </tr>
            `)
    })

}

//************************ SaveRolePermissionInfo Function  *********************************//


function SaveRolePermissionInfo(data) {
    var getCall = new ApiPost(`/SaveRolePermissionInfo`, data);
    getCall.onSuccess = (function onSuccess(response) {

        if (response == 1) {
            /*Swal.fire(
                'Success!',
                '',
                'success'
            )*/
            getList(roleId)
        }

    });
    getCall.call();
}