ShowPageDetails("page", "Area Explorer");

//****************************** Document Ready Function  *************************************//

$(document).ready(function () {
  var getCall = new ApiGet(`/userDetails`);
  getCall.onSuccess = function onSuccess(result) {
    PageRefresh(result);
  };
  getCall.call();
});

function linkFor_model(area, comp) {
  var url = `../ModalExplorer/index.html?area=${area}&comp=${comp}`;

  window.location.href = url;
}

function PageRefresh(userData) {
  //****************************** Title *************************************//
  var plantName =
    userData.role_id == 2 || userData.role_id == 14
      ? "All"
      : userData.plant_location;

  var getplant = getQueryStringValue("plant");
  $(`#AreaExploreTitle`).append(
    `<h6 class="text-white text-capitalize ps-3">${
      getplant ? getplant : plantName
    } : Area Wise PIST Measurement Status   | (PIST : Percentage of Inspection points Satisfying Tolerance) </h6>`
  );

  //****************************** API Call*************************************//

  var getCall = new ApiGet(`/Test_area_explorer`);
  getCall.onSuccess = function onSuccess(result) {
    console.log("Test_area_explore", result);
    var filterData;
    var area = (userData.area || "").split(",");
    var plant = (userData.plant_location || "").split(",");

    if (userData.role_id == 2 || userData.role_id == 14) {
      if (getQueryStringValue("plant")) {
        filterData = result.filter(
          (ele) => ele.plant == getQueryStringValue("plant")
        );
      } else {
        filterData = result;
      }
    } else if (userData.role_id == 4) {
      filterData = result.filter(
        (ele) => area.includes(ele.area) && plant.includes(ele.plant)
      );
    } else {
      if (getQueryStringValue("plant")) {
        filterData = result.filter(
          (ele) => ele.plant == getQueryStringValue("plant")
        );
      } else {
        filterData = result.filter((ele) => plant.includes(ele.plant));
      }
    }

    var data = GuageSummary(filterData);
    $("#divChart").html("");

    PopulateArea(filterData);
    //****************************** Generate Dropdown for No of Days *************************************//

    const Dayarr = ["7D", "6D", "5D", "4D", "3D", "2D", "1D"];

    Dayarr.forEach((ele, index) => {
      $("#days_id").append(
        `<option value="${ele}">${
          ele == "1D" ? `${ele}ay` : `${ele}ays`
        }</option>`
      );
    });

    for (var i = 0; i < data.length; i++) {
      var oper = result.filter(function (ele) {
        return ele.area == data[i].area;
      });

      showGuageline(data[i], oper);
    }
    $("#btnRefresh").click(function () {
      $("#divChart").html("");
      refreshfun(filterData, data);
    });
  };
  getCall.call();
}

//****************************** Refresh button  *************************************//

function refreshfun(result, data) {
  var area = $("#Area_id :selected").val();
  var days = $("#days_id :selected").val();

  var filteredarray = [];

  if (area === "" && days === "") {
    filteredarray = result;
  } else {
    selected = days;
    selection = ["1D", "2D", "3D", "4D", "5D", "6D", "7D"];
    inSelection = selection.slice(0, selection.indexOf(selected) + 1);
    filteredarray = result.filter((ele) => {
      return (
        (area === "" || ele.area === area) &&
        (days === "" || inSelection.includes(ele.day))
      );
    });
  }

  var data1 = GuageSummary(filteredarray);
  for (var i = 0; i < data1.length; i++) {
    var oper = filteredarray.filter(function (ele) {
      return ele.area === data1[i].area;
    });
    showGuageline(data1[i], oper);
  }
}

//****************************** Generate Dropdown for Area *************************************//

function PopulateArea(array) {
  const areaArr = [...new Set(array.map((x) => x.area))];

  $("#Area_id").html("");
  $("#Area_id").html(`<option value="">All</option>`);
  areaArr.forEach((ele, index) => {
    $("#Area_id").append(`<option value="${ele}">${ele}</option>`);
  });
}

//****************************** Generate HTML for Main  *************************************//

function showGuageline(data, obj) {
  var myvar = ` <div class="col-sm-5 m-3  align-items-center   area-container " >
                         <div class="chartClass" id="chart-container${data.sr_no}"></div>
                            <div class="row  align-items-center line-status-container" id="machine-line-id${data.sr_no}"> </div>
                      </div>`;

  $("#divChart").append(myvar);
  var index = data.sr_no;
  guageChart(data);
  $(`#machine-line-id${index}`).html("");
  var comp_obj = DataReducer(obj);

  for (var i = 0; i < comp_obj.length; i++) {
    display_alarm(comp_obj[i], index, i);
  }
}

