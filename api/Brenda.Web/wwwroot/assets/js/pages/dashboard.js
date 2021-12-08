"use strict";

var KTWidgets = function() {

    var _initMixedWidget18 = function () {
        var element = document.getElementById("kt_mixed_widget_18_chart");
        var height = parseInt(KTUtil.css(element, 'height'));

        if (!element) {
            return;
        }

        var options = {
            series: [74],
            chart: {
                height: height,
                type: 'radialBar',
                offsetY: 0
            },
            plotOptions: {
                radialBar: {
                    startAngle: -90,
                    endAngle: 90,

                    hollow: {
                        margin: 0,
                        size: "70%"
                    },
                    dataLabels: {
                        showOn: "always",
                        name: {
                            show: true,
                            fontSize: "13px",
                            fontWeight: "700",
                            offsetY: -5,
                            color: KTApp.getSettings()['colors']['gray']['gray-500']
                        },
                        value: {
                            color: KTApp.getSettings()['colors']['gray']['gray-700'],
                            fontSize: "30px",
                            fontWeight: "700",
                            offsetY: -40,
                            show: true
                        }
                    },
                    track: {
                        background: KTApp.getSettings()['colors']['theme']['light']['primary'],
                        strokeWidth: '100%'
                    }
                }
            },
            colors: [KTApp.getSettings()['colors']['theme']['base']['primary']],
            stroke: {
                lineCap: "round",
            },
            labels: ["Disponibilidade"]
        };

        var chart = new ApexCharts(element, options);
        chart.render();
    }

    var _initMixedWidget2 = function () {

        var element = document.getElementById("kt_mixed_widget_2_chart");
        var height = parseInt(KTUtil.css(element, 'height'));

        if (!element) {
            return;
        }

        var strokeColor = '#287ED7';

        var options = {
            series: [{
                name: 'Requisições',
                data: requestsY
            }],
            chart: {
                type: 'area',
                height: height,
                toolbar: {
                    show: false
                },
                zoom: {
                    enabled: false
                },
                sparkline: {
                    enabled: true
                },
                dropShadow: {
                    enabled: true,
                    enabledOnSeries: undefined,
                    top: 5,
                    left: 0,
                    blur: 3,
                    color: strokeColor,
                    opacity: 0.5
                }
            },
            plotOptions: {},
            legend: {
                show: false
            },
            dataLabels: {
                enabled: false
            },
            fill: {
                type: 'solid',
                opacity: 0
            },
            stroke: {
                curve: 'smooth',
                show: true,
                width: 3,
                colors: [strokeColor]
            },
            xaxis: {
                categories: requestsX,
                axisBorder: {
                    show: false,
                },
                axisTicks: {
                    show: false
                },
                labels: {
                    show: false,
                    style: {
                        colors: KTApp.getSettings()['colors']['gray']['gray-500'],
                        fontSize: '12px',
                        fontFamily: KTApp.getSettings()['font-family']
                    }
                },
                crosshairs: { show: false },
                tooltip: {
                    enabled: false,
                    formatter: undefined,
                    offsetY: 0,
                    style: {
                        fontSize: '12px',
                        fontFamily: KTApp.getSettings()['font-family']
                    }
                }
            },
            yaxis: {
                min: -10,
                max: requestsMax + 1,
                labels: {
                    show: false,
                    style: {
                        colors: KTApp.getSettings()['colors']['gray']['gray-500'],
                        fontSize: '12px',
                        fontFamily: KTApp.getSettings()['font-family']
                    }
                }
            },
            states: {
                normal: {
                    filter: {
                        type: 'none',
                        value: 0
                    }
                },
                hover: {
                    filter: {
                        type: 'none',
                        value: 0
                    }
                },
                active: {
                    allowMultipleDataPointsSelection: false,
                    filter: {
                        type: 'none',
                        value: 0
                    }
                }
            },
            tooltip: {
                style: {
                    fontSize: '12px',
                    fontFamily: KTApp.getSettings()['font-family']
                },
                
                marker: {
                    show: false
                }
            },
            colors: ['transparent'],
            markers: {
                colors: [KTApp.getSettings()['colors']['theme']['light']['info']],
                strokeColor: [strokeColor],
                strokeWidth: 3
            }
        };

        var chart = new ApexCharts(element, options);
        chart.render();
    }

    var _initMixedWidget16 = function () {
        var element = document.getElementById("kt_mixed_widget_16_chart");
        var height = parseInt(KTUtil.css(element, 'height'));

        if (!element) {
            return;
        }

        var options = {
            series: [60, 50, 75, 80],
            chart: {
                height: height,
                type: 'radialBar',
            },
            plotOptions: {
                radialBar: {
                    hollow: {
                        margin: 0,
                        size: "30%"
                    },
                    dataLabels: {
                        showOn: "always",
                        name: {
                            show: false,
                            fontWeight: "700",
                        },
                        value: {
                            color: KTApp.getSettings()['colors']['gray']['gray-700'],
                            fontSize: "18px",
                            fontWeight: "700",
                            offsetY: 10,
                            show: true
                        },
                        total: {
                            show: true,
                            label: 'Total',
                            fontWeight: "bold",
                            formatter: function (w) {
                                // By default this function returns the average of all series. The below is just an example to show the use of custom formatter function
                                return '60%';
                            }
                        }
                    },
                    track: {
                        background: KTApp.getSettings()['colors']['gray']['gray-100'],
                        strokeWidth: '100%'
                    }
                }
            },
            colors: [
                KTApp.getSettings()['colors']['theme']['base']['info'],
                KTApp.getSettings()['colors']['theme']['base']['danger'],
                KTApp.getSettings()['colors']['theme']['base']['success'],
                KTApp.getSettings()['colors']['theme']['base']['primary']
            ],
            stroke: {
                lineCap: "round",
            },
            labels: ["Progresso"]
        };

        var chart = new ApexCharts(element, options);
        chart.render();
    }

    return {
        init: function() {
            _initMixedWidget2();
            _initMixedWidget18();
            _initMixedWidget16();
        }
    }
}();

if (typeof module !== 'undefined') {
    module.exports = KTWidgets;
}

jQuery(document).ready(function() {
    KTWidgets.init();
});
