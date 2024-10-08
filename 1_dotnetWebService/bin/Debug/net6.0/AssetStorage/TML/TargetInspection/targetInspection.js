ShowPageDetails("page", "Target Inspection");


//****************************** DataTable Generator for   *************************************//

var dataTable = $("#char-data-table").DataTable({
    paging: true,
    searching: true,
    info: false,
    columnDefs: [
        {
            render: function (data, type, full, meta) {
                return ` <div class="d-flex px-3 py-1">
                                     <div class="d-flex flex-column justify-content-center">
                                         <h6 class="mb-0 text-sm">${full.part_desc}</h6>
                                             <p class="text-sm font-weight-normal text-secondary mb-0"><span class="text-dark">${full.component}  ${full.model}</span> </p>
                                      </div>
                                 </div>`;
            },
            targets: 0,
        },
        {
            render: function (data, type, full, meta) {
                return ` <p class="text-sm font-weight-normal mb-0 ">${full.area}</p>`;
            },
            targets: 1,
        },
        {
            render: function (data, type, full, meta) {
                return ` <p class="text-sm font-weight-normal mb-0 ">${full.operations}</p>`;
            },
            targets: 2,
        },
        {
            render: function (data, type, full, meta) {
                return `<p class="text-sm font-weight-normal mb-0 d-flex px-3 py-1 justify-content-center align-items-center">${full.targeted_value == "" ? `undefined` : full.targeted_value  }</p>`;
            },
            targets: 3,
        },
        {
            render: function (data, type, full, meta) {
                return ` <p class="text-sm font-weight-normal mb-0 d-flex px-3 py-1 justify-content-center align-items-center">${roundNumber(
                    full.base_value
                )}</p>`;
            },
            targets: 4,
        },
        {
            render: function (data, type, full, meta) {
                return `  <p class="text-sm font-weight-normal mb-0 d-flex px-3 py-1 justify-content-center align-items-center">${roundNumber(
                    full.actual_value
                   )}</p>`;
            },
            targets: 5,
        },
        {
            render: function (data, type, full, meta) {
                return `<div class="d-flex px-3 py-1 justify-content-center align-items-center">
                                         <p class="text-sm font-weight-normal mb-0">${full.actual_value -
                    full.targeted_value
                    }</p>
                                         ${full.targeted_value - full.actual_value < 0 ? `<i class="ni ni-bold-up text-sm ms-1 text-success"></i>`
                        : ` <i class="ni ni-bold-down text-sm ms-1 text-danger"></i>`
                    } </div>`;
            },
            targets: 6,
        },

    ],
    language: {
        paginate: {
            next: "&#8594;", // or '→'
            previous: "&#8592;", // or '←'
        },
    },
});

//****************************** Document Ready Function  *************************************//
$(document).ready(function () {
    var getCall = new ApiGet(`/userDetails`);
    getCall.onSuccess = function onSuccess(result) {
        processRefresh(result);
    }
    getCall.call();

});

//****************************** processRefresh Function  *************************************//

function processRefresh(userData) {
    GetCharacteristicsInfo();
    async function GetCharacteristicsInfo() {
        var plantName = GetPlants().responseText;
        var getCall = new ApiGet(`/Test_Inspection`);
        getCall.onSuccess = function onSuccess(result) {
            console.log("/Test_Inspection", result);
            var filterData;
            var area = (userData.area || '').split(',')
            var plant = (userData.plant_location || '').split(',')

            if (userData.role_id == 2 || userData.role_id == 14) {
                filterData = result;
            }
            else if (userData.role_id == 4) {
                filterData = result.filter((ele) => area.includes(ele.area) && plant.includes(ele.plant));
            }
            else {
                filterData = result.filter((ele) => plant.includes(ele.plant));
            }

            var FromDate = moment(filterData[0].from_date).format("DD-MM-YYYY");
            var ToDate = moment(filterData[0].to_date).format("DD-MM-YYYY");
            //$(format.date(result[0].to_date, "dd-MM-yyyy"));

            $("#target_title").append(
                `<h6 class="text-white text-capitalize ps-3">Inspection report From: ${FromDate} To: ${ToDate} </h6>`
            );
            dataTable.clear().draw();
            dataTable.rows.add(result);
            dataTable.columns.adjust().draw();
        };
        getCall.call();
    }
}
