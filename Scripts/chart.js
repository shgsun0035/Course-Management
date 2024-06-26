$(document).ready(function () {
    var labelSet = [];
    var dataSet = [];
    var barBackgroundColorSet = [];
    var barBorderColorSet = [];

    // generate random color
    var dynamicColor = function () {
        var r = Math.floor(Math.random() * 255);
        var g = Math.floor(Math.random() * 255);
        var b = Math.floor(Math.random() * 255);
        return "rgb(" + r + "," + g + "," + b;
    };

    $(".chartModelData").each(function () {

        // get the data
        var courseName = $(".courseName", this).text().trim();
        var bookingAmount = $(".bookingAmount", this).text().trim();
        var intBookingAmount = parseInt(bookingAmount);

        labelSet.push(courseName);
        dataSet.push(intBookingAmount);

        // randomly assign different colors to different courses
        var generatedDynamicColor = dynamicColor();
        var dynamicBackgroundColor = generatedDynamicColor + ", 0.2)";
        var dynamicBorderColor = generatedDynamicColor + ", 1)";
        barBackgroundColorSet.push(dynamicBackgroundColor);
        barBorderColorSet.push(dynamicBorderColor);

    });

    var ctx = document.getElementById('myChart').getContext('2d');

    // display of the bar chart
    var chart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labelSet,
            datasets: [{
                data: dataSet,
                backgroundColor: barBackgroundColorSet,
                borderColor: barBorderColorSet,
                borderWidth: 1
            }]
        },
        options: {
            legend: {
                labels: {
                    generateLabels: function (chart) {
                        var labels = chart.data.labels;
                        var dataset = chart.data.datasets[0];
                        var legend = labels.map(function (label, index) {
                            return {
                                datasetIndex: 0,
                                text: label,
                                fillStyle: dataset.backgroundColor[index],
                                strokeStyle: dataset.borderColor[index],
                                lineWidth: 1
                            }
                        });
                        return legend;
                    }
                }
            }
        }
    });
});