//*************************************  Generate HTML for display_alarm *********************************************//
function display_alarm(obj, index, i) {
  var okavg = (obj.ok / (obj.ok + obj.nok)) * 100;
  var nokavg = (obj.nok / (obj.ok + obj.nok)) * 100;

  var machine_linedata = `<div class="col-sm-12">
                                    <div class="line-layout row">
                                      <span class="line-machine col-sm-4 px-0">${
                                        obj.component
                                      }</span>
                                      <div class="col-sm-2 overall-status"></div>
                                      <div class="col-sm-4 px-0 wrap cursor-pointer user-select-none" onclick="linkFor_model('${
                                        obj.area
                                      }', '${obj.component}')">
                                        <span class="split-green" title="${
                                          obj.ok
                                        }" style="width:${
    okavg ? Math.round(okavg) : 0
  }%;">${obj.ok}</span>
                                        <span class="split-red" title="${
                                          obj.nok
                                        }" style="width:${
    nokavg ? Math.round(nokavg) : 0
  }%;">${obj.nok}</span>
                                      </div>
                                    </div>
                                    </div>
                                    `;

  $(`#machine-line-id${index}`).append(machine_linedata);

  //var op_status_ele = $('.overall-status')[i];
  var op_status_ele = $(`#machine-line-id${index}`).find(".overall-status")[i];

  var op_status = obj.nok == 0 ? "ACCEPTED" : "REJECTED";

  if (op_status == "REJECTED") {
    $(op_status_ele).css("background-color", "var(--red)");
  } else if (op_status == "WARNING") {
    $(op_status_ele).css("background-color", "var(--yellow)");
  } else {
    $(op_status_ele).css("background-color", "var(--green)");
  }
}

//************************************* Guage Chart *********************************************//

function guageChart(obj) {
  //Guage chart Starting
  var dom = document.getElementById(`chart-container${obj.sr_no}`);
  var myChart = echarts.init(dom, {
    renderer: "svg",
    useDirtyRect: false,
  });

  $(dom).attr("guage_chart_sr_no", obj.sr_no);
  var nok = obj == undefined ? 100 : (obj.nok / (obj.ok + obj.nok)) * 100;
  var val = obj == undefined ? 0 : (obj.ok / (obj.ok + obj.nok)) * 100;
  var app = {};
  var option;

  option = {
    series: [
      {
        type: "gauge",
        axisLine: {
          lineStyle: {
            width: 12,
            color: [
              [nok ? nok / 100 : 1, "#f44335"],
              [1, " #4caf50"],
            ],
          },
        },
        pointer: {
          show: false,
          itemStyle: {
            color: "inherit",
          },
        },
        axisTick: {
          distance: -15,
          length: 8,
          lineStyle: {
            color: "#fff",
            width: 0.5,
          },
        },
        splitLine: {
          distance: -30,
          length: 30,
          lineStyle: {
            color: "#fff",
            width: 4,
          },
        },
        axisLabel: {
          color: "inherit",
          distance: 15,
          fontSize: 12,
        },
        detail: {
          fontSize: 15,
          valueAnimation: true,
          formatter: "{value}%",
          color: "inherit",
        },
        data: [
          {
            name: obj == undefined ? "no data found" : obj.area,

            value: val ? Math.trunc(val) : 0,

            title: {
              fontWeight: "bold",
              //lineHeight: 2,
              fontSize: 14,
              offsetCenter: ["0", "0"],
              //color: '#9B728E'
            },
          },
        ],
        center: ["50%", "50%"],
        radius: "90%",
      },
    ],
  };

  if (option && typeof option === "object") {
    myChart.setOption(option);
  }
  window.addEventListener("resize", myChart.resize);

  // Guage chart End
}

//**************** Genrate HTML for Guage *****************/Gu

function GuageSummary(result) {
  isExists = [];
  var values = {};
  var inputs = result.forEach(function (ele, i) {
    if (isExists.includes(ele.area)) {
      values[ele.area] = {
        sr_no: i,
        area: ele.area,
        ok: values[ele.area].ok + ele.ok,
        nok: values[ele.area].nok + ele.nok,
      };
    } else {
      isExists.push(ele.area);
      var avgok = Math.trunc((ele.ok / (ele.ok + ele.nok)) * 100);
      var avgnok = Math.trunc((ele.nok / (ele.ok + ele.nok)) * 100);
      values[ele.area] = {
        sr_no: i,
        area: ele.area,
        ok: avgok ? avgok : 0,
        nok: avgnok ? avgnok : 0,
      };
    }
  });
  var data = Object.values(values);

  return data;
}
