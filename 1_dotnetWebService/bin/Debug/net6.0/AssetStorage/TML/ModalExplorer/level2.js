ShowPageDetails("page", "Area Explorer");


//**************************** Document Ready function *********************************//

$(document).ready(function () {
  var getCall = new ApiGet(`/userDetails`);
  getCall.onSuccess = function onSuccess(result) {
    PageRefresh(result);
  };
  getCall.call();
});

//**************************** PageRefresh function *********************************//

function PageRefresh(userData) {
  var Area = getQueryStringValue("area");
  var Component = getQueryStringValue("comp");

  var getCall = new ApiGet(`/Test_model_explorer`);
  getCall.onSuccess = function onSuccess(result) {
    console.log("Test_model_explorer", result);
    var area = (userData.area || "").split(",");
    var plant = (userData.plant_location || "").split(",");
    var filterData;
    if (userData.role_id == 2 || userData.role_id == 14) {
      filterData = result;
    } else if (userData.role_id == 4) {
      filterData = result.filter(
        (ele) => area.includes(ele.area) && plant.includes(ele.plant)
      );
    } else {
      filterData = result.filter(
        (ele) => ele.plant && plant.includes(ele.plant)
      );
    }
    var result = filterData.filter(
      (ele) => ele.area == Area && ele.component == Component
    );
    $("#machine-layout").html(" ");
    AreaWiseData(result);
    PopulateModel(result);
    const Dayarr = ["7D", "6D", "5D", "4D", "3D", "2D", "1D"];

    Dayarr.forEach((ele, index) => {
      $("#days_id").append(
        `<option value="${ele}">${
          ele == "1D" ? `${ele}ay` : `${ele}ays`
        }</option>`
      );
    });

    $("#btnRefresh").click(function () {
      refreshfun(result);
    });
  };
  getCall.call();
}
//******************* AreaWiseData Function ************** */

function AreaWiseData(array) {
  let uniqueCombinations = {};

  for (let ele of array) {
    const key = `${ele.model}_${ele.operation}_${ele.cplan}_${ele.component}`;

    if (!uniqueCombinations[key]) {
      uniqueCombinations[key] = {
        model: ele.model,
        operation: ele.operation,
        cplan: ele.cplan,
        component: ele.component,
        area: ele.area,
        ok: 0,
        nok: 0,
        total: 0,
      };
    }

    if (ele.status === "OK") {
      uniqueCombinations[key].ok++;
    } else if (ele.status === "NOK") {
      uniqueCombinations[key].nok++;
    }

    uniqueCombinations[key].total =
      uniqueCombinations[key].OK + uniqueCombinations[key].NOK;
  }

  const result = Object.values(uniqueCombinations);

  for (var i = 0; i < result.length; i++) {
    showTimechart(result[i], i, array);
  }
  /*
  var obj = DataReducer(result);
  for (var i = 0; i < obj.length; i++) {
    showAlarm(obj[i], i);
  }
  */

  // guage_Chart(result[0]);
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
      values[ele.area] = {
        sr_no: i,
        area: ele.area,
        ok: (ele.ok / (ele.ok + ele.nok)) * 100,
        nok: (ele.nok / (ele.ok + ele.nok)) * 100,
      };
    }
  });
  var data = Object.values(values);

  guage_Chart(data[0]);
}

//******************* refreshfun Function ************** */

function refreshfun(result) {
  var model = $("#model_id :selected").val();
  var days = $("#days_id :selected").val();

  var filteredarray;

  if (model === "" && days === "") {
    filteredarray = result;
  } else {
    selected = days;
    selection = ["1D", "2D", "3D", "4D", "5D", "6D", "7D"];
    inSelection = selection.slice(0, selection.indexOf(selected) + 1);
    filteredarray = result.filter((ele) => {
      return (
        (model === "" || ele.model === model) &&
        (days === "" || inSelection.includes(ele.day))
      );
    });
  }

  $("#machine-layout").html("");
  $("#plant-machine-desc").html("");
  AreaWiseData(filteredarray);
}

//******************* PopulateModel Function ************** */

function PopulateModel(array) {
  const areaArr = [...new Set(array.map((x) => x.model))];
  $("#model_id").html(`<option value="">All</option>`);

  areaArr.forEach((ele, index) => {
    $("#model_id").append(`<option value="${ele}">${ele}</option>`);
  });
  // $("#days_id").html(`<option value="">All</option>`);
}